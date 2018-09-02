using Tranquire.Selenium.Questions;

namespace Tranquire.Selenium.Actions.Waiters
{
    /// <summary>
    /// Builer for wait actions on a target
    /// </summary>
    public sealed class WaitUntilTargetBuilder
    {
        private readonly ITarget _target;

        /// <summary>
        /// Creates a new instance of <see cref="WaitUntilTargetBuilder"/>
        /// </summary>
        /// <param name="target"></param>
        public WaitUntilTargetBuilder(ITarget target)
        {
            this._target = target ?? throw new System.ArgumentNullException(nameof(target));
        }

        /// <summary>
        /// Wait until the target is visible
        /// </summary>
        public WaitUntilQuestionIsAnswered<bool> IsVisible => new WaitUntilQuestionIsAnswered<bool>(Visibility.Of(_target), isVisible => isVisible);

        /// <summary>
        /// Wait until the target is not visible
        /// </summary>
        public WaitUntilQuestionIsAnswered<bool> IsNotVisible => new WaitUntilQuestionIsAnswered<bool>(Visibility.Of(_target), isVisible => !isVisible);

        /// <summary>
        /// Wait until the given target is present in the DOM
        /// </summary>
        public WaitUntilQuestionIsAnswered<bool> IsPresent => new WaitUntilQuestionIsAnswered<bool>(Presence.Of(_target), isPresent => isPresent);
    }
}