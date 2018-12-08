using AutoFixture;
using AutoFixture.AutoMoq;

namespace Tranquire.SpecFlow.Generation.Tests
{
    public class DomainCustomization : CompositeCustomization
    {
        public DomainCustomization()
            : base(
                 new AutoMoqCustomization() { ConfigureMembers = true })
        {
        }
    }
}
