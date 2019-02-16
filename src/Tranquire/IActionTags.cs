using System.Collections.Generic;

namespace Tranquire
{
    /// <summary>
    /// Provides a way to find the best tag to use among a list of tags
    /// </summary>
    public interface IActionTags<TTag>
    {
        /// <summary>
        /// Find the best tag to use among the tags passed as parameter
        /// </summary>
        /// <param name="tags">The list of tags to find from</param>
        /// <returns></returns>
        TTag FindBestGivenTag(IEnumerable<TTag> tags);
        /// <summary>
        /// Find the best tag to use among the tags passed as parameter
        /// </summary>
        /// <param name="tags">The list of tags to find from</param>
        /// <returns></returns>
        TTag FindBestWhenTag(IEnumerable<TTag> tags);
    }
}
