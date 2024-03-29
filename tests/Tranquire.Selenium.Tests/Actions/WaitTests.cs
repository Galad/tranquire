﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using OpenQA.Selenium;
using Tranquire.Selenium.Actions;
using Tranquire.Selenium.Questions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Actions;

public class WaitTests : WebDriverTest
{
    public WaitTests(WebDriverFixture fixture) : base(fixture, "Wait.html")
    {
    }

    [Theory, DomainAutoData]
    public void UntilTargetIsPresent_ShouldWait(string id)
    {
        //arrange
        var target = Target.The("element to wait for").LocatedBy(By.Id(id));
        InsertElementAfter2Second(id);
        //act
        Fixture.Actor.When(Wait.Until(target).IsPresent);
        //assert
        var actual = Answer(Presence.Of(target));
        Assert.True(actual);
    }

    [Theory, DomainAutoData]
    public void UntilTargetIsPresent_WhenTimeout_ShouldThrow(string id)
    {
        //arrange
        var target = Target.The("element to wait for").LocatedBy(By.Id(id));
        InsertElementAfter2Second(id);
        //act
        Assert.Throws<TimeoutException>(() => Fixture.Actor.When(Wait.Until(target).IsPresent.Timeout(TimeSpan.FromMilliseconds(100))));
    }

    private void InsertElementAfter2Second(string id)
    {
        var js = "var element = document.createElement('div');" +
                             "element.id = '" + id + "';" +
                             "element.innerText = '" + id + "';" +
                             "document.body.appendChild(element)";
        js = "setTimeout(function(){" + js + "}, 2000);";
        Fixture.WebDriver.ExecuteScript(js);
    }

    [Theory, DomainAutoData]
    public void UntilQuestionIsAnswered_ShouldWait(string expected)
    {
        //arrange
        var target = Target.The("element to wait for").LocatedBy(By.Id("ChangeTextElement"));
        ChangeText(expected);
        var question = TextContent.Of(target);
        //act
        Fixture.Actor.When(Wait.UntilQuestionIsAnswered(question, t => t == expected));
        //arrange
        var actual = Answer(question);
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void UntilQuestionIsAnswered_WhenTimeout_ShouldThrow(string expected)
    {
        //arrange
        var target = Target.The("element to wait for").LocatedBy(By.Id("ChangeTextElement"));
        ChangeText(expected);
        var question = TextContent.Of(target);
        //act
        Assert.Throws<TimeoutException>(() =>
        Fixture.Actor.When(Wait.UntilQuestionIsAnswered(question, t => t == expected)
                                     .Timeout(TimeSpan.FromMilliseconds(100))
                                ));
    }

    private void ChangeText(string expected)
    {
        var js = $"var element = document.getElementById('ChangeTextElement').innerText = '{expected}';";
        js = "setTimeout(function(){" + js + "}, 5000);";
        Fixture.WebDriver.ExecuteScript(js);
    }

    [Theory]
    [DomainInlineAutoData(VisibilityHidden, VisibilityVisible)]
    [DomainInlineAutoData(DisplayNone, DisplayBlock)]
    public void UntilTargetIsVisible_ShouldWait(string stateBefore, string stateAfter, string id)
    {
        // arrange
        var target = Target.The("element to wait for").LocatedBy(By.Id(id));
        InsertElementAndChangeStateAfter1Second(id, stateBefore, stateAfter);
        // act
        Fixture.Actor.When(Wait.Until(target).IsVisible);
        // assert
        var actual = Answer(Visibility.Of(target));
        Assert.True(actual);
    }

    [Theory]
    [DomainInlineAutoData(VisibilityHidden)]
    [DomainInlineAutoData(DisplayNone)]
    public void UntilTargetIsVisible_WhenTimeout_ShouldThrow(string state, string id)
    {
        // arrange
        var target = Target.The("element to wait for").LocatedBy(By.Id(id));
        InsertElementAndChangeStateAfter1Second(id, state, state);
        // act
        Assert.Throws<TimeoutException>(() => Fixture.Actor.When(Wait.Until(target).IsVisible.Timeout(TimeSpan.FromMilliseconds(100))));
    }

    [Theory]
    [DomainInlineAutoData(VisibilityVisible, VisibilityHidden)]
    [DomainInlineAutoData(DisplayBlock, DisplayNone)]
    public void UntilTargetIsNotVisible_ShouldWait(string stateBefore, string stateAfter, string id)
    {
        // arrange
        var target = Target.The("element to wait for").LocatedBy(By.Id(id));
        InsertElementAndChangeStateAfter1Second(id, stateBefore, stateAfter);
        // act
        Fixture.Actor.When(Wait.Until(target).IsNotVisible);
        // assert
        var actual = Answer(Visibility.Of(target));
        Assert.False(actual);
    }

    [Theory]
    [DomainInlineAutoData(VisibilityVisible)]
    [DomainInlineAutoData(DisplayBlock)]
    public void UntilTargetIsNotVisible_WhenTimeout_ShouldThrow(string state, string id)
    {
        // arrange
        var target = Target.The("element to wait for").LocatedBy(By.Id(id));
        InsertElementAndChangeStateAfter1Second(id, state, state);
        // act
        Assert.Throws<TimeoutException>(() => Fixture.Actor.When(Wait.Until(target).IsNotVisible.Timeout(TimeSpan.FromMilliseconds(100))));
    }

    private const string VisibilityHidden = "element.style.visibility = 'hidden';";
    private const string VisibilityVisible = "element.style.visibility = 'visible';";
    private const string DisplayBlock = "element.style.display = 'block';";
    private const string DisplayNone = "element.style.display= 'none';";


    private void InsertElementAndChangeStateAfter1Second(string id, string stateBefore, string stateAfter)
    {
        var js = "var element = document.createElement('div');" +
                             $"element.id = '{id}';" +
                             "element.innerText = 'test';" +
                             stateBefore +
                             "document.body.appendChild(element)";
        js = js + "\nsetTimeout(function(){" + stateAfter + "}, 1000);";
        Fixture.WebDriver.ExecuteScript(js);
    }

    [Fact]
    public void During_ShouldWait()
    {
        // arrange
        var timeToWait = TimeSpan.FromMilliseconds(100);
        var action = Wait.During(timeToWait);
        var sw = Stopwatch.StartNew();
        //act
        Fixture.Actor.When(action);
        // assert
        sw.Stop();
        sw.Elapsed.Should().BeGreaterOrEqualTo(timeToWait);
    }

    [Fact]
    public async Task DuringAsync_ShouldWait()
    {
        // arrange
        var timeToWait = TimeSpan.FromMilliseconds(100);
        var action = Wait.During(timeToWait).Async;
        var sw = Stopwatch.StartNew();
        //act
        await Fixture.Actor.When(action);
        // assert
        sw.Stop();
        sw.Elapsed.Should().BeGreaterOrEqualTo(timeToWait);
    }
}
