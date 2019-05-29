namespace Tranquire.Extensions
{
    internal interface ISelectMany<out TResult> : INamed
    {
        TResult Apply(IActor actor);
    }
}
