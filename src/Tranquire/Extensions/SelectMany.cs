using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Tranquire.Extensions
{

    internal class SelectMany<TActionSource, TSource, TActionResult, TResult> : ISelectMany<TResult>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISelectMany<TResult> Create<TActionSource, TSource, TActionResult, TResult>(
            TActionSource source,
            Func<IActor, TActionSource, TSource> applySource,
            Func<TSource, TActionResult> selector,
            Func<IActor, TActionResult, TResult> applyResult
            )
            where TActionSource : class, INamed
            where TActionResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new SelectMany<TActionSource, TSource, TActionResult, TResult>(source, applySource, selector, applyResult);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISelectMany<Task<TResult>> Create<TActionSource, TSource, TActionResult, TResult>(
            TActionSource source,
            Func<IActor, TActionSource, Task<TSource>> applySource,
            Func<TSource, TActionResult> selector,
            Func<IActor, Task<TActionResult>, Task<TResult>> applyResult
            )
            where TActionSource : class, INamed
            where TActionResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            Func<Task<TSource>, Task<TActionResult>> selectorAsync = async sourceTask =>
            {
                var s = await sourceTask;
                return selector(s);
            };
            return Create(source, applySource, selectorAsync, applyResult);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISelectMany<Task<TResult>> Create<TActionSource, TActionResult, TResult>(
            TActionSource source,
            Func<IActor, TActionSource, Task> applySource,
            Func<TActionResult> selector,
            Func<IActor, Task<TActionResult>, Task<TResult>> applyResult
            )
            where TActionSource : class, INamed
            where TActionResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            Func<Task, Task<TActionResult>> selectorAsync = async sourceTask =>
            {
                await sourceTask;
                return selector();
            };
            return Create(source, applySource, selectorAsync, applyResult);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<IActor, IQuestion<T>, T> AsksFor<T>() => (actor, q) => actor.AsksFor(q);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<IActor, Task<IQuestion<T>>, Task<T>> AsksForAsync<T>() => async (actor, questionTask) =>
        {
            var question = await questionTask;
            return actor.AsksFor(question);
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<IActor, IAction<T>, T> Execute<T>() => (actor, q) => actor.Execute(q);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<IActor, Task<IAction<T>>, Task<T>> ExecuteAsync<T>() => async (actor, actionTask) =>
        {
            var action = await actionTask;
            return actor.Execute(action);
        };
    }

    internal static class SelectManyExtensions
    {
        public static IAction<TResult> ToAction<TResult>(this ISelectMany<TResult> selectMany)
        {
            return Actions.Create(
                selectMany.Name,
                actor => selectMany.Apply(actor));
        }

        public static IAction<Task<TResult>> ToAction<TResult>(this ISelectMany<Task<Task<TResult>>> selectMany)
        {
            return Actions.Create(
                selectMany.Name,
                async actor =>
                {
                    var task = await selectMany.Apply(actor);
                    return await task;
                });
        }

        public static IAction<Task> ToAction(this ISelectMany<Task<Task>> selectMany)
        {
            return Actions.Create(
                selectMany.Name,
                async actor =>
                {
                    var task = await selectMany.Apply(actor);
                    await task;
                });
        }

        public static IQuestion<TResult> ToQuestion<TResult>(this ISelectMany<TResult> selectMany)
        {
            return Questions.Create(
                selectMany.Name,
                actor => selectMany.Apply(actor));
        }

        public static IQuestion<Task<TResult>> ToQuestion<TResult>(this ISelectMany<Task<Task<TResult>>> selectMany)
        {
            return Questions.Create(
                selectMany.Name,
                async actor =>
                {
                    var task = await selectMany.Apply(actor);
                    return await task;
                });
        }
    }
}
