using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;

namespace Tranquire.Tests
{
    public class DomainAutoDataAttribute : AutoDataAttribute
    {
        public DomainAutoDataAttribute() : base(() => new Fixture().Customize(new DomainCustomization()))
        {
        }
    }

    public class DomainInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public DomainInlineAutoDataAttribute(params object[] args)
            : base(new DomainAutoDataAttribute(), args)
        {
        }
    }

    public class DomainCustomization : CompositeCustomization
    {
        public DomainCustomization()
            : base(
                 new ImmutableCollectionCustomization(),
                 new ActorCustomization(),
                 new AutoMoqCustomization() { ConfigureMembers = true })
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
