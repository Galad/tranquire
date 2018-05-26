using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquire
{
    public sealed class ThenAction<T> : Action<T>
    {
        private readonly IQuestion<T> _question;
        private readonly System.Action<T> _verifyAction;

        public ThenAction(IQuestion<T> question, System.Action<T> verifyAction)
        {
            _question = question ?? throw new ArgumentNullException(nameof(question));
            _verifyAction = verifyAction ?? throw new ArgumentNullException(nameof(verifyAction));
        }

        public override string Name => "Then";

        protected override T ExecuteWhen(IActor actor)
        {
            var answer = actor.AsksFor(_question);
            _verifyAction(answer);
            return answer;
        }
    }
}
