using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Tranquire.Selenium.Questions.UIModels;
using Xunit;

namespace Tranquire.Selenium.Tests.Questions.UIModels;

public class TargetAttributeTests
{
    public static IEnumerable<object[]> ByMethods
    {
        get
        {
            return Enum.GetValues(typeof(ByMethod))
                       .Cast<ByMethod>()
                       .Select(b => new object[] { b });
        }
    }

    [Theory, MemberData(nameof(ByMethods))]
    public void AllByMethodsCanMapToTarget(ByMethod byMethod)
    {
        // arrange
        var targetName = "target";
        var sut = new TargetAttribute(byMethod, "targetId");
        // act
        var actual = sut.CreateTarget(targetName);
        // arrange
        Assert.NotNull(actual);
        Assert.Equal(actual.Name, targetName);
    }
}
