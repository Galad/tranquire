using AutoFixture;
using AutoFixture.Xunit2;

namespace Tranquire.SpecFlow.Generation.Tests
{
    public class DomainAutoDataAttribute : AutoDataAttribute
    {
        public DomainAutoDataAttribute() : base(() => new Fixture().Customize(new DomainCustomization()))
        {
        }
    }
}
