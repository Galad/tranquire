using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Tranquire;

/// <summary>
/// Contains factories to create <see cref="IActionTags{TTag}"/>
/// </summary>
public static class ActionTags
{

    /// <summary>
    /// Creates a new instance of action by specifying the tags in order of execution
    /// </summary>
    /// <param name="tags">The available tags. The first tag is most likely to be chosen by <see cref="IActionTags{TTag}.FindBestWhenTag(IEnumerable{TTag})"/> while the last one is most likely to be chosen by <see cref="IActionTags{TTag}.FindBestGivenTag(IEnumerable{TTag})"/></param>
    /// <returns></returns>
    public static ActionTags<TTag> Create<TTag>(params TTag[] tags)
    {
        if (tags == null)
        {
            throw new ArgumentNullException(nameof(tags));
        }

        var whenTags = tags.Select((tag, index) => (tag, index)).ToImmutableDictionary(t => t.tag, t => t.index);
        var givenTags = tags.Reverse().Select((tag, index) => (tag, index)).ToImmutableDictionary(t => t.tag, t => t.index);
        return new ActionTags<TTag>(whenTags, givenTags);
    }
}

/// <summary>
/// Default implementation of <see cref="IActionTags{TTag}"/>. It takes the known tags and their priority and find the best tags from it.
/// </summary>
public class ActionTags<TTag> : IActionTags<TTag>
{
    /// <summary>
    /// Gets the tags used to find the best When tag
    /// </summary>
    public ImmutableDictionary<TTag, int> WhenTags { get; private set; }

    /// <summary>
    /// Gets the tags used to find the best Given tag
    /// </summary>
    public ImmutableDictionary<TTag, int> GivenTags { get; private set; }

    /// <summary>
    /// Creates a new instance of <see cref="ActionTags{TTag}"/>
    /// </summary>
    /// <param name="whenTags">The tag priorities to use for <see cref="FindBestWhenTag(IEnumerable{TTag})"/></param>
    /// <param name="givenTags">The tag priorities to use for <see cref="FindBestGivenTag(IEnumerable{TTag})"/></param>
    public ActionTags(ImmutableDictionary<TTag, int> whenTags, ImmutableDictionary<TTag, int> givenTags)
    {
        WhenTags = whenTags ?? throw new ArgumentNullException(nameof(whenTags));
        GivenTags = givenTags ?? throw new ArgumentNullException(nameof(givenTags));
    }

    /// <inheritdoc />
    public TTag FindBestWhenTag(IEnumerable<TTag> tags)
    {
        return GetBestTag(WhenTags, tags);
    }

    /// <inheritdoc />
    public TTag FindBestGivenTag(IEnumerable<TTag> tags)
    {
        return GetBestTag(GivenTags, tags);
    }

    private TTag GetBestTag(
        ImmutableDictionary<TTag, int> orderedTags,
        IEnumerable<TTag> tags)
    {
        if (tags == null)
        {
            throw new ArgumentNullException(nameof(tags));
        }

        var result = tags.Select(FindTag)
                         .Where(t => t.HasValue)
                         .OrderBy(tag => tag.Value.index)
                         .FirstOrDefault();
        if (!result.HasValue)
        {
            var tagsString = string.Join(", ", tags.Select(t => t.ToString()));
            throw new KeyNotFoundException($"None of the tag matched a known tag. You might be using a tagged action that use tags that were not prioritized. The tags from this action are: ${tagsString}");
        }
        return result.Value.tag;

        (TTag tag, int index)? FindTag(TTag t)
        {
            if (!orderedTags.TryGetValue(t, out var order))
            {
                return null;
            }
            return (t, order);
        }
    }

    /// <summary>
    /// Add a tag for <see cref="FindBestWhenTag(IEnumerable{TTag})"/> with the highest priority
    /// </summary>
    /// <param name="tag">The tag to reference</param>
    /// <returns></returns>
    public ActionTags<TTag> AddFirstPriorityWhenTag(TTag tag)
    {
        if (tag == null)
        {
            throw new ArgumentNullException(nameof(tag));
        }

        var minIndex = WhenTags.Values.Min();
        return new ActionTags<TTag>(WhenTags.SetItem(tag, minIndex - 1), GivenTags);
    }

    /// <summary>
    /// Add a tag for <see cref="FindBestWhenTag(IEnumerable{TTag})"/> with the highest priority
    /// </summary>
    /// <param name="tag">The tag to reference</param>
    /// <returns></returns>
    public ActionTags<TTag> AddFirstPriorityGivenTag(TTag tag)
    {
        if (tag == null)
        {
            throw new ArgumentNullException(nameof(tag));
        }
        var minIndex = GivenTags.Values.Min();
        return new ActionTags<TTag>(WhenTags, GivenTags.SetItem(tag, minIndex - 1));
    }
}
