using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Tranquire.Selenium.Questions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Questions;

public class SingleUIStateTests
{
    public static IEnumerable<object[]> SingleUIStateValues
    {
        get
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization() { ConfigureMembers = true }).Customize(new DomainCustomization());
            return typeof(SingleUIState<,>).Assembly
                                           .GetTypes()
                                           .Where(t => InheritGenericTypeDefinition(t, typeof(SingleUIState<,>)))
                                           .Select(t => new object[] { fixture.Create(t, new SpecimenContext(fixture)) });
        }
    }

    [Theory, MemberData(nameof(SingleUIStateValues))]
    public void Many_ShouldReturnCorrectValue(object value)
    {
        Assert.NotNull(value.GetType().GetMethod("Many").Invoke(value, new object[] { }));
    }

    [Theory, MemberData(nameof(SingleUIStateValues))]
    public void WithCulture_ShouldReturnCorrectCultureValue_ShouldReturnCorrectValue(object value)
    {
        //arrange
        var expected = new CultureInfo("de-DE");
        var cultureMethod = value.GetType().GetProperty("Culture").GetMethod;
        var withCulture = value.GetType().GetMethod("WithCulture");
        //act
        var actual = cultureMethod.Invoke(withCulture.Invoke(value, new object[] { expected }), new object[] { });
        //assert
        Assert.Equal(expected, actual);
    }

    private static bool InheritGenericTypeDefinition(Type type, Type genericTypeDefinition)
    {
        if (type.BaseType == null || type.BaseType == typeof(object))
        {
            return false;
        }
        if (type.BaseType.IsGenericType &&
           type.BaseType.GetGenericTypeDefinition() == genericTypeDefinition)
        {
            return true;
        }
        return InheritGenericTypeDefinition(type.BaseType, genericTypeDefinition);
    }
}
