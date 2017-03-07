namespace Tranquire.Reporting
{
    /// <summary>
    /// Returns false when an action is a <see cref="ActionWithAbilityToActionAdapter{TGiven, TWhen, TResult}"/> instance
    /// </summary>
    public class CannotNotifyActionAdapter : CanNotify
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public override bool Action<TResult>(IAction<TResult> action)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            var type = action.GetType();
            return !type.IsGenericType ||
                   type.IsGenericType &&
                   type.GetGenericTypeDefinition() != typeof(ActionWithAbilityToActionAdapter<,,>);
        }
    }
}
