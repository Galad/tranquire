using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Xunit;

namespace Tranquire.Tests
{
    public class ActionTagsTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion, IFixture fixture)
        {
            fixture.Register(() => ImmutableDictionary.Create<string, int>());
            assertion.Verify(typeof(ActionTags));
            assertion.Verify(typeof(ActionTags<string>));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorInitializedMembers(ConstructorInitializedMemberAssertion assertion, IFixture fixture)
        {
            fixture.Register(() => ImmutableDictionary.Create<string, int>());
            assertion.Verify(typeof(ActionTags<string>));
        }

        [Theory]
        [DomainInlineAutoData(new[] { "tag1", "tag2", "tag3" }, "tag1")]
        [DomainInlineAutoData(new[] { "tag1", "tag4", "tag5" }, "tag1")]
        [DomainInlineAutoData(new[] { "tag3", "tag2", "tag1" }, "tag1")]
        [DomainInlineAutoData(new[] { "tag2", "tag3", "tag4" }, "tag2")]
        [DomainInlineAutoData(new[] { "tag3", "tag4", "tag5" }, "tag3")]
        public void FindBestWhenTag_ShouldReturnCorrectValue(string[] tags, string expected)
        {
            // arrange       
            var whenTags = ImmutableDictionary.Create<string, int>()
                                          .Add("tag1", 1)
                                          .Add("tag2", 2)
                                          .Add("tag3", 3);
            var sut = new ActionTags<string>(whenTags, ImmutableDictionary<string, int>.Empty);
            // act            
            var actual = sut.FindBestWhenTag(tags);
            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [DomainInlineAutoData(new[] { "tag1", "tag2", "tag3" }, "tag1")]
        [DomainInlineAutoData(new[] { "tag1", "tag4", "tag5" }, "tag1")]
        [DomainInlineAutoData(new[] { "tag3", "tag2", "tag1" }, "tag1")]
        [DomainInlineAutoData(new[] { "tag2", "tag3", "tag4" }, "tag2")]
        [DomainInlineAutoData(new[] { "tag3", "tag4", "tag5" }, "tag3")]
        public void FindBestGivenTag_ShouldReturnCorrectValue(string[] tags, string expected)
        {
            // arrange       
            var givenTags = ImmutableDictionary.Create<string, int>()
                                               .Add("tag1", 1)
                                               .Add("tag2", 2)
                                               .Add("tag3", 3);
            var sut = new ActionTags<string>(ImmutableDictionary<string, int>.Empty, givenTags);
            // act            
            var actual = sut.FindBestGivenTag(tags);
            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FindBestWhenTag_WhenTagIsNotFound_ShouldThrow()
        {
            // arrange       
            var whenTags = ImmutableDictionary.Create<string, int>()
                                               .Add("tag1", 1)
                                               .Add("tag2", 2)
                                               .Add("tag3", 3);
            var sut = new ActionTags<string>(whenTags, ImmutableDictionary<string, int>.Empty);
            // act and assert
            Assert.Throws<KeyNotFoundException>(() => sut.FindBestGivenTag(new[] { "tag4" }));
        }

        [Fact]
        public void FindBestGivenTag_WhenTagIsNotFound_ShouldThrow()
        {
            // arrange       
            var givenTags = ImmutableDictionary.Create<string, int>()
                                               .Add("tag1", 1)
                                               .Add("tag2", 2)
                                               .Add("tag3", 3);
            var sut = new ActionTags<string>(ImmutableDictionary<string, int>.Empty, givenTags);
            // act and assert
            Assert.Throws<KeyNotFoundException>(() => sut.FindBestGivenTag(new[] { "tag4" }));
        }

        [Fact]
        public void AddFirstPriorityGivenTag_ShouldAddTheTagWithTheHighestPriority()
        {
            // arrange
            var givenTags = ImmutableDictionary.Create<string, int>()
                                               .Add("tag1", -1)
                                               .Add("tag2", 0)
                                               .Add("tag3", 1);
            var sut = new ActionTags<string>(ImmutableDictionary<string, int>.Empty, givenTags);
            // act
            var actual = sut.AddFirstPriorityGivenTag("tag4").GivenTags.OrderBy(k => k.Value);
            // assert
            var expected = givenTags.Add("tag4", -2).OrderBy(k => k.Value);
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void AddFirstPriorityWhenTag_ShouldAddTheTagWithTheHighestPriority()
        {
            // arrange
            var whenTags = ImmutableDictionary.Create<string, int>()
                                               .Add("tag1", -1)
                                               .Add("tag2", 0)
                                               .Add("tag3", 1);
            var sut = new ActionTags<string>(whenTags, ImmutableDictionary<string, int>.Empty);
            // act
            var actual = sut.AddFirstPriorityWhenTag("tag4").WhenTags.OrderBy(k => k.Value);
            // assert
            var expected = whenTags.Add("tag4", -2).OrderBy(k => k.Value);
            Assert.Equal(actual, expected);
        }

        #region Static factory
        [Fact(DisplayName = "Create should return a ActionTags instance where the Given tags have the inverse priority that the when tags")]
        public void Create_ShouldReturnActionTagsWitCorrectPriority()
        {
            // act
            var actual = ActionTags.Create("tag1", "tag2", "tag3");
            // assert
            var expectedWhenTags = new Dictionary<string, int>()
            {
                { "tag1", 0 },
                { "tag2", 1 },
                { "tag3", 2 },
            }.OrderBy(k => k.Key);
            var expectedGivenTags = new Dictionary<string, int>()
            {
                { "tag1", 2 },
                { "tag2", 1 },
                { "tag3", 0 },
            }.OrderBy(k => k.Key);
            Assert.Equal(actual.WhenTags.OrderBy(k => k.Key), expectedWhenTags);
            Assert.Equal(actual.GivenTags.OrderBy(k => k.Key), expectedGivenTags);
        }
        #endregion
    }
}
