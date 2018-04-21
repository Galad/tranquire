using FluentAssertions;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Linq;
using System.Reflection;

namespace Tranquire.Tests
{
    public class ActorDecoratorExtensionAssertion : IdiomaticAssertion
    {
        public ActorDecoratorExtensionAssertion(ISpecimenBuilder specimenBuilder)
        {
            if (specimenBuilder == null)
            {
                throw new ArgumentNullException(nameof(specimenBuilder));
            }

            SpecimenBuilder = specimenBuilder;
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

            var values = parameters.Select(p => new SpecimenContext(SpecimenBuilder).Resolve(p)).ToArray();
            var expected = (Actor)values[0];
            var actual = (Actor)methodInfo.Invoke(null, values);
            actual.ShouldBeEquivalentTo(expected, o => o.Excluding(a => a.InnerActorBuilder), "The method {0} did not return a correct actor", methodInfo.Name);
            base.Verify(methodInfo);
        }
    }
}
