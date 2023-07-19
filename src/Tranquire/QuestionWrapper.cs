namespace Tranquire;

/// <summary>
/// A wrapper of a <see cref="IQuestion{TAnswer}"/> that represents it as a class
/// </summary>
/// <typeparam name="T">The question's answer type</typeparam>
public sealed class QuestionClassWrapper<T> : IQuestion<T>
{
    private readonly IQuestion<T> _question;

    /// <summary>
    /// Creates a new instance of <see cref=" QuestionClassWrapper{T}"/>
    /// </summary>
    /// <param name="question"></param>
    public QuestionClassWrapper(IQuestion<T> question)
    {
        _question = question ?? throw new System.ArgumentNullException(nameof(question));
    }

    /// <inheritdoc />
    public string Name => _question.Name;

    /// <inheritdoc />
    public T AnsweredBy(IActor actor) => _question.AnsweredBy(actor);
}
