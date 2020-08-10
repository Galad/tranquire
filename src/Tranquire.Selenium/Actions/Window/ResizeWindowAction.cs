using System.Drawing;

namespace Tranquire.Selenium.Actions.Window
{
    /// <summary>
    /// Contains actions for resizing the window
    /// </summary>
    public class ResizeWindowAction : ActionBaseUnit<WebBrowser>
    {
        private readonly Size _size;

        /// <inheritdoc />
        public override string Name => "Resize Window";

        /// <summary>
        /// Creates a new instance of <see cref="ResizeWindowAction" />
        /// </summary>
        /// <param name="size"></param>
        public ResizeWindowAction(Size size)
        {
            _size = size;
        }

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected override void ExecuteWhen(IActor actor, WebBrowser ability)
        {
            ability.Driver.Manage().Window.Size = _size;
        }
    }

}
