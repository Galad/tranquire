using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Tranquire.Selenium.Actions.Window;
using Tranquire.Selenium.Questions;

namespace Tranquire.Selenium
{
    /// <summary>
    /// Contains actions about the web browser window
    /// </summary>
    public static class WebBrowserWindow
    {
        /// <summary>
        /// Create an action that resizes the window
        /// </summary>
        /// <param name="size">The new size</param>
        /// <returns></returns>
        public static IAction<Unit> Resize(Size size) => new ResizeWindowAction(size);

        /// <summary>
        /// Create an action that maximizes the windows
        /// </summary>
        /// <returns></returns>
        public static IAction<Unit> Maximize() => new MaximizeWindowAction();

        /// <summary>
        /// Create an action that minimizes the windows
        /// </summary>
        /// <returns></returns>
        public static IAction<Unit> Minimize() => new MinimizeWindowAction();

        /// <summary>
        /// Create an action that maximizes the windows
        /// </summary>
        /// <returns></returns>
        public static IAction<Unit> FullScreen() => new FullScreenWindowAction();
    }
}
