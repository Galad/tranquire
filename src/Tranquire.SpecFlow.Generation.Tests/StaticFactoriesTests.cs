using AutoFixture;
using BoDi;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace Tranquire.SpecFlow.Generation.Tests
{    
    public class StaticFactoriesTests
    {
        [Fact]
        public async Task GeneratedStepsMethods_HaveCorrectAttribute()
        {
            var assembly = await CompilationTests.CompileSource(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Add.cs")));
            var type = assembly.GetType("Tranquire.SpecFlow.Generation.Tests.Add_Steps");
            Assert.NotNull(type);
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                              .Where(m => m.Name.StartsWith("Given") || m.Name.StartsWith("When"))
                              .Select(m => (m, attribute: m.Name.StartsWith("Given") ? typeof(GivenAttribute) : typeof(WhenAttribute)))
                              .Select(m => (m.m.Name, attribute: m.m.GetCustomAttribute(m.attribute)))
                              .ToArray();

            Assert.All(methods, m => Assert.NotNull(m.attribute));
        }

        public static IEnumerable<object[]> InvokeFactoryMethod
        {
            get
            {
                MethodInfo verifyMethod = typeof(Mock<IActorFacade>).GetMethods()
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
                            IFixture fixture = new Fixture().Customize(new DomainCustomization());
                            Mock<IActorFacade> actor = fixture.Freeze<Mock<IActorFacade>>();
                            Mock<IObjectContainer> objectContainer = fixture.Freeze<Mock<IObjectContainer>>();
                            objectContainer.Setup(o => o.Resolve<IActorFacade>()).Returns(actor.Object);
                            Add_Steps sut = fixture.Create<Add_Steps>();
                            return new object[]
                            {
                                new System.Action<string, object[]>((methodName, args) =>
                                {
                                    string sutMethodName = $"{name}_Add_{methodName}";
                                    MethodInfo sutMethod = typeof(Add_Steps).GetMethod(sutMethodName);
                                    Assert.True(sutMethod != null, $"Expected method {sutMethodName} to be generated.");
                                    sutMethod.Invoke(sut, args);
                                }),
                                new System.Action<object>(expected =>
                                {
                                    Type returnType = expected.GetType()
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
                    MethodInfo method = verifyMethod.MakeGenericMethod(returnType);
                    object verifyMethodExpression = typeof(StaticFactoriesTests).GetMethod($"Verify{actionName}Expression", BindingFlags.NonPublic | BindingFlags.Static)
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
        public void ParameterlessMethod_ShouldCallAction(
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

        [Theory, MemberData(nameof(InvokeFactoryMethod))]
        public void MethodWithOneParameter_ShouldCallAction(
            System.Action<string, object[]> act,
            System.Action<object> verify
            )
        {
            // arrange
            var actionFactory = new Mock<Func<string, IAction<Unit>>>();
            var expected = Actions.Create("test", a => { });
            var parameter = Guid.NewGuid().ToString();
            actionFactory.Setup(f => f(parameter)).Returns(expected);
            Add.MethodWithOneParameterAction = actionFactory.Object;
            // act
            act(nameof(Add.MethodWithOneParameter), new[] { parameter });
            // assert
            verify(expected);
        }
    }
}
