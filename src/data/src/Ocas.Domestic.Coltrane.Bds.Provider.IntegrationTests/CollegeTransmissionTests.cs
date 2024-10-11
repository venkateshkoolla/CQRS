using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Coltrane.Bds.Provider.IntegrationTests
{
    public class CollegeTransmissionTests : BaseTest
    {
        [Fact]
        public async Task GetCollegeTransmissions_ShouldPass()
        {
            // Arrange
            const string applicationNumber = TestConstants.CollegeTrasmission.ApplicationNumber;

            // Act
            var result = await Provider.GetCollegeTransmissions(applicationNumber);

            // Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetCollegeTransmissions_ShouldPass_When_Empty()
        {
            // Arrange & Act
            var result = await Provider.GetCollegeTransmissions(string.Empty);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCollegeTransmissions_ShouldPass_When_Options_Available()
        {
            // Arrange
            const string applicationNumber = TestConstants.CollegeTrasmission.ApplicationNumber;
            var options = new GetCollegeTransmissionHistoryOptions
            {
                FromDate = new DateTime(2019, 10, 09),
                ToDate = new DateTime(2019, 10, 10),
                TransactionCode = TestConstants.CollegeTransmissionCodes.Grade,
                TransactionType = TestConstants.CollegeTrasmission.TransactionType
            };

            // Act
            var result = await Provider.GetCollegeTransmissions(applicationNumber, options);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }
    }
}
