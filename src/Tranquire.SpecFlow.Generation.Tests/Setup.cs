using BoDi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Tranquire.SpecFlow.Generation.Tests
{
    [Binding]
    public sealed class Setup
    {
        private readonly IObjectContainer objectContainer;

        public Setup(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            objectContainer.RegisterInstanceAs<IActorFacade>(new Actor("test"));
        }
    }
}
