using System.Collections.Immutable;
using System.Globalization;
using OpenQA.Selenium;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions.UIModels
{
    /// <summary>
    /// A question that maps UI element values to a data model by finding all the element of the container target
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class UIModelMany<T> : QuestionBase<ImmutableArray<T>>
    {
        private readonly UIModel.UIModelInfo _uIModelInfo;
        private readonly ITarget _containerTarget;
        private readonly CultureInfo _culture;

        internal UIModelMany(UIModel.UIModelInfo uIModelInfo, ITarget containerTarget, CultureInfo culture, string name)
        {
            _uIModelInfo = uIModelInfo;
            _containerTarget = containerTarget;
            _culture = culture;
            Name = name;
        }

        /// <inheritdoc />
        public override string Name { get; }

        /// <inheritdoc />
        protected override ImmutableArray<T> Answer(IActor actor)
        {
            var converter = (IConverter<IWebElement, T>)_uIModelInfo.CreateConverter(actor);
            var question = Element.Of(_containerTarget).WithCulture(_culture).Many().As(converter);
            return actor.AsksFor(question);
        }
    }
}
