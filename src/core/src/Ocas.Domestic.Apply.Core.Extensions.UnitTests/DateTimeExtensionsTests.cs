using System;
using FluentAssertions;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Core.Extensions.UnitTests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        [UnitTest("Extension")]
        public void TotalMonths_Should_Pass_When_Date_Null()
        {
            // Arrange
            var date = (DateTime?)null;

            // Act
            var result = date.TotalMonths();

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        [UnitTest("Extension")]
        public void TotalMonths_Should_Pass_When_Date_UtcNow()
        {
            // Arrange & Act
            var result = DateTime.UtcNow.TotalMonths();

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        [UnitTest("Extension")]
        public void TotalMonths_Should_Pass_When_Date_In_Past()
        {
            // Arrange
            const int expectedMonths = -9;

            // Act
            var result = DateTime.UtcNow.AddMonths(expectedMonths).TotalMonths();

            // Assert
            result.Should().Be(expectedMonths);
        }

        [Fact]
        [UnitTest("Extension")]
        public void TotalMonths_Should_Pass_When_Date_In_Future()
        {
            // Arrange
            const int expectedMonths = 9;

            // Act
            var result = DateTime.UtcNow.AddMonths(expectedMonths).TotalMonths();

            // Assert
            result.Should().Be(expectedMonths);
        }
    }
}
