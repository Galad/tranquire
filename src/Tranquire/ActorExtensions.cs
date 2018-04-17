using System;
using Tranquire.Reporting;

namespace Tranquire
{
    /// <summary>
    /// Extension methods for the <see cref="Actor"/> class
    /// </summary>
    public static class ActorExtensions
    {
        /// <summary>
        /// Add the ability to report the actor actions
        /// </summary>
        /// <param name="actor">The actor</param>
        /// <param name="observer">The observer used to report the actions</param>
        /// <returns>An new actor</returns>
        public static Actor WithReporting(this Actor actor, IObserver<ActionNotification> observer)
        {
            Guard.ForNull(actor, nameof(actor));
            Guard.ForNull(observer, nameof(observer));
            return new Actor(actor.Name, actor.Abilities, a => new ReportingActor(observer, actor.InnerActorBuilder(a)));
        }

        /// <summary>
        /// Add the ability to report the actor actions
        /// </summary>
        /// <param name="actor">The actor</param>
        /// <param name="observer">The observer used to report the actions</param>
        /// <param name="canNotify">A <see cref="ICanNotify"/> instance that is used to filter actions that should not send a notification</param>
        /// <returns>An new actor</returns>
        public static Actor WithReporting(this Actor actor, IObserver<ActionNotification> observer, ICanNotify canNotify)
        {
            Guard.ForNull(actor, nameof(actor));
            Guard.ForNull(observer, nameof(observer));
            Guard.ForNull(canNotify, nameof(canNotify));
            return new Actor(actor.Name, actor.Abilities, a => new ReportingActor(observer, actor.InnerActorBuilder(a), new DefaultMeasureDuration(), canNotify));
        }

        /// <summary>
        /// Add the ability to report the actor actions as text
        /// </summary>
        /// <param name="actor">The actor</param>
        /// <param name="observer">The observer used to report the actions</param>
        /// <returns>An new actor</returns>
        public static Actor WithReporting(this Actor actor, IObserver<string> observer)
        {
            Guard.ForNull(actor, nameof(actor));
            Guard.ForNull(observer, nameof(observer));
            return new Actor(
                actor.Name,
                actor.Abilities,
                a => new ReportingActor(
                    new RenderedReportingObserver(
                        observer,
                        RenderedReportingObserver.DefaultRenderer),
                    actor.InnerActorBuilder(a)
                    )
                );
        }

        /// <summary>
        /// Add the ability to report the actor actions as text
        /// </summary>
        /// <param name="actor">The actor</param>
        /// <param name="observer">The observer used to report the actions</param>
        /// <param name="canNotify">A <see cref="ICanNotify"/> instance that is used to filter actions that should not send a notification</param>
        /// <returns>An new actor</returns>
        public static Actor WithReporting(this Actor actor, IObserver<string> observer, ICanNotify canNotify)
        {
            Guard.ForNull(actor, nameof(actor));
            Guard.ForNull(observer, nameof(observer));
            Guard.ForNull(canNotify, nameof(canNotify));
            return new Actor(
                actor.Name,
                actor.Abilities,
                a => new ReportingActor(
                    new RenderedReportingObserver(
                        observer,
                        RenderedReportingObserver.DefaultRenderer),
                    actor.InnerActorBuilder(a),
                    new DefaultMeasureDuration(),
                    canNotify
                    )
                );
        }
    }
}
