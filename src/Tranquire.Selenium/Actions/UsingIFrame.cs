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
    public class UsingIFrame : Action<BrowseTheWeb>
    {
        private ITarget _target;

        /// <summary>
        /// Creates a new instance of <see cref="UsingIFrame"/>
        /// </summary>
        /// <param name="target">The target representing the frame to switch to</param>
        public UsingIFrame(ITarget target)
        {
            Guard.ForNull(target, "actor");
            _target = target;
        }

        /// <summary>
        /// Define the actor to use when switching to the frame
        /// </summary>
        /// <param name="actor"></param>
        /// <returns>A <see cref="UsingIFrame"/> object that can be used to switch to a frame</returns>
        public IDisposable ExecuteFor(IActor actor)
        {
            actor.Execute(this);            
            return new Disposable(() => actor.Execute(new SwitchToParentIFrame()));
        }

        /// <summary>
        /// Switch to the frame located by the given <see cref="ITarget"/>
        /// </summary>
        /// <param name="target">The target representing the frame to switch to</param>
        /// <returns>A <see cref="IDisposable"/> object. When it is disposed, the browser will go back to the parent frame.</returns>
        public static UsingIFrame LocatedBy(ITarget target)
        {
            return new UsingIFrame(target);                
        }

        /// <summary>
        /// Switch to the IFrame
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected override void ExecuteWhen(IActor actor, BrowseTheWeb ability)
        {
            ability.SwitchTo().Frame(_target.ResolveFor(ability));
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
                var action = Interlocked.Exchange(ref _action, null);
                if (action == null)
                {
                    return;
                }                
                action();
            }
        }

        private class SwitchToParentIFrame : Action<BrowseTheWeb>
        {            
            protected override void ExecuteWhen(IActor actor, BrowseTheWeb ability)
            {
                ability.SwitchTo().ParentFrame();
            }
        }
    }
}
