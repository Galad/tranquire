using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium
{
    /// <summary>
    /// Represent the ability to use a web browser with selenium
    /// </summary>
    public partial class BrowseTheWeb : IAbility<BrowseTheWeb>
    {
        private readonly IActor _actor;

        public IWebDriver Driver { get; }

        private BrowseTheWeb(IWebDriver driver) : this(driver, new NoActor())
        {
        }

        public BrowseTheWeb(IWebDriver driver, IActor actor)
        {
            _actor = actor;
            Driver = driver;
        }

        public static BrowseTheWeb With(IWebDriver driver)
        {
            return new BrowseTheWeb(driver);
        }

        public static BrowseTheWeb As(IActor actor)
        {
            return actor.AbilityTo<BrowseTheWeb>().AsActor(actor);
        }

        public BrowseTheWeb AsActor(IActor actor)
        {
            return new BrowseTheWeb(Driver, actor);
        }

        private class NoActor : IActor
        {
            public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
            {
                throw new NotImplementedException();
            }

            public IActor AttemptsTo(IWhenCommand performable)
            {
                throw new NotImplementedException();
            }

            public IActor WasAbleTo(IGivenCommand performable)
            {
                throw new NotImplementedException();
            }

            TAbility IActor.AbilityTo<TAbility>()
            {
                throw new NotImplementedException();
            }

            IActor IActor.Can<T>(T doSomething)
            {
                throw new NotImplementedException();
            }
        }
    }
}
