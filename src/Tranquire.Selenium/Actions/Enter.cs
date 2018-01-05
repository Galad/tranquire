using Tranquire.Selenium.Actions.Enters;

namespace Tranquire.Selenium.Actions
{
    /// <summary>
    /// Creates input keys actions
    /// </summary>
    public class Enter : TargetableAction<EnterValue>
    {
        /// <summary>
        /// Gets the value to enter
        /// </summary>
        public string Value
        {
            get;
        }

        /// <summary>
        /// Creates a new instance of <see cref = "Enter"/>
        /// </summary>
        /// <param name = "value">The value to enter</param>
        public Enter(string value) : base(t => new EnterValue(value, t))
        {
            Value = value;
        }

        /// <summary>
        /// Creates an action which input the given string
        /// </summary>
        /// <param name = "value"></param>
        /// <returns></returns>
        public static Enter TheValue(string value)
        {
            return new Enter(value);
        }

        /// <summary>
        /// Creates an action which clear the target value and enter a new value 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static EnterNewValueBuilder TheNewValue(string value)
        {
            return new EnterNewValueBuilder(value);
        }
    }
}