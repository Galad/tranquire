using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Moq;
using System;
using Tranquire;
using Tranquire.Tests;
using Xunit;
using FluentAssertions;
using System.Linq;

namespace Tranquire.NUnit.Tests
{
    public class VerifiesExtensionsTests
    {
        [Fact]
        public void Sut_VerifyGuardClause()
        {
            new Actor("aa").Test();
        }
    }
}
