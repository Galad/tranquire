using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Tranquire.Tests
{
    public sealed class ImmutableCollectionCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new ImmutableCollectionSpecimenBuilder(typeof(ImmutableArray), typeof(ImmutableArray<>)));
            fixture.Customizations.Add(new ImmutableCollectionSpecimenBuilder(typeof(ImmutableHashSet), typeof(ImmutableHashSet<>)));
        }
    }

    public sealed class ImmutableCollectionSpecimenBuilder : ISpecimenBuilder
    {
        private readonly MethodInfo CreateValueMethod;
        private readonly Type _immutableCollectionGenericTypeDefinition;

        public ImmutableCollectionSpecimenBuilder(
            Type immutableCollectionFactoryType,
            Type immutableCollectionGenericTypeDefinition)
        {
            CreateValueMethod = immutableCollectionFactoryType.GetMethods(BindingFlags.Static | BindingFlags.Public)
                                                              .Single(m => m.Name == "Create" &&
                                                                       m.IsGenericMethod &&
                                                                       m.GetGenericArguments().Length == 1 &&
                                                                       m.GetParameters().Length == 1 &&
                                                                       m.GetParameters()[0].ParameterType.IsArray);
            _immutableCollectionGenericTypeDefinition = immutableCollectionGenericTypeDefinition;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var requestType = request as Type;
            if (requestType == null || !requestType.IsGenericType || requestType.GetGenericTypeDefinition() != _immutableCollectionGenericTypeDefinition)
            {
                return new NoSpecimen();
            }
            var itemType = requestType.GetGenericArguments()[0];
            var itemTypeArray = itemType.MakeArrayType();
            var items = context.Resolve(itemTypeArray);
            var value = CreateValueMethod.MakeGenericMethod(itemType).Invoke(null, new object[] { items });
            return value;
        }
    }
}
