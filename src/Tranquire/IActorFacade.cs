namespace Tranquire
{
    /// <summary>
    /// Facade interface which is the entry point for executing actions or askin questions
    /// </summary>
    public interface IActorFacade : ICanHaveAbility<IActorFacade>, IActionExecutor, IAsksQuestions, IVerifies
    {
    }
}