using System;
using FluentAssertions;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Core.Extensions.UnitTests
{
    public class GuidExtensionsTests
    {
        [Fact]
        [UnitTest("Extension")]
        public void IsEmpty_ShouldPass_With_Guid()
        {
            var guid = Guid.NewGuid();
            guid.IsEmpty().Should().BeFalse();
        }

        [Fact]
        [UnitTest("Extension")]
        public void IsEmpty_ShouldPass_With_EmptyGuid()
        {
            var guid = Guid.Empty;
            guid.IsEmpty().Should().BeTrue();
        }

        [Fact]
        [UnitTest("Extension")]
        public void IsEmpty_ShouldPass_With_NullGuid()
        {
            Guid? guid = null;
            guid.IsEmpty().Should().BeTrue();
        }
    }
}
