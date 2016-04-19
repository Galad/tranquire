namespace Tranquire.Selenium.Actions.Enters
{
    /// <summary>
    /// clears the target value and enter a new value 
    /// </summary>
    public class EnterNewValue : TargetableAction<Task>
    {
        /// <summary>
        /// Creates a new instance of <see cref="EnterNewValue"/>
        /// </summary>
        /// <param name="value">The new value to enter</param>
        public EnterNewValue(string value) : base(t => new Task().And(Clear.TheValueOf(t)).And(Enter.TheValue(value).Into(t)))
        {
        }
    }
}