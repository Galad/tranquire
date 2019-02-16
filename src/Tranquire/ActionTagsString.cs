using System.Collections.Generic;
using System.Collections.Immutable;

namespace Tranquire
{
    /// <summary>
    /// A <see cref="ActionTags{TTag}"/> implementation that use string tags
    /// </summary>
    public class ActionTagsString : ActionTags<string>
    {
        /// <summary>
        /// Creates a new instance of <see cref="ActionTagsString"/>
        /// </summary>
        /// <param name="whenTags">The tag priorities to use for <see cref="IActionTags{TTag}.FindBestWhenTag(IEnumerable{TTag})"/></param>
        /// <param name="givenTags">The tag priorities to use for <see cref="IActionTags{TTag}.FindBestGivenTag(IEnumerable{TTag})"/></param>
        public ActionTagsString(ImmutableDictionary<string, int> whenTags, ImmutableDictionary<string, int> givenTags) : base(whenTags, givenTags)
        {
        }
    }
}
