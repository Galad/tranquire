using System.Collections.Generic;
using Tranquire.Selenium.Actions.Selects;

namespace Tranquire.Selenium.Actions;

/// <summary>
/// Allow the selection of values in a HTML select element
/// </summary>
public static class Select
{
    /// <summary>
    /// Select the given value
    /// </summary>
    /// <param name="value">The value to select</param>
    /// <returns>A <see cref="SelectBuilder{TValue}"/> instance that can used to configure the selection</returns>
    public static SelectBuilder<string> TheValue(string value)
    {
        return new SelectBuilder<string>(value, new SelectByValueStrategy());
    }

    /// <summary>
    /// Select the given values
    /// </summary>
    /// <param name="values">The values to select</param>
    /// <returns>A <see cref="SelectBuilder{TValue}"/> instance that can used to configure the selection</returns>
    public static SelectBuilderMany<string> TheValues(IEnumerable<string> values)
    {
        return new SelectBuilderMany<string>(values, new SelectByValueStrategy());
    }

    /// <summary>
    /// Select the given index
    /// </summary>
    /// <param name="index">The index to select</param>
    /// <returns>A <see cref="SelectBuilder{TValue}"/> instance that can used to configure the selection</returns>
    public static SelectBuilder<int> TheIndex(int index)
    {
        return new SelectBuilder<int>(index, new SelectByIndexStrategy());
    }

    /// <summary>
    /// Select the given indexes
    /// </summary>
    /// <param name="indexes">The indexes to select</param>
    /// <returns>A <see cref="SelectBuilder{TValue}"/> instance that can used to configure the selection</returns>
    public static SelectBuilderMany<int> TheIndexes(int[] indexes)
    {
        return new SelectBuilderMany<int>(indexes, new SelectByIndexStrategy());
    }

    /// <summary>
    /// Select the given text
    /// </summary>
    /// <param name="text">The text to select</param>
    /// <returns>A <see cref="SelectBuilder{TValue}"/> instance that can used to configure the selection</returns>
    public static SelectBuilder<string> TheText(string text)
    {
        return new SelectBuilder<string>(text, new SelectByTextStrategy());
    }

    /// <summary>
    /// Select the given texts
    /// </summary>
    /// <param name="texts">The texts to select</param>
    /// <returns>A <see cref="SelectBuilder{TValue}"/> instance that can used to configure the selection</returns>
    public static SelectBuilderMany<string> TheTexts(string[] texts)
    {
        return new SelectBuilderMany<string>(texts, new SelectByTextStrategy());
    }
}