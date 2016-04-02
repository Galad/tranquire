using System.Collections.Generic;
using Tranquire.Selenium.Actions.Selects;

namespace Tranquire.Selenium.Actions
{
    public class SelectBuilderMany<TValue> : TargetableAction<SelectByMany<TValue>>
    {
        public SelectBuilderMany(IEnumerable<TValue> value, ISelectStrategy<TValue> strategy)
            : base(t => new SelectByMany<TValue>(t, value, strategy))
        {
        }
    }
}