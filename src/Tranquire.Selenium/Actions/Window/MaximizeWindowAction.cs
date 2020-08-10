using System.Drawing;

namespace Tranquire.Selenium.Actions.Window
{
    /// <summary>
    /// Maximizes the window
    /// </summary>
    public class MaximizeWindowAction : ActionBaseUnit<WebBrowser>
    {
        /// <inheritdoc />
        public override string Name => "Maximize Window";

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected override void ExecuteWhen(IActor actor, WebBrowser ability)
        {
            ability.Driver.Manage().Window.Maximize();
        }
    }
}
