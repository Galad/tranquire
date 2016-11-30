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
    public class UsingIFrame : Action<BrowseTheWeb, IDisposable>
    {
        private ITarget _target;
        /// <summary>
        /// Gets the action's name
        /// </summary>
        public override string Name => $"Use iframe {_target.Name}";

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
        protected override IDisposable ExecuteWhen(IActor actor, BrowseTheWeb ability)
        {
            ability.SwitchTo().Frame(_target.ResolveFor(ability));
            return new Disposable(() => actor.Execute(new SwitchToParentIFrame()));
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

        private class SwitchToParentIFrame : ActionUnit<BrowseTheWeb>
        {            
            protected override void ExecuteWhen(IActor actor, BrowseTheWeb ability)
            {
                ability.SwitchTo().ParentFrame();
            }

            /// <summary>
            /// Gets the action's name
            /// </summary>
            public override string Name => $"Switch to parent iframe";
        }
    }
}
