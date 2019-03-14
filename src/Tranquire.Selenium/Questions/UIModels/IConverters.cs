using System.Collections.Immutable;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions.UIModels
{
    internal interface IConverters<T> : IConverter<string, T>, IConverter<bool, T>, IConverter<ImmutableArray<string>, T>
    {
    }
}
