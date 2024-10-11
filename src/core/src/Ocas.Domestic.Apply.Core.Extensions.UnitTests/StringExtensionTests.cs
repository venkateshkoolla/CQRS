using FluentAssertions;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Core.Extensions.UnitTests
{
    public class StringExtensionTests
    {
        [Theory]
        [UnitTest("Extension")]
        [InlineData("Foo", "foo")]
        [InlineData("foo", "foo")]
        [InlineData("FooBar", "foo-bar")]
        [InlineData("fooBar", "foo-bar")]
        [InlineData("FooBarBaz", "foo-bar-baz")]
        [InlineData("fooBarBaz", "foo-bar-baz")]
        [InlineData("FooBar-Baz", "foo-bar-baz")]
        [InlineData("Foo-Bar-Baz", "foo-bar-baz")]
        [InlineData("foo-bar-baz", "foo-bar-baz")]
        public void ToKebabCase_ShouldPass(string original, string expected)
        {
            var skewered = original.ToKebabCase();
            skewered.Should().Be(expected);
        }

        [Theory]
        [UnitTest("Extension")]
        [InlineData("Foo", "foo")]
        [InlineData("foo", "foo")]
        [InlineData("FooBar", "fooBar")]
        [InlineData("fooBar", "fooBar")]
        [InlineData("FooBarBaz", "fooBarBaz")]
        [InlineData("fooBarBaz", "fooBarBaz")]
        public void ToCamelCase_ShouldPass(string original, string expected)
        {
            var humped = original.ToCamelCase();
            humped.Should().Be(expected);
        }
    }
}
