using Tranquire.Selenium.Actions.Selects;

namespace Tranquire.Selenium.Actions
{
    public class SelectBuilder<TValue> : TargetableAction<SelectBy<TValue>>
    {
        public SelectBuilder(TValue value, ISelectStrategy<TValue> strategy)
            : base(t => new SelectBy<TValue>(t, value, strategy))
        {
        }
    }
}