using System;
using System.Collections.Generic;

namespace Tranquire.Selenium.Actions.Selects
{
    public class SelectBuilder
    {
        private readonly ITarget _target;
        public SelectBuilder(ITarget target)
        {
            Guard.ForNull(target, nameof(target));
            _target = target;
        }

        public SelectBy<string> TheValue(string value)
        {
            return new SelectBy<string>(_target, value, new SelectByValueStrategy());
        }

        public SelectByMany<string> TheValues(IEnumerable<string> values)
        {
            return new SelectByMany<string>(_target, values, new SelectByValueStrategy());
        }

        public SelectBy<int> TheIndex(int index)
        {
            return new SelectBy<int>(_target, index, new SelectByIndexStrategy());
        }

        public SelectByMany<int> TheIndexes(int[] indexes)
        {
            return new SelectByMany<int>(_target, indexes, new SelectByIndexStrategy());
        }

        public SelectBy<string> TheText(string text)
        {
            return new SelectBy<string>(_target, text, new SelectByTextStrategy());
        }

        public SelectByMany<string> TheTexts(string[] texts)
        {
            return new SelectByMany<string>(_target, texts, new SelectByTextStrategy());
        }
    }
}