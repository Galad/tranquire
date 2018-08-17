using System;

namespace Tranquire.Extensions
{
    internal class SelectMany<TActionSource, TSource, TActionResult, TResult>
        where TActionSource : class, INamed
        where TActionResult : class
    {
        public SelectMany(
            TActionSource source, 
            Func<IActor, TActionSource, TSource> applySource, 
            Func<TSource, TActionResult> selector, 
            Func<IActor, TActionResult, TResult> applyResult)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            ApplySource = applySource ?? throw new ArgumentNullException(nameof(applySource));
            Selector = selector ?? throw new ArgumentNullException(nameof(selector));
            ApplyResult = applyResult ?? throw new ArgumentNullException(nameof(applyResult));
        }

        public string Name => "[SelectMany] " + Source.Name;

        public TActionSource Source { get; }
        public Func<IActor, TActionSource, TSource> ApplySource { get; }
        public Func<TSource, TActionResult> Selector { get; }
        public Func<IActor, TActionResult, TResult> ApplyResult { get; }

        public TResult Apply(IActor actor)
        {
            var sourceValue = ApplySource(actor, Source);
            var nextAction = Selector(sourceValue);
            var resultValue = ApplyResult(actor, nextAction);
            return resultValue;
        }
    }

    internal static class SelectMany
    {
        public static SelectMany<TActionSource, TSource, TActionResult, TResult> Create<TActionSource, TSource, TActionResult, TResult>(
            TActionSource source,
            Func<IActor, TActionSource, TSource> applySource,
            Func<TSource, TActionResult> selector,
            Func<IActor, TActionResult, TResult> applyResult
            )
            where TActionSource : class, INamed
            where TActionResult : class
        {
            return new SelectMany<TActionSource, TSource, TActionResult, TResult>(source, applySource, selector, applyResult);
        }

        public static Func<IActor, IQuestion<T>, T> AsksFor<T>() => (actor, q) => actor.AsksFor(q);
        public static Func<IActor, IAction<T>, T> Execute<T>() => (actor, q) => actor.Execute(q);
    }
}
