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
    public class UsingIFrame : Action<WebBrowser, IDisposable>
    {
        private readonly ITarget _target;
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
        protected override IDisposable ExecuteWhen(IActor actor, WebBrowser ability)
        {
            ability.SwitchTo().Frame(_target.ResolveFor(ability));
            return new Disposable(() => actor.Execute(new SwitchToParentIFrame()));
        }
        
        private sealed class Disposable : IDisposable
        {
            private System.Action _action;

            public Disposable(System.Action action)
            {
                _action = action;
            }

            #region IDisposable Support
            private bool disposedValue = false; // To detect redundant calls

            void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        Interlocked.Exchange(ref _action, null)?.Invoke();
                    }
                    disposedValue = true;
                }
            }
                        
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);                
            }
            #endregion
        }

        private class SwitchToParentIFrame : ActionUnit<WebBrowser>
        {            
            protected override void ExecuteWhen(IActor actor, WebBrowser ability)
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
