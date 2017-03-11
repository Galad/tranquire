namespace Tranquire.Reporting
{
    /// <summary>
    /// A base calse for <see cref="ICanNotify"/>. All methods returns true;
    /// </summary>
    public abstract class CanNotify : ICanNotify
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public virtual bool Action<TResult>(IAction<TResult> action) => true;        
        public virtual bool Question<TResult>(IQuestion<TResult> question) => true;        
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
