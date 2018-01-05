namespace Tranquire
{

    internal static class CommandExtensions
    {
        public static IAction<TResult> AsAction<TResult>(this IWhenCommand<TResult> command) => new WhenCommandAsAction<TResult>(command);
        public static IAction<TResult> AsAction<TResult>(this IGivenCommand<TResult> command) => new GivenCommandAsAction<TResult>(command);
    }
}
