using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Core.Extensions.UnitTests
{
    public class LinqExtensionsTests
    {
        [Fact]
        [UnitTest("Extension")]
        public void AllEqual_ShouldPass_When_Empty()
        {
            var list = Enumerable.Empty<int>();
            list.AllEqual().Should().BeTrue();
        }

        [Fact]
        [UnitTest("Extension")]
        public void AllEqual_ShouldPass_When_Equal()
        {
            var list = new List<int> { 1, 1, 1, 1 };
            list.AllEqual().Should().BeTrue();
        }

        [Fact]
        [UnitTest("Extension")]
        public void AllEqual_ShouldPass_When_NoneEqual()
        {
            var list = new List<int> { 1, 2, 3, 4 };
            list.AllEqual().Should().BeFalse();
        }

        [Fact]
        [UnitTest("Extension")]
        public void AllEqual_ShouldPass_When_NotEqual()
        {
            var list = new List<int> { 1, 1, 2, 2 };
            list.AllEqual().Should().BeFalse();
        }

        [Fact]
        [UnitTest("Extension")]
        public void AllUnique_ShouldPass_When_Empty()
        {
            var list = Enumerable.Empty<int>();
            list.AllUnique().Should().BeTrue();
        }

        [Fact]
        [UnitTest("Extension")]
        public void AllUnique_ShouldPass_When_Equal()
        {
            var list = new List<int> { 1, 1, 1, 1 };
            list.AllUnique().Should().BeFalse();
        }

        [Fact]
        [UnitTest("Extension")]
        public void AllUnique_ShouldPass_When_NoneEqual()
        {
            var list = new List<int> { 1, 2, 3, 4 };
            list.AllUnique().Should().BeTrue();
        }

        [Fact]
        [UnitTest("Extension")]
        public void AllUnique_ShouldPass_When_NotEqual()
        {
            var list = new List<int> { 1, 1, 2, 2 };
            list.AllUnique().Should().BeFalse();
        }
    }
}
