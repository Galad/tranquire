using AutoFixture;
using AutoFixture.Xunit2;
using BoDi;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TechTalk.SpecFlow;
using Xunit;

namespace Tranquire.SpecFlow.Generation.Tests
{
    public class StaticFactoriesTests
    {
        public static IEnumerable<object[]> GeneratedStepsMethods
        {
            get
            {
                return typeof(Add_Steps).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                        .Where(m => m.Name.StartsWith("Given") || m.Name.StartsWith("When"))
                                        .Select(m => new object[] { m });
            }
        }

        [Theory, MemberData(nameof(GeneratedStepsMethods))]
        public void GeneratedStepsMethods_HaveCorrectAttribute(MethodInfo method)
        {
            Type expectedAttribute;
            if (method.Name.StartsWith("Given"))
            {
                expectedAttribute = typeof(GivenAttribute);
            }
            else if (method.Name.StartsWith("When"))
            {
                expectedAttribute = typeof(WhenAttribute);
            }
            else
            {
                throw new Exception("Unexpected method name. It should begin with Given or When");
            }

            Assert.NotNull(method.GetCustomAttribute(expectedAttribute));
        }

        public static IEnumerable<object[]> InvokeFactoryMethod
        {
            get
            {
                var verifyMethod = typeof(Mock<IActorFacade>).GetMethods()
                                                             .Single(m => m.Name == "Verify" &&
                                                                          m.IsGenericMethod &&
                                                                          m.GetParameters().Length == 1
                                                             );

                return new[] {
                            "Given",
                            "When"
                        }
                        .Select(name =>
                        {
                            var fixture = new Fixture().Customize(new DomainCustomization());
                            var actor = fixture.Freeze<Mock<IActorFacade>>();
                            var objectContainer = fixture.Freeze<Mock<IObjectContainer>>();
                            objectContainer.Setup(o => o.Resolve<IActorFacade>()).Returns(actor.Object);
                            var sut = fixture.Create<Add_Steps>();    
                            return new object[]
                            {
                                fixture,
                                actor,
                                new System.Action<string, object[]>((methodName, args) =>
                                {
                                    var sutMethodName = $"{name}_Add_{methodName}";
                                    var sutMethod = typeof(Add_Steps).GetMethod(sutMethodName);
                                    sutMethod.Invoke(sut, args);
                                }),
                                new System.Action<object>(expected =>
                                {
                                    var returnType = expected.GetType()
                                                             .GetInterfaces()
                                                             .Single(t => t.IsGenericType &&
                                                                          t.GetGenericTypeDefinition() == typeof(IAction<>)
                                                                   )
                                                             .GetGenericArguments()[0];
                                    invokeVerify(name, actor, returnType, expected);
                                })
                            };
                        });

                void invokeVerify(string actionName, Mock<IActorFacade> actor, Type returnType, object expected)
                {
                    var method = verifyMethod.MakeGenericMethod(returnType);
                    var verifyMethodExpression = typeof(StaticFactoriesTests).GetMethod($"Verify{actionName}Expression", BindingFlags.NonPublic | BindingFlags.Static)
                                                                             .MakeGenericMethod(returnType)
                                                                             .Invoke(null, new object[] { expected });
                    method.Invoke(actor, new object[] { verifyMethodExpression });
                }
            }
        }

#pragma warning disable IDE0051,S1144
        private static Expression<Func<IActorFacade, T>> VerifyGivenExpression<T>(IAction<T> expected)
        {
            return (IActorFacade actor) => actor.Given(expected);
        }

        private static Expression<Func<IActorFacade, T>> VerifyWhenExpression<T>(IAction<T> expected)
        {
            return (IActorFacade actor) => actor.When(expected);
        }
#pragma warning restore IDE0051,S1144

        [Theory, MemberData(nameof(InvokeFactoryMethod))]
        public void ParameterlessMethod_ShouldCallAction2(
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
            IFixture fixture,
            Mock<IActorFacade> actor,
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
            System.Action<string, object[]> act,
            System.Action<object> verify
            )
        {
            // arrange
            Add.ParameterlessMethodAction = Actions.Empty;
            // act
            act(nameof(Add.ParameterlessMethod), new object[] { });
            // assert
            verify(Actions.Empty);
        }
    }
}
