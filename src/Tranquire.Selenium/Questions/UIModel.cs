using OpenQA.Selenium;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Tranquire.Selenium.Questions.Converters;
using Tranquire.Selenium.Questions.UIModels;
using Tranquire.Selenium.Questions.UIModels.Converters;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Provides questions that maps UI element values to a data model.
    /// You can define a UI model by using the attributes <see cref="TargetAttribute"/> and attributes deriving from <see cref="UIStateAttribute"/>
    /// </summary>
    public static class UIModel
    {
        internal class UIModelInfo
        {
            public UIModelInfo(Func<IActor, object> createConverter)
            {
                CreateConverter = createConverter;
            }

            public Func<IActor, object> CreateConverter { get; }
        }

        private static readonly ConcurrentDictionary<Type, UIModelInfo> _containers = new ConcurrentDictionary<Type, UIModelInfo>();

        /// <summary>
        /// Creates a question for the given model type
        /// </summary>
        /// <typeparam name="T">The model type that contains properties targeting UI elements</typeparam>
        /// <param name="containerTarget">The target that describe the container for the data</param>
        /// <returns></returns>
        public static UIModel<T> Of<T>(ITarget containerTarget) where T : new()
        {
            if (containerTarget == null)
            {
                throw new ArgumentNullException(nameof(containerTarget));
            }

            var modelInfo = _containers.GetOrAdd(typeof(T), _ => GetUIModelInfo<T>());
            return new UIModel<T>(modelInfo, containerTarget);
        }

        private static UIModelInfo GetUIModelInfo<T>() where T : new()
        {
            var type = typeof(T);
            var setValues = type.GetProperties()
                                 .Select(pi => (pi, targetAttribute: GetTargetAttribute(pi), uiStateAttribute: GetUIStateAttribute(pi)))
                                 .Where(p => p.targetAttribute != null)
                                 .Select(p => (p.pi, retrieveValue: RetrieveValue(p.pi, p.targetAttribute.GetSeleniumBy(), p.uiStateAttribute ?? new TextContentAttribute())))
                                 .Select(f => ExecuteQuestions(f.pi, f.retrieveValue))
                                 .ToArray();

            return new UIModelInfo(actor => new ModelConverterBySettingValues<T>(actor, setValues));
        }

        private static TargetAttribute GetTargetAttribute(PropertyInfo pi)
        {
            return pi.GetCustomAttributes(typeof(TargetAttribute), true).Cast<TargetAttribute>().SingleOrDefault();
        }

        private static UIStateAttribute GetUIStateAttribute(PropertyInfo pi)
        {
            return pi.GetCustomAttributes(typeof(UIStateAttribute), true).Cast<UIStateAttribute>().SingleOrDefault();
        }


        private sealed class ModelConverterBySettingValues<T> : IConverter<IWebElement, T>
            where T : new()
        {
            private readonly IActor actor;
            private readonly IEnumerable<Action<IActor, ITarget, CultureInfo, object>> setValues;

            public ModelConverterBySettingValues(IActor actor, IEnumerable<Action<IActor, ITarget, CultureInfo, object>> setValues)
            {
                this.actor = actor;
                this.setValues = setValues;
            }

            public T Convert(IWebElement value, CultureInfo culture)
            {
                var model = new T();
                var container = Target.The("container").LocatedByWebElement(value);
                foreach (var setValue in setValues)
                {
                    setValue(actor, container, culture, model);
                }
                return model;
            }
        }

        private static Action<IActor, ITarget, CultureInfo, object> ExecuteQuestions(PropertyInfo pi, Func<IActor, ITarget, CultureInfo, object> f)
        {
            return (actor, container, culture, model) => pi.SetValue(model, f(actor, container, culture));
        }

        private static Func<IActor, ITarget, CultureInfo, object> RetrieveValue(PropertyInfo pi, By by, UIStateAttribute valueAttribute)
        {
            var target = Target.The(pi.Name).LocatedBy(by);
            return ApplyGetConverter(target, pi.PropertyType, valueAttribute);
        }

        private static readonly IntegerConverters _integerConverters = new IntegerConverters();
        private static readonly BooleanConverters _booleanConverters = new BooleanConverters();
        private static readonly TextConverters _textConverters = new TextConverters();
        private static readonly DateTimeConverters _dateTimeConverters = new DateTimeConverters();
        private static readonly DoubleConverters _doubleConverters = new DoubleConverters();
        private static readonly StringArrayConverters _stringArrayConverters = new StringArrayConverters();
        private static readonly IntegerArrayConverters _integerArrayConverters = new IntegerArrayConverters();
        private static readonly DoubleArrayConverters _doubleArrayConverters = new DoubleArrayConverters();

        private static Func<IActor, ITarget, CultureInfo, object> ApplyGetConverter(ITarget target, Type type, UIStateAttribute uiStateAttribute)
        {
            if (type == typeof(int))
            {
                return GetFunction(_integerConverters);
            }
            if (type == typeof(string))
            {
                return GetFunction(_textConverters);
            }
            if (type == typeof(bool))
            {
                return GetFunction(_booleanConverters);
            }
            if (type == typeof(DateTime))
            {
                return GetFunction(_dateTimeConverters);
            }
            if (type == typeof(double))
            {
                return GetFunction(_doubleConverters);
            }
            if (type == typeof(ImmutableArray<string>))
            {
                return GetFunction(_stringArrayConverters);
            }
            if (type == typeof(ImmutableArray<int>))
            {
                return GetFunction(_integerArrayConverters);
            }
            if (type == typeof(ImmutableArray<double>))
            {
                return GetFunction(_doubleArrayConverters);
            }


            throw new NotSupportedException($"Type {type} is not supported");

            Func<IActor, ITarget, CultureInfo, object> GetFunction<T>(IConverters<T> converters)
            {
                return (actor, container, culture) =>
                {
                    var relativeTarget = target.RelativeTo(container);
                    var question = uiStateAttribute.CreateQuestion(relativeTarget, converters, culture);
                    return actor.AsksFor(question);
                };
            }
        }
    }
}
