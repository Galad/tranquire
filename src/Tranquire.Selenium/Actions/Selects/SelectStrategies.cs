using OpenQA.Selenium.Support.UI;

namespace Tranquire.Selenium.Actions.Selects
{
    /// <summary>
    /// Select the value
    /// </summary>
    public sealed class SelectByValueStrategy : ISelectStrategy<string>
    {
        /// <summary>
        /// Perform the selection
        /// </summary>
        /// <param name="selectElement"></param>
        /// <param name="value"></param>
        public void Select(SelectElement selectElement, string value) => selectElement.SelectByValue(value);
    }

    /// <summary>
    /// Select the index
    /// </summary>
    public sealed class SelectByIndexStrategy : ISelectStrategy<int>
    {
        /// <summary>
        /// Perform the selection
        /// </summary>
        /// <param name="selectElement"></param>
        /// <param name="value"></param>
        public void Select(SelectElement selectElement, int value) => selectElement.SelectByIndex(value);
    }

    /// <summary>
    /// Select the text
    /// </summary>
    public sealed class SelectByTextStrategy : ISelectStrategy<string>
    {
        /// <summary>
        /// Perform the selection
        /// </summary>
        /// <param name="selectElement"></param>
        /// <param name="value"></param>
        public void Select(SelectElement selectElement, string value) => selectElement.SelectByText(value);
    }
}
