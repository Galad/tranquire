namespace Tranquire.Selenium.Actions.Window
{
    /// <summary>
    /// Minimizes the window
    /// </summary>
    public class MinimizeWindowAction : ActionBaseUnit<WebBrowser>
    {
        /// <inheritdoc />
        public override string Name => "Minimize Window";

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected override void ExecuteWhen(IActor actor, WebBrowser ability)
        {
            ability.Driver.Manage().Window.Minimize();
        }
    }
}
