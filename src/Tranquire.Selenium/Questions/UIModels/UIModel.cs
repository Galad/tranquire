using OpenQA.Selenium;
using System;
using System.Globalization;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions.UIModels
{
    /// <summary>
    /// A question that maps UI element values to a data model
    /// </summary>
    /// <typeparam name="T">The object type that is mapped to the UI</typeparam>
    public sealed class UIModel<T> : QuestionBase<T>
    {
        private readonly UIModel.UIModelInfo _uIModelInfo;
        private readonly ITarget _containerTarget;
        private readonly CultureInfo _culture;
                
        internal UIModel(UIModel.UIModelInfo uIModelInfo, ITarget containerTarget, string name, CultureInfo culture = null)
        {
            _uIModelInfo = uIModelInfo ?? throw new ArgumentNullException(nameof(uIModelInfo));
            _containerTarget = containerTarget ?? throw new ArgumentNullException(nameof(containerTarget));
            _culture = culture ?? CultureInfo.CurrentCulture;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <inheritdoc />
        public override string Name { get; }

        /// <inheritdoc />
        protected override T Answer(IActor actor)
        {
            var converter = (IConverter<IWebElement, T>)_uIModelInfo.CreateConverter(actor);
            var question = Element.Of(_containerTarget).WithCulture(_culture).As(converter);
            return actor.AsksFor(question);
        }

        /// <summary>
        /// Specifiy a different culture
        /// </summary>
        /// <param name="culture">The new culture</param>
        /// <returns></returns>
        public UIModel<T> WithCulture(CultureInfo culture) => new UIModel<T>(_uIModelInfo, _containerTarget, Name, culture);

        /// <summary>
        /// Configure the question so that it finds all the elements matching the container target
        /// </summary>
        /// <returns></returns>
        public UIModelMany<T> Many()
        {
            return new UIModelMany<T>(_uIModelInfo, _containerTarget, _culture, Name);
        }
    }
}
