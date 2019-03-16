using AutoFixture;
using AutoFixture.Idioms;
using System;
using System.Collections.Generic;
using System.Linq;
using Tranquire.Selenium.Questions.UIModels;
using Xunit;

namespace Tranquire.Selenium.Tests.Questions
{
    public class AttributesTests
    {
        public static IEnumerable<object[]> Attributes
        {
            get
            {
                return typeof(TargetAttribute)
                    .Assembly
                    .GetTypes()
                    .Where(t => typeof(TargetAttribute).IsAssignableFrom(t) || typeof(UIStateAttribute).IsAssignableFrom(t))
                    .Select(t => new object[] { t });
            }
        }

        [Theory, MemberData(nameof(Attributes))]
        public void VerifyAttributesGuardClauses(Type attributeType)
        {
            var fixture = new Fixture();
            var assertion = new GuardClauseAssertion(fixture);
            assertion.Verify(attributeType.GetConstructors());
        }
    }
}
