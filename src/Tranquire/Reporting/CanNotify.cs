namespace Tranquire.Reporting
{
    /// <summary>
    /// A base calse for <see cref="ICanNotify"/>. All methods returns true;
    /// </summary>
    public abstract class CanNotify : ICanNotify
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public virtual bool Action<TResult>(IAction<TResult> action) => true;
#pragma warning disable CS0618 // Type or member is obsolete
        public virtual bool Action<TGivenAbility, TWhenAbility, TResult>(IAction<TGivenAbility, TWhenAbility, TResult> action) => true;
#pragma warning restore CS0618 // Type or member is obsolete
        public virtual bool Question<TResult>(IQuestion<TResult> question) => true;
        public virtual bool Question<TAbility, TResult>(IQuestion<TAbility, TResult> question) => true;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
