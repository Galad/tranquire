using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using FluentAssertions;

namespace Tranquire.Tests
{
    public class ActorDecoratorExtensionAssertion : IdiomaticAssertion
    {
        public ActorDecoratorExtensionAssertion(ISpecimenBuilder specimenBuilder)
        {
            SpecimenBuilder = specimenBuilder ?? throw new ArgumentNullException(nameof(specimenBuilder));
        }

        public ISpecimenBuilder SpecimenBuilder { get; }

        public override void Verify(MethodInfo methodInfo)
        {
            if (!methodInfo.IsStatic)
            {
                return;
            }
            var parameters = methodInfo.GetParameters();
            if (methodInfo.ReturnType != typeof(Actor) || parameters.Length == 0 || parameters[0].ParameterType != typeof(Actor))
            {
                return;
            }

            var values = parameters.Select(p => p.IsOut ? null : new SpecimenContext(SpecimenBuilder).Resolve(p))
                                   .ToArray();
            var expected = (Actor)values[0];
            var actual = (Actor)methodInfo.Invoke(null, values);
            actual.Should().BeEquivalentTo(expected, o => o.Excluding(a => a.InnerActorBuilder), "The method {0} did not return a correct actor", methodInfo.Name);
            base.Verify(methodInfo);
        }
    }
}
