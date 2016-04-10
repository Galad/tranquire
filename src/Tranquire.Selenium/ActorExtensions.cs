using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Provides extensions for the <see cref="IActor"/>
    /// </summary>
    public static class ActorExtensions
    {
        /// <summary>
        /// Gets the <see cref="Selenium.BrowseTheWeb"/> instance for the given actor
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static Selenium.BrowseTheWeb BrowseTheWeb(this IActor actor)
        {
            return Selenium.BrowseTheWeb.As(actor);
        }
    }
}
