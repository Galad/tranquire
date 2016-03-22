using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    public static class ActorExtensions
    {
        public static Selenium.BrowseTheWeb BrowseTheWeb(this IActor actor)
        {
            return Selenium.BrowseTheWeb.As(actor);
        }
    }
}
