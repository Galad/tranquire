using System.Collections.Immutable;
using System.Linq;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Targets;

/// <summary>
/// Represent a relative target
/// </summary>
public class RelativeTarget : ITarget
{
    /// <summary>
    /// Creates a new instance of <see cref="RelativeTarget"/>
    /// </summary>
    /// <param name="targetSource">The source that is used as the search context</param>
    /// <param name="target">The target to look for</param>
    public RelativeTarget(ITarget targetSource, ITarget target)
    {
        TargetSource = targetSource ?? throw new System.ArgumentNullException(nameof(targetSource));
        Target = target ?? throw new System.ArgumentNullException(nameof(target));
    }

    /// <inheritdoc />
    public string Name => $"{Target.Name} relative to {TargetSource.Name}";

    /// <summary>
    /// Gets the source target that is used as the search context
    /// </summary>
    public ITarget TargetSource { get; }
    /// <summary>
    /// Gets the target
    /// </summary>
    public ITarget Target { get; }

    /// <inheritdoc />
    public ITarget RelativeTo(ITarget targetSource)
    {
        return new RelativeTarget(targetSource, this);
    }

    /// <inheritdoc />
    public ImmutableArray<IWebElement> ResolveAllFor(ISearchContext searchContext)
    {
        if (searchContext == null)
        {
            throw new System.ArgumentNullException(nameof(searchContext));
        }

        var elements = TargetSource.ResolveAllFor(searchContext);
        return elements.SelectMany(e => Target.ResolveAllFor(e)).ToImmutableArray();
    }

    /// <inheritdoc />
    public IWebElement ResolveFor(ISearchContext searchContext)
    {
        if (searchContext == null)
        {
            throw new System.ArgumentNullException(nameof(searchContext));
        }

        var element = TargetSource.ResolveFor(searchContext);
        return Target.ResolveFor(element);
    }

    /// <inheritdoc />
    public override string ToString() => Name;
}