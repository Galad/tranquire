namespace Tranquire.Selenium.Actions.Window
{
    /// <summary>
    /// Set the window in full screen
    /// </summary>
    public class FullScreenWindowAction : ActionBaseUnit<WebBrowser>
    {
        /// <inheritdoc />
        public override string Name => "Full screen Window";

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected override void ExecuteWhen(IActor actor, WebBrowser ability)
        {
            ability.Driver.Manage().Window.FullScreen();
        }
    }
}
