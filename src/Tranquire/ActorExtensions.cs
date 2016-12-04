using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Extension methods for the <see cref="Actor"/> class
    /// </summary>
    public static class ActorExtensions
    {
        /// <summary>
        /// Add the ability to highligh a target in the web browser on each action
        /// </summary>
        /// <param name="actor">The actor</param>
        /// <param name="observer">The observer used to report the actions</param>
        /// <returns>An new actor</returns>
        public static Actor WithReporting(this Actor actor, IObserver<string> observer)
        {
            Guard.ForNull(actor, nameof(actor));
            Guard.ForNull(observer, nameof(observer));
            return new Actor(actor.Name, actor.Abilities, a => new ReportingActor(observer, actor.InnerActorBuilder(a)));
        }
    }
}
