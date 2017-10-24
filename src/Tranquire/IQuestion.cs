using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent a question
    /// </summary>
    /// <typeparam name="TAnswer">The answer's type</typeparam>
    public interface IQuestion<out TAnswer> : INamed
    {
        /// <summary>
        /// Answers the question
        /// </summary>
        /// <param name="actor">The actor used to answer the question</param>
        /// <returns>The answer to the question</returns>
        TAnswer AnsweredBy(IActor actor);
    }

    /// <summary>
    /// Represent a question from an ability
    /// </summary>
    /// <typeparam name="TAnswer">The answer's type</typeparam>
    /// <typeparam name="TAbility">The type of the ability required to answer this question</typeparam>
    [Obsolete("Prefer using IQuestion<TAnswer> when exposing a question, or inheriting the abstract class Question<TAnswer, TAbility> when implementing one.", false)]
    public interface IQuestion<out TAnswer, in TAbility> : INamed, IQuestion<TAnswer>
    {
        /// <summary>
        /// Answers the question
        /// </summary>
        /// <param name="actor">The actor used to answer the question</param>
        /// <param name="ability">The ability required by the question</param>
        /// <returns>The answer to the question</returns>
        TAnswer AnsweredBy(IActor actor, TAbility ability);
    }
}
