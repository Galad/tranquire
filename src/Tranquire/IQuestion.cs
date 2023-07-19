using System;

namespace Tranquire;

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
#pragma warning disable CS0618, S1133 // Type or member is obsolete
[Obsolete("Prefer using IQuestion<TAnswer> when exposing a question, or inheriting the abstract class Question<TAbility, TAnswer> when implementing one.", false)]
#pragma warning restore CS0618, S1133 // Type or member is obsolete
public interface IQuestion<in TAbility, out TAnswer> : IQuestion<TAnswer>
{
    /// <summary>
    /// Answers the question
    /// </summary>
    /// <param name="actor">The actor used to answer the question</param>
    /// <param name="ability">The ability required by the question</param>
    /// <returns>The answer to the question</returns>
    TAnswer AnsweredBy(IActor actor, TAbility ability);
}
