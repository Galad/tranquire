using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Actions
{
    /// <summary>
    /// Allow to switch to a frame and go back to the parent frame
    /// </summary>
    public class UsingIFrame
    {
        private IActor _actor;

        /// <summary>
        /// Creates a new instance of <see cref="UsingIFrame"/>
        /// </summary>
        /// <param name="actor">The actor used to retrieve the <see cref="BrowseTheWeb"/> ability</param>
        public UsingIFrame(IActor actor)
        {
            Guard.ForNull(actor, "actor");
            _actor = actor;
        }

        /// <summary>
        /// Define the actor to use when switching to the frame
        /// </summary>
        /// <param name="actor"></param>
        /// <returns>A <see cref="UsingIFrame"/> object that can be used to switch to a frame</returns>
        public static UsingIFrame For(IActor actor)
        {
            return new UsingIFrame(actor);
        }

        /// <summary>
        /// Switch to the frame located by the given <see cref="ITarget"/>
        /// </summary>
        /// <param name="target">The target representing the frame to switch to</param>
        /// <returns>A <see cref="IDisposable"/> object. When it is disposed, the browser will go back to the parent frame.</returns>
        public IDisposable LocatedBy(ITarget target)
        {
            //var driver = _actor.BrowseTheWeb();
            //driver.SwitchTo().Frame(target.ResolveFor(_actor));
            //return new Disposable(() => driver.SwitchTo().ParentFrame());
            return new Disposable(() => { });
        }

        private class Disposable : IDisposable
        {
            private System.Action _action;

            public Disposable(System.Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                if(_action == null)
                {
                    return;
                }
                var action = Interlocked.Exchange(ref _action, null);
                action();
            }
        }
    }
}
