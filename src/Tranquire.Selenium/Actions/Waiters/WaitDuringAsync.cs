using System;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Actions.Waiters
{
    /// <summary>
    /// Wait during the specified amount of time
    /// </summary>
    public sealed class WaitDuringAsync : ActionBase<Task>
    {
        private readonly TimeSpan _timeToWait;

        /// <summary>
        /// Creates a new instance of <see cref="WaitDuringAsync"/>
        /// </summary>
        /// <param name="timeToWait">The time to wait</param>
        public WaitDuringAsync(TimeSpan timeToWait)
        {
            _timeToWait = timeToWait;
        }

        /// <inheritdoc />
        public override string Name => $"Wait during ${_timeToWait}";

        /// <inheritdoc />
        protected override async Task ExecuteWhen(IActor actor)
        {
            await Task.Delay(_timeToWait).ConfigureAwait(false);
        }
    }
}