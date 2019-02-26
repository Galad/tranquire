using System;
using System.Collections.Generic;

namespace Tranquire
{
    /// <summary>
    /// A base class for implementing a question
    /// </summary>
    /// <typeparam name="TAnswer">The answer type</typeparam>
    /// <typeparam name="TAbility">The ability type</typeparam>
#pragma warning disable CS0618 // Type or member is obsolete
    public abstract class QuestionBase<TAbility, TAnswer> : IQuestion<TAbility, TAnswer>
#pragma warning restore CS0618 // Type or member is obsolete
    {
        /// <summary>
        /// Gets the question name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Returns the question answer. 
        /// </summary>
        /// <param name="actor">The actor used for answering this question</param>
        /// <param name="ability">The ability</param>
        /// <returns>The question answer</returns>
        protected abstract TAnswer Answer(IActor actor, TAbility ability);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TAnswer AnsweredBy(IActor actor, TAbility ability)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            if (EqualityComparer<TAbility>.Default.Equals(ability, default))
            {
                throw new ArgumentNullException(nameof(ability));
            }

            return Answer(actor, ability);
        }

        public TAnswer AnsweredBy(IActor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }
#pragma warning disable CS0618 // Type or member is obsolete
            return actor.AsksForWithAbility(this);
#pragma warning restore CS0618 // Type or member is obsolete
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Returns the question name
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;
    }

    /// <summary>
    /// A base class for implementing a question
    /// </summary>
    /// <typeparam name="TAnswer">The answer type</typeparam>
    public abstract class QuestionBase<TAnswer> : IQuestion<TAnswer>
    {
        /// <summary>
        /// Gets the question name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Returns the question answer. 
        /// </summary>
        /// <param name="actor">The actor used for answering this question</param>
        protected abstract TAnswer Answer(IActor actor);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TAnswer AnsweredBy(IActor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            return Answer(actor);
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Returns the question name
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;
    }
}
