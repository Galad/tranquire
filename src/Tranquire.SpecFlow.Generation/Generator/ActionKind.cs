namespace Tranquire.SpecFlow.Generation.Generator
{
    /// <summary>
    /// An action kind
    /// </summary>
    public enum ActionKind
    {
        /// <summary>
        /// An action (<see cref="Tranquire.IAction{TResult}"/> or <see cref="Tranquire.IAction{TAbility, TResult}"/>
        /// </summary>
        Action,
        /// <summary>
        /// A question (<see cref="Tranquire.IQuestion{TAnswer}"/> or <see cref="Tranquire.IQuestion{TAnswer, TAbility}"/>
        /// </summary>
        Question
    }
}
