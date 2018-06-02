namespace Tranquire
{

    internal static class CommandExtensions
    {
        public static IAction<TResult> AsAction<TResult>(this IWhenCommand<TResult> command)
        {
            return new CommandAction<TResult>(
                new WhenCommandAsAction<TResult>(command),
                Reporting.ActionContext.When
                );
        }

        public static IAction<TResult> AsAction<TResult>(this IGivenCommand<TResult> command)
        {
            return new CommandAction<TResult>(
                new GivenCommandAsAction<TResult>(command),
                Reporting.ActionContext.Given
                );
        }
    }
}
