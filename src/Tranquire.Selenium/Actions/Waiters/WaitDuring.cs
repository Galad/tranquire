using System;
using System.Threading;

namespace Tranquire.Selenium.Actions.Waiters
{
    /// <summary>
    /// Wait during the specified amount of time
    /// </summary>
    public sealed class WaitDuring : ActionBaseUnit
    {
        private readonly TimeSpan _timeToWait;

        /// <summary>
        /// Creates a new instance of <see cref="WaitDuring"/>
        /// </summary>
        /// <param name="timeToWait">The time to wait</param>
        public WaitDuring(TimeSpan timeToWait)
        {
            _timeToWait = timeToWait;
        }

        /// <inheritdoc />
        public override string Name => $"Wait during ${_timeToWait}";

        /// <inheritdoc />
        protected override void ExecuteWhen(IActor actor)
        {
            Thread.Sleep((int)_timeToWait.TotalMilliseconds);
        }

        /// <summary>
        /// Returns an action that waits asynchronously
        /// </summary>
        public WaitDuringAsync Async => new WaitDuringAsync(_timeToWait);
    }
}
