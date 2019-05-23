using AutoFixture;
using AutoFixture.Idioms;
using Moq;
using System;
using Tranquire.Reporting;
using Xunit;

namespace Tranquire.Tests.Reporting
{
    public class ActionFileAttachmentTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ActionFileAttachment));
        }

        [Theory, DomainAutoData]
        public void Sut_ConstructorInitializedMembers(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(ActionFileAttachment));
        }
    }
}
