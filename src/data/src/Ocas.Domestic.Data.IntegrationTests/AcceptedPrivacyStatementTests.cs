using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Data.TestFramework.Extensions;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class AcceptedPrivacyStatementTests : BaseTest
    {
        [Fact]
        public async Task AddAcceptedPrivacyStatement_ShouldPass()
        {
            var estToday = DateTime.UtcNow.ToDateInEstAsUtc();

            Contact entityBefore = null;
            try
            {
                // Arrange
                var model = DataFakerFixture.Models.ContactBase.Generate();
                model.AcceptedPrivacyStatementId = null;
                entityBefore = await Context.CreateContact(model);
                var privacyStatement = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.PrivacyStatements);
                entityBefore.AcceptedPrivacyStatementId = privacyStatement.Id;

                // Act
                var entityAfter = await Context.UpdateContact(entityBefore);
                var acceptedPrivacyStatement = await Context.AddAcceptedPrivacyStatement(entityBefore, privacyStatement, estToday);

                // Assert
                acceptedPrivacyStatement.AcceptedDate.Should().Be(estToday);
                acceptedPrivacyStatement.Name.Should().Be(privacyStatement.Name);
                acceptedPrivacyStatement.PrivacyStatementId.Should().Be(privacyStatement.Id);
                acceptedPrivacyStatement.ContactId.Should().Be(entityBefore.Id);
            }
            finally
            {
                // Cleanup
                if (entityBefore?.Id != null)
                    await Context.DeleteContact(entityBefore.Id);
            }
        }
    }
}
