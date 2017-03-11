using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Actions
{
    /// <summary>
    /// Navigate to an url
    /// </summary>
    public sealed class Navigate : ActionUnit<WebBrowser>
    {
        /// <summary>
        /// Gets the Uri to navigate to 
        /// </summary>
        public Uri Uri { get; }

        /// <summary>
        /// Creates a new instance of <see cref="Navigate"/>
        /// </summary>
        /// <param name="uri">The uri to navigate to</param>
        public Navigate(Uri uri)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
        }

        /// <summary>
        /// Creates an action that navigates to an Uri
        /// </summary>
        /// <param name="uri">The Uri to navigate to</param>
        /// <returns>A new action that navigate to the Uri</returns>
        public static IAction<WebBrowser, WebBrowser, Unit> To(Uri uri)
        {
            return new Navigate(uri);
        }

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected override void ExecuteWhen(IActor actor, WebBrowser ability)
        {
            if (ability == null) throw new ArgumentNullException(nameof(ability));
            ability.Navigate().GoToUrl(Uri);
        }

        /// <summary>
        /// Gets the action name
        /// </summary>
        public override string Name => "Navigate to " + Uri.ToString();
    }
}
