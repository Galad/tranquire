using System;
using Tranquire.Selenium.Actions.Waiters;

namespace Tranquire.Selenium.Actions;

/// <summary>
/// An action used to wait for a condition
/// </summary>
public static class Wait
{
    /// <summary>
    /// Wait until a question is answered
    /// </summary>
    /// <typeparam name="TAnswer"></typeparam>
    /// <param name="question">The question to answer</param>
    /// <param name="isAnswered">A predicate returning wether the answer is satisfying</param>
    /// <returns>An action waiting until the question is answered</returns>
    public static WaitUntilQuestionIsAnswered<TAnswer> UntilQuestionIsAnswered<TAnswer>(IQuestion<TAnswer> question, Predicate<TAnswer> isAnswered)
    {
        return new WaitUntilQuestionIsAnswered<TAnswer>(question, isAnswered);
    }

    /// <summary>
    /// Wait until the given target is visible
    /// </summary>
    /// <param name="target">The target to wait for</param>
    public static WaitUntilTargetBuilder Until(ITarget target)
    {
        return new WaitUntilTargetBuilder(target);
    }


    /// <summary>
    /// Wait during the specified amount of time
    /// </summary>
    /// <param name="timeToWait">The time to wait</param>
    public static WaitDuring During(TimeSpan timeToWait) => new(timeToWait);
}
