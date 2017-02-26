using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Tranquire.Tests
{
    public class ActionTests : ActionTestsBase<ActionTests.ActionExecuteGiven, ActionTests.ActionExecuteWhen, ActionTests.ActionExecuteWhenAndGivenNotOverridden>
    {
        public class ActionExecuteWhen : Action<object>, IWithActor
        {
            public IActor Actor { get; private set; }
            private readonly IAction<object> _action;
            public override string Name { get; }

            public ActionExecuteWhen(IAction<object> action, string name)
            {
                _action = action;
                Name = name;
            }

            protected override object ExecuteWhen(IActor actor)
            {
                Actor = actor;
                return actor.Execute(_action);
            }

            protected override object ExecuteGiven(IActor actor)
            {
                return new object();
            }
        }

        public class ActionExecuteGiven : Action<object>, IWithActor
        {
            public IActor Actor { get; private set; }
            private readonly IAction<object> _action;
            public override string Name => "";

            public ActionExecuteGiven(IAction<object> action)
            {
                if (action == null) throw new ArgumentNullException(nameof(action));
                _action = action;
            }

            protected override object ExecuteWhen(IActor actor)
            {
                return new object();
            }

            protected override object ExecuteGiven(IActor actor)
            {
                Actor = actor;
                return actor.Execute(_action);
            }
        }

        public class ActionExecuteWhenAndGivenNotOverridden : Action<object>
        {
            private readonly IAction<object> _action;
            public override string Name => "";

            public ActionExecuteWhenAndGivenNotOverridden(IAction<object> action)
            {
                _action = action;
            }

            protected override object ExecuteWhen(IActor actor)
            {
                return actor.Execute(_action);
            }
        }
    }
}

