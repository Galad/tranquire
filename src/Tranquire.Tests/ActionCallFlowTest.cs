using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire;
using Xunit;
using Xunit.Abstractions;

namespace ToDoList.Specifications
{
    public class Action1 : Tranquire.Action
    {
        private readonly Action<string> _notify;

        public Action1(Action<string> notify)
        {
            _notify = notify;
        }

        protected override void ExecuteWhen(IActionCommand command, IActor actor)
        {
            _notify(ActionCallFlowTest.ExecuteWhen);
            command.Execute(new Action2(_notify));
        }
    }

    public class Action2 : Tranquire.Action
    {
        private readonly Action<string> _notify;

        public Action2(Action<string> notify)
        {
            _notify = notify;
        }

        protected override void ExecuteGiven(IActionCommand command, IActor actor)
        {
            _notify(ActionCallFlowTest.ExecuteGiven);
            command.Execute(new Action3(_notify));
        }

        protected override void ExecuteWhen(IActionCommand command, IActor actor)
        {
            _notify(ActionCallFlowTest.ExecuteWhen);
            command.Execute(new Action3(_notify));
        }
    }


    public class Action3 : Tranquire.Action
    {
        private readonly Action<string> _notify;

        public Action3(Action<string> notify)
        {
            _notify = notify;
        }

        protected override void ExecuteWhen(IActionCommand command, IActor actor)
        {
            _notify(ActionCallFlowTest.ExecuteWhen);
        }
    }

    public class ActionCallFlowTest
    {
        public const string ExecuteWhen = "ExecuteWhen";
        public const string ExecuteGiven = "ExecuteWhen";

        [Fact]
        public void When()
        {
            //arrange
            var actor = new Actor("john");
            var actual = new List<string>();
            Action<string> notify = (s) => actual.Add(s);
            //act
            actor.AttemptsTo(new Action1(notify));
            //assert
            var expected = new[]
            {
                ExecuteWhen,
                ExecuteWhen,
                ExecuteWhen,
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Given()
        {
            //arrange
            var actor = new Actor("john");
            var actual = new List<string>();
            Action<string> notify = (s) => actual.Add(s);
            //act
            actor.WasAbleTo(new Action1(notify));
            //assert
            var expected = new[]
            {
                ExecuteWhen,
                ExecuteGiven,
                ExecuteWhen,
            };
            Assert.Equal(expected, actual);
        }
    }
}
