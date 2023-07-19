using System;
using Tranquire.Reporting;

namespace Tranquire;

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
    /// <param name="measureDuration">A <see cref="IMeasureDuration"/> instance that provides duration and current date</param>
    /// <returns>An new actor</returns>
    public static Actor WithReporting(
        this Actor actor,
        IObserver<ActionNotification> observer,
        IMeasureDuration measureDuration)
    {
        if (actor == null)
        {
            throw new ArgumentNullException(nameof(actor));
        }
        if (observer == null)
        {
            throw new ArgumentNullException(nameof(observer));
        }
        if (measureDuration == null)
        {
            throw new ArgumentNullException(nameof(measureDuration));
        }

        return new Actor(actor.Name, actor.Abilities, a => new ReportingActor(observer, actor.InnerActorBuilder(a), measureDuration));
    }

    /// <summary>
    /// Add the ability to report the actor actions
    /// </summary>
    /// <param name="actor">The actor</param>
    /// <param name="observer">The observer used to report the actions</param>        
    /// <returns>An new actor</returns>
    public static Actor WithReporting(
        this Actor actor,
        IObserver<ActionNotification> observer)
    {
        return actor.WithReporting(observer, new DefaultMeasureDuration());
    }

    /// <summary>
    /// Add the ability to report the actor actions
    /// </summary>
    /// <param name="actor">The actor</param>
    /// <param name="observer">The observer used to report the actions</param>
    /// <param name="canNotify">A <see cref="ICanNotify"/> instance that is used to filter actions that should not send a notification</param>
    /// <param name="measureDuration">A <see cref="IMeasureDuration"/> instance that provides duration and current date</param>
    /// <returns>An new actor</returns>
    public static Actor WithReporting(this Actor actor, IObserver<ActionNotification> observer, ICanNotify canNotify, IMeasureDuration measureDuration)
    {
        if (actor == null)
        {
            throw new ArgumentNullException(nameof(actor));
        }
        if (observer == null)
        {
            throw new ArgumentNullException(nameof(observer));
        }
        if (canNotify == null)
        {
            throw new ArgumentNullException(nameof(canNotify));
        }
        if (measureDuration == null)
        {
            throw new ArgumentNullException(nameof(measureDuration));
        }

        return new Actor(actor.Name, actor.Abilities, a => new ReportingActor(observer, actor.InnerActorBuilder(a), measureDuration, canNotify));
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
        return actor.WithReporting(observer, canNotify, new DefaultMeasureDuration());
    }

    /// <summary>
    /// Add the ability to report the actor actions as text
    /// </summary>
    /// <param name="actor">The actor</param>
    /// <param name="observer">The observer used to report the actions</param>
    /// <param name="measureDuration">A <see cref="IMeasureDuration"/> instance that provides duration and current date</param>
    /// <returns>An new actor</returns>
    public static Actor WithReporting(this Actor actor, IObserver<string> observer, IMeasureDuration measureDuration)
    {
        if (actor == null)
        {
            throw new ArgumentNullException(nameof(actor));
        }
        if (observer == null)
        {
            throw new ArgumentNullException(nameof(observer));
        }
        if (measureDuration == null)
        {
            throw new ArgumentNullException(nameof(measureDuration));
        }

        return new Actor(
            actor.Name,
            actor.Abilities,
            a => new ReportingActor(
                new RenderedReportingObserver(
                    observer,
                    RenderedReportingObserver.DefaultRenderer),
                actor.InnerActorBuilder(a),
                measureDuration
                )
            );
    }

    /// <summary>
    /// Add the ability to report the actor actions as text
    /// </summary>
    /// <param name="actor">The actor</param>
    /// <param name="observer">The observer used to report the actions</param>        
    /// <returns>An new actor</returns>
    public static Actor WithReporting(this Actor actor, IObserver<string> observer)
    {
        return actor.WithReporting(observer, new DefaultMeasureDuration());
    }

    /// <summary>
    /// Add the ability to report the actor actions as text
    /// </summary>
    /// <param name="actor">The actor</param>
    /// <param name="observer">The observer used to report the actions</param>
    /// <param name="canNotify">A <see cref="ICanNotify"/> instance that is used to filter actions that should not send a notification</param>
    /// <param name="measureDuration">A <see cref="IMeasureDuration"/> instance that provides duration and current date</param>
    /// <returns>An new actor</returns>
    public static Actor WithReporting(this Actor actor, IObserver<string> observer, ICanNotify canNotify, IMeasureDuration measureDuration)
    {
        if (actor == null)
        {
            throw new ArgumentNullException(nameof(actor));
        }
        if (observer == null)
        {
            throw new ArgumentNullException(nameof(observer));
        }
        if (canNotify == null)
        {
            throw new ArgumentNullException(nameof(canNotify));
        }
        if (measureDuration == null)
        {
            throw new ArgumentNullException(nameof(measureDuration));
        }

        return new Actor(
            actor.Name,
            actor.Abilities,
            a => new ReportingActor(
                new RenderedReportingObserver(
                    observer,
                    RenderedReportingObserver.DefaultRenderer),
                actor.InnerActorBuilder(a),
                measureDuration,
                canNotify
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
    public static Actor WithReporting(
        this Actor actor,
        IObserver<string> observer,
        ICanNotify canNotify)
    {
        return actor.WithReporting(observer, canNotify, new DefaultMeasureDuration());
    }
}
