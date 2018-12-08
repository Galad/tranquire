using AutoFixture.Xunit2;

namespace Tranquire.SpecFlow.Generation.Tests
{
    public class DomainInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public DomainInlineAutoDataAttribute(params object[] args)
            : base(new DomainAutoDataAttribute(), args)
        {
        }
    }
}
