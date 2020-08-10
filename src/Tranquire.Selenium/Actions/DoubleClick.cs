using Tranquire.Selenium.Actions.Clicks;

namespace Tranquire.Selenium.Actions
{
    /// <summary>
    /// Create double click actions
    /// </summary>
    public static class DoubleClick
    {
        /// <summary>
        /// Create an action that double clicks on the given target
        /// </summary>
        /// <param name="target">The target to </param>
        /// <returns></returns>
        public static DoubleClickOnAction On(ITarget target)
        {
            return new DoubleClickOnAction(target);
        }
    }
}
