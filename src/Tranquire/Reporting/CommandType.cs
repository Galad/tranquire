namespace Tranquire.Reporting;

/// <summary>
/// Represent the type of a command executed by an actor
/// </summary>
public enum CommandType
{
    /// <summary>
    /// Represent an action (see <see cref="IAction{TResult}"/>)
    /// </summary>
    Action,
    /// <summary>
    /// Represent a question (see <see cref="IQuestion{TAnswer}"/>
    /// </summary>
    Question
}
