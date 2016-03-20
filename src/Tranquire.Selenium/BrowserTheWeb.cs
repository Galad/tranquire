using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium
{
    public class BrowseTheWeb : IAbility<BrowseTheWeb>
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
            return actor.AbilityTo<BrowseTheWeb>();
        }

        public BrowseTheWeb AsActor(IActor actor)
        {
            return new BrowseTheWeb(Driver, actor);
        }

        private class NoActor : IActor
        {
            public NoActor()
            {
            }

            public T AbilityTo<T>() where T : IAbility<T>
            {
                throw new NotImplementedException();
            }

            public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
            {
                throw new NotImplementedException();
            }

            public IActor AttemptsTo(Tranquire.IAction performable)
            {
                throw new NotImplementedException();
            }

            public IActor Can<T>(T doSomething) where T : IAbility<T>
            {
                throw new NotImplementedException();
            }

            public IActor WasAbleTo(ITask performable)
            {
                throw new NotImplementedException();
            }
        }
    }
}
