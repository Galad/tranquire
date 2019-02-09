using OpenQA.Selenium;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Tranquire.Selenium.Targets
{
    /// <summary>
    /// Represent a target located by a <see cref="By"/> object
    /// </summary>    
    public class TargetBy : TargetByBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="TargetBy"/>
        /// </summary>
        /// <param name="by">The <see cref="By"/> locator</param>
        /// <param name="name">The target name</param>
        public TargetBy(By by, string name) : base(by, name)
        {
        }

        /// <summary>
        /// Return the <see cref="ISearchContext"/> for the current target
        /// </summary>
        /// <param name="webDriver"></param>
        /// <returns></returns>
        protected override ISearchContext SearchContext(IWebDriver webDriver)
        {
            return webDriver;
        }

        /// <inheritdoc />
        public override ITarget RelativeTo(ITarget targetSource)
        {
            if (!(targetSource is TargetBy sourceTargetBy))
            {
                return base.RelativeTo(targetSource);
            }

            var (sourceValue, sourceByType) = GetByInfo(sourceTargetBy.By);
            var (relativeValue, relativeByType) = GetByInfo(By);

            if (sourceByType == ByType.Other || relativeByType == ByType.Other)
            {
                return base.RelativeTo(targetSource);
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

        private static PropertyInfo _byDescriptionProperty
            = typeof(By).GetProperty("Description", BindingFlags.NonPublic | BindingFlags.Instance);
        private static Regex _byDescriptionRegex = new Regex(@"(?<type>By\.[^:]*):\s(?<value>.*)", RegexOptions.Compiled);
        private static Dictionary<string, ByType> _byTypeMap = new Dictionary<string, ByType>()
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