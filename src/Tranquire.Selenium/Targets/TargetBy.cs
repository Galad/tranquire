using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Text.RegularExpressions;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Targets
{
    /// <summary>
    /// Represent a target located by a <see cref="By"/> object
    /// </summary>    
    public class TargetBy : ITarget
    {
        /// <summary>
        /// Creates a new instance of <see cref="TargetBy"/>
        /// </summary>
        /// <param name="by">The <see cref="By"/> locator</param>
        /// <param name="name">The target name</param>
        public TargetBy(By by, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentNullException(nameof(name), ExceptionMessages.ArgumentCannotBeNullOrEmpty);
            }

            By = by ?? throw new System.ArgumentNullException(nameof(by));
            Name = name;
        }

        /// <summary>
        /// Gets the target locator
        /// </summary>
        public By By { get; }
        /// <summary>
        /// Gets the target name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns a <see cref="IWebElement"/> corresponding to the target
        /// </summary>
        /// <param name="searchContext">The <see cref="IWebDriver"/> used to retrieved the element</param>
        /// <returns></returns>
        public IWebElement ResolveFor(ISearchContext searchContext)
        {
            if (searchContext == null)
            {
                throw new System.ArgumentNullException(nameof(searchContext));
            }

            return searchContext.FindElement(By);
        }

        /// <summary>
        /// Returns an array of <see cref="IWebElement"/> corresponding to all targets
        /// </summary>
        /// <param name="searchContext">The <see cref="IWebDriver"/> used to retrieved the element</param>
        /// <returns></returns>
        public ImmutableArray<IWebElement> ResolveAllFor(ISearchContext searchContext)
        {
            if (searchContext == null)
            {
                throw new System.ArgumentNullException(nameof(searchContext));
            }

            return searchContext.FindElements(By).ToImmutableArray();
        }

        /// <summary>
        /// Returns the action's name
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name + $" ({By.ToString()})";

        /// <inheritdoc />
        public ITarget RelativeTo(ITarget targetSource)
        {
            if (!(targetSource is TargetBy sourceTargetBy))
            {
                return new RelativeTarget(targetSource, this);
            }

            var (sourceValue, sourceByType) = GetByInfo(sourceTargetBy.By);
            var (relativeValue, relativeByType) = GetByInfo(By);

            if (sourceByType == ByType.Other || relativeByType == ByType.Other)
            {
                return new RelativeTarget(targetSource, this);
            }

            var by = By.CssSelector(AsCssSelector(sourceValue, sourceByType) + " " + AsCssSelector(relativeValue, relativeByType));
            return new TargetBy(by, Name);

            string AsCssSelector(string value, ByType byType)
            {
                switch (byType)
                {
                    case ByType.Id:
                        return "#" + value;
                    case ByType.ClassName:
                        return "." + value;
                    case ByType.CssSelector:
                        return value;
                    case ByType.Name:
                        return "[name=" + value + "]";
                    case ByType.TagName:
                        return value;
                    case ByType.Other:
                    default:
                        return value;
                }
            }
        }

        private enum ByType
        {
            Id,
            ClassName,
            CssSelector,
            Name,
            TagName,
            Other
        }

        private static readonly PropertyInfo _byDescriptionProperty
            = typeof(By).GetProperty("Description", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly Regex _byDescriptionRegex = new Regex(@"(?<type>By\.[^:]*):\s(?<value>.*)", RegexOptions.Compiled);
        private static readonly Dictionary<string, ByType> _byTypeMap = new Dictionary<string, ByType>()
        {
            { "By.Id", ByType.Id },
            { "By.ClassName[Contains]", ByType.ClassName },
            { "By.CssSelector", ByType.CssSelector },
            { "By.Name", ByType.Name },
            { "By.TagName", ByType.TagName }
        };

        private static (string selector, ByType type) GetByInfo(By by)
        {
            var description = (string)_byDescriptionProperty.GetValue(by);
            var match = _byDescriptionRegex.Match(description);
            if (!match.Success)
            {
                return (null, ByType.Other);
            }

            var type = match.Groups["type"].Value;
            if (!_byTypeMap.TryGetValue(type, out var byType))
            {
                return (null, ByType.Other);
            }
            var value = match.Groups["value"].Value;
            return (value, byType);
        }
    }
}