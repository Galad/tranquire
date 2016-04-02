using System.Collections.Generic;
using Tranquire.Selenium.Actions.Selects;

namespace Tranquire.Selenium.Actions
{
    public static class Select
    {
        public static SelectBuilder<string> TheValue(string value)
        {
            return new SelectBuilder<string>(value, new SelectByValueStrategy());
        }

        public static SelectBuilderMany<string> TheValues(IEnumerable<string> values)
        {
            return new SelectBuilderMany<string>(values, new SelectByValueStrategy());
        }

        public static SelectBuilder<int> TheIndex(int index)
        {
            return new SelectBuilder<int>(index, new SelectByIndexStrategy());
        }

        public static SelectBuilderMany<int> TheIndexes(int[] indexes)
        {
            return new SelectBuilderMany<int>(indexes, new SelectByIndexStrategy());
        }

        public static SelectBuilder<string> TheText(string text)
        {
            return new SelectBuilder<string>(text, new SelectByTextStrategy());
        }

        public static SelectBuilderMany<string> TheTexts(string[] texts)
        {
            return new SelectBuilderMany<string>(texts, new SelectByTextStrategy());
        }
    }
}