using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Moq;
using Ploeh.AutoFixture.Kernel;

namespace Tranquire.Tests
{
    public class DomainAutoDataAttribute : AutoDataAttribute
    {
        public DomainAutoDataAttribute() : base(new Fixture().Customize(new AutoConfiguredMoqCustomization()).Customize(new DomainCustomization()))
        {
        }
    }

    public class DomainCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new ActorCustomization());
        }

        private class ActorCustomization : ISpecimenBuilder
        {
            public object Create(object request, ISpecimenContext context)
            {
                var type = request as Type;
                if(type == null || !typeof(IActor).IsAssignableFrom(type))
                {
                    return new NoSpecimen();
                }
                return new Actor("John");
            }
        }
    }
}
