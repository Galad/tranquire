using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tranquire.Tests
{
    public class ActorTests
    {
        public class Ability1 { }
        public class Ability2 { }
        public class Ability3 { }

        public class ActorCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Register<IReadOnlyDictionary<Type, object>>(() =>
                {
                    return new Dictionary<Type, object>()
                    {
                        { typeof(Ability1), new Ability1() },
                        { typeof(Ability2), new Ability2() },
                        { typeof(Ability3), new Ability3() },
                    };
                });
            }
        }

        public class ActorAutoData : AutoDataAttribute
        {
            public ActorAutoData() : base(new Fixture().Customize(new ActorCustomization()).Customize(new DomainCustomization()))
            {
            }
        }

        public class AbilityTest : IAbility
        {
            public AbilityTest AsActor(IActor actor)
            {
                return this;
            }
        }

        public class AbilityTest2 : IAbility
        {
            public AbilityTest2 AsActor(IActor actor)
            {
                return this;
            }
        }

        [Theory, DomainAutoData]
        public void Sut_ShouldBeActor(Actor sut)
        {
            Assert.IsAssignableFrom(typeof(IActor), sut);
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorParameters(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(Actor));
        }

        [Theory, DomainAutoData]
        public void Abilities_WhenUsingModestConstructor_ShouldBeEmpty([Modest]Actor sut)
        {
            sut.Abilities.Should().BeEmpty();
        }

        #region Can
        [Theory, DomainAutoData]
        public void Can_ShouldReturnCorrectValue(
        [Modest]Actor sut,
        AbilityTest ability)
        {
            var expected = new[] { ability };
            //act
            var actual = sut.Can(ability);
            //assert
            actual.Should().BeOfType<Actor>().Which.Abilities.Values.Should().Equal(expected);
        }

        [Theory, ActorAutoData]
        public void Can_WhenActorHasAbilities_ShouldReturnCorrectValue(
           [Greedy]Actor sut,
           AbilityTest ability)
        {
            //
            var expected = sut.Abilities.Values.Concat(new[] { ability }).ToArray();
            //act
            var actual = sut.Can(ability);
            //assert
            actual.Should().BeOfType<Actor>().Which.Abilities.Values.Should().Equal(expected);
        }
        #endregion

        #region Execute
        [Theory, DomainAutoData]
        public void Execute_ShouldCallExecuteWhen(
           Actor sut,
           Mock<IAction> action)
        {
            //act            
            sut.Execute(action.Object);
            //assert
            action.Verify(a => a.ExecuteWhenAs(sut));
        }

        [Theory, DomainAutoData]
        public void Execute_CalledAfterCallingAttempsTo_ShouldCallExecuteWhen(
           Actor sut,
           Mock<IAction> action)
        {
            var secondAction = new Mock<IAction>(MockBehavior.Loose);
            IActor actual = null;
            action.Setup(p => p.ExecuteWhenAs(It.IsAny<IActor>())).Callback((IActor actor) => actual = actor);
            //act
            sut.AttemptsTo(action.Object);
            actual.Execute(secondAction.Object);
            //assert
            secondAction.Verify(a => a.ExecuteWhenAs(It.IsAny<IActor>()));
        }

        [Theory, DomainAutoData]
        public void Execute_CalledAfterCallingWasAbleTo_ShouldCallExecuteGiven(
           Actor sut,
           Mock<IAction> action)
        {
            var secondAction = new Mock<IAction>(MockBehavior.Loose);
            IActor actual = null;
            action.Setup(p => p.ExecuteGivenAs(It.IsAny<IActor>())).Callback((IActor actor) => actual = actor);
            //act
            sut.WasAbleTo(action.Object);
            actual.Execute(secondAction.Object);
            //assert
            secondAction.Verify(a => a.ExecuteGivenAs(It.IsAny<IActor>()));
        }

        [Theory, ActorAutoData]
        public void ExecuteWithAbility_ShouldCallExecuteWhen(
          [Greedy]Actor sut,
          Mock<IAction<Ability1, Ability2>> action)
        {
            var expected = sut.Abilities.Values.OfType<Ability2>().First();
            //act            
            sut.Execute(action.Object);
            //assert
            action.Verify(a => a.ExecuteWhenAs(sut, expected));
        }

        [Theory, ActorAutoData]
        public void ExecuteWithAbility_CalledAfterCallingAttempsTo_ShouldCallExecuteWhen(
           [Greedy]Actor sut,
           Mock<IAction<Ability1, Ability2>> action)
        {
            var expectedAbility = sut.Abilities.Values.OfType<Ability2>().First();
            var secondAction = new Mock<IAction<Ability1, Ability2>>(MockBehavior.Loose);
            IActor actual = null;
            action.Setup(p => p.ExecuteWhenAs(It.IsAny<IActor>(), It.IsAny<Ability2>())).Callback((IActor actor, Ability2 _) => actual = actor);
            //act
            sut.AttemptsTo(action.Object);
            actual.Execute(secondAction.Object);
            //assert
            secondAction.Verify(a => a.ExecuteWhenAs(It.IsAny<IActor>(), expectedAbility));
        }

        [Theory, ActorAutoData]
        public void ExecuteWithAbility_CalledAfterCallingWasAbleTo_ShouldCallExecuteGiven(
           [Greedy]Actor sut,
           Mock<IAction<Ability1, Ability2>> action)
        {
            var expectedAbility = sut.Abilities.Values.OfType<Ability1>().First();
            var secondAction = new Mock<IAction<Ability1, Ability2>>(MockBehavior.Loose);
            IActor actual = null;
            action.Setup(p => p.ExecuteGivenAs(It.IsAny<IActor>(), It.IsAny<Ability1>())).Callback((IActor actor, Ability1 _) => actual = actor);
            //act
            sut.WasAbleTo(action.Object);
            actual.Execute(secondAction.Object);
            //assert
            secondAction.Verify(a => a.ExecuteGivenAs(It.IsAny<IActor>(), expectedAbility));
        }
        #endregion

        #region AttemptsTo
        [Theory, DomainAutoData]
        public void AttemptsTo_ShouldCallExecuteWhen(
          Actor sut,
          Mock<IAction> action)
        {
            //act
            sut.AttemptsTo(action.Object);
            //assert
            action.Verify(a => a.ExecuteWhenAs(It.IsAny<IActor>()));
        }

        [Theory, ActorAutoData]
        public void AttemptsToWithAbility_ShouldCallExecuteWhen(
         [Greedy]Actor sut,
         Mock<IAction<Ability1, Ability2>> action)
        {
            //arrange
            var expected = sut.Abilities.Values.OfType<Ability2>().First();
            //act
            sut.AttemptsTo(action.Object);
            //assert
            action.Verify(a => a.ExecuteWhenAs(It.IsAny<IActor>(), expected));
        }

        [Theory, DomainAutoData]
        public void WasAbleTo_ShouldCallExecuteGiven(
         Actor sut,
         Mock<IAction> action)
        {
            //act
            sut.WasAbleTo(action.Object);
            //assert
            action.Verify(a => a.ExecuteGivenAs(It.IsAny<IActor>()));
        }

        [Theory, ActorAutoData]
        public void WasAbleToWithAbility_ShouldCallExecuteGiven(
          [Greedy]Actor sut,
          Mock<IAction<Ability1, Ability2>> action)
        {
            //arrange
            var expected = sut.Abilities.Values.OfType<Ability1>().First();
            //act
            sut.WasAbleTo(action.Object);
            //assert
            action.Verify(a => a.ExecuteGivenAs(It.IsAny<IActor>(), expected));
        }
        #endregion

        #region AsksFor
        [Theory, DomainAutoData]
        public void AsksFor_ShouldReturnCorrectValue(
           Actor sut,
           Mock<IQuestion<object>> question,
           object expected)
        {
            question.Setup(q => q.AnsweredBy(sut)).Returns(expected);
            //act
            var actual = sut.AsksFor(question.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void AsksFor_AfterCallingWasAbleTo_ShouldReturnCorrectValue(
          Actor actor,
          Mock<IQuestion<object>> question,
          Mock<IGivenCommand> givenCommand,
          object expected)
        {
            IActor sut = null;
            givenCommand.Setup(g => g.ExecuteGivenAs(It.IsAny<IActor>())).Callback((IActor a) => sut = a);
            question.Setup(q => q.AnsweredBy(actor)).Returns(expected);
            actor.WasAbleTo(givenCommand.Object);
            //act
            var actual = sut.AsksFor(question.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, ActorAutoData]
        public void AsksFor_WithAbility_ShouldReturnCorrectValue(
          [Greedy]Actor sut,
          Mock<IQuestion<object, Ability1>> question,
          object expected)
        {
            //arrange
            var ability = sut.Abilities.Values.OfType<Ability1>().First();
            question.Setup(q => q.AnsweredBy(sut, ability)).Returns(expected);
            //act
            var actual = sut.AsksFor(question.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, ActorAutoData]
        public void AsksFor_AfterCallingWasAbleTo_ShouldReturnCorrectValue(
         [Greedy]Actor actor,
         Mock<IQuestion<object, Ability1>> question,
         Mock<IGivenCommand<Ability1>> givenCommand,
         object expected)         
        {
            var expectedAbility = actor.Abilities.Values.OfType<Ability1>().First();
            IActor sut = null;
            givenCommand.Setup(g => g.ExecuteGivenAs(It.IsAny<IActor>(), expectedAbility)).Callback((IActor a, Ability1 _) => sut = a);
            question.Setup(q => q.AnsweredBy(actor, expectedAbility)).Returns(expected);
            actor.WasAbleTo(givenCommand.Object);
            //act
            var actual = sut.AsksFor(question.Object);
            //assert
            Assert.Equal(expected, actual);
        }
        #endregion

        public static IEnumerable<object[]> ActionsWithAbility
        {
            get
            {
                return new Action<Actor, IFixture>[]
                {
                    (sut, fixture) => sut.AsksFor(fixture.Create<IQuestion<string, AbilityTest>>()),
                    (sut, fixture) => sut.AttemptsTo(fixture.Create<IWhenCommand<AbilityTest>>()),
                    (sut, fixture) => sut.WasAbleTo(fixture.Create<IGivenCommand<AbilityTest>>()),
                    (sut, fixture) => sut.Execute(fixture.Create<IAction<AbilityTest, AbilityTest>>()),
                }
                .Select(a => new object[] { a });
            }
        }

        [Theory, MemberData("ActionsWithAbility")]
        public void ActionWithAbility_WhenAbilityIsNotRegistered_ShouldThrow(Action<Actor, IFixture> action)
        {
            //arrange
            var fixture = new Fixture().Customize(new ActorCustomization()).Customize(new AutoConfiguredMoqCustomization());
            var sut = fixture.Create<Actor>();            
            System.Action testedAction = () => action(sut, fixture);
            //act and assert
            testedAction.ShouldThrow<InvalidOperationException>().Where(ex => ex.Message.Contains(typeof(AbilityTest).Name));            
        }
    }
}
