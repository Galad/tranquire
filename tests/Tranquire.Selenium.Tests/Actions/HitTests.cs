using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Tranquire.Selenium.Actions;
using Tranquire.Selenium.Questions;
using Xunit;

namespace Tranquire.Selenium.Tests.Actions;

public class HitTests : WebDriverTest
{
    public HitTests(WebDriverFixture fixture) : base(fixture, "Hit.html")
    {
    }

    public static IEnumerable<object[]> HitActions =>
        new[] {
        new object[]{ Hit.Enter(), "Enter"},
        new object[]{ Hit.Escape(), "Escape" },
        new object[]{ Hit.Keys("y"), "y"}
    };

    [Theory, MemberData(nameof(HitActions))]
    public void Hit_WithTarget_ShouldHitKeys(Hit hitAction, string expected)
    {
        //arrange
        var target = Target.The("hit target").LocatedBy(By.Id("Hit"));
        var action = hitAction.Into(target);
        //act
        Fixture.Actor.When(action);
        var actual = Answer(TextContent.Of(Target.The("Hit result").LocatedBy(By.Id("HitResult"))));
        //assert
        Assert.Equal(expected, actual);
    }
}
