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

        internal UIModel(UIModel.UIModelInfo uIModelInfo, ITarget containerTarget, CultureInfo culture)
        {
            _uIModelInfo = uIModelInfo ?? throw new ArgumentNullException(nameof(uIModelInfo));
            _containerTarget = containerTarget ?? throw new ArgumentNullException(nameof(containerTarget));
            _culture = culture ?? throw new ArgumentNullException(nameof(culture));
        }

        internal UIModel(UIModel.UIModelInfo uIModelInfo, ITarget containerTarget)
            : this(uIModelInfo, containerTarget, CultureInfo.CurrentCulture)
        {
        }

        /// <inheritdoc />
        public override string Name => $"Get the model of {typeof(T).Name}";

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
        public UIModel<T> WithCulture(CultureInfo culture) => new UIModel<T>(_uIModelInfo, _containerTarget, culture);
    }
}
