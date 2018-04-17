using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tranquire.Tests
{
    public class DomainAutoDataAttribute : AutoDataAttribute
    {
        public DomainAutoDataAttribute() : base(new Fixture().Customize(new DomainCustomization()))
        {
        }
    }

    public class DomainInlineAutoDataAttribute : CompositeDataAttribute
    {
        public DomainInlineAutoDataAttribute(params object[] args)
            : base(new InlineAutoDataAttribute(args), new AutoDataAttribute(new Fixture().Customize(new DomainCustomization())))
        {
        }
    }

    public class DomainCustomization : CompositeCustomization
    {
        public DomainCustomization()
            : base(
                 new ImmutableCollectionCustomization(),
                 new ActorCustomization(),
                 new AutoConfiguredMoqCustomization())
        {
        }
    }

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
            fixture.Register<Func<IActor, IActor>>(() => a => a);
            new ConstructorCustomization(typeof(Actor), new ActorConstructorQuery()).Customize(fixture);
        }

        private class ActorConstructorQuery : IMethodQuery
        {
            public IEnumerable<IMethod> SelectMethods(Type type)
            {
                yield return new ConstructorMethod(
                    type.GetConstructor(
                        new[] {
                                typeof(string),
                                typeof(IReadOnlyDictionary<Type, object>)
                        })
                    );
            }
        }

        private class ConstructorMethod : IMethod
        {
            private readonly ConstructorInfo _constructor;

            public ConstructorMethod(ConstructorInfo constructor)
            {
                _constructor = constructor;
            }

            public IEnumerable<ParameterInfo> Parameters
            {
                get
                {
                    return _constructor.GetParameters();
                }
            }

            public object Invoke(IEnumerable<object> parameters)
            {
                return _constructor.Invoke(parameters.ToArray());
            }
        }
    }
}
