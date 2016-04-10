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

        /// <summary>
        /// Gets the Selenium <see cref="IWebDriver"/>
        /// </summary>
        public IWebDriver Driver { get; }

        private BrowseTheWeb(IWebDriver driver) : this(driver, new NoActor())
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="BrowseTheWeb"/>
        /// </summary>
        /// <param name="driver">The driver to use to browse the web</param>
        /// <param name="actor">The actor</param>
        public BrowseTheWeb(IWebDriver driver, IActor actor)
        {
            _actor = actor;
            Driver = driver;
        }

        /// <summary>
        /// Returns a new <see cref="BrowseTheWeb"/> instance
        /// </summary>
        /// <param name="driver">The driver to use</param>
        /// <returns></returns>
        public static BrowseTheWeb With(IWebDriver driver)
        {
            return new BrowseTheWeb(driver);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static BrowseTheWeb As(IActor actor)
        {
            return actor.AbilityTo<BrowseTheWeb>().AsActor(actor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
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

            public IActor Execute(IAction action)
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
