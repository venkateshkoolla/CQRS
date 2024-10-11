using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public class ProgramChoicesControllerTests : BaseTest
    {
        private readonly IdentityUserFixture _identityUserFixture;
        private readonly AdminTestFramework.ModelFakerFixture _modelFakerFixture;
        private readonly DataFakerFixture _dataFakerFixture;

        public ProgramChoicesControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.DataFakerFixture, XunitInjectionCollection.IdentityUserFixture, XunitInjectionCollection.ModelFakerFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task Update_ProgramChoice_EffectiveDate_ShouldPass()
        {
            // Arrange
            var applicant = await ApplyClientFixture.CreateNewApplicant();
            var applicationBase = _modelFakerFixture.GetApplicationBase().Generate();
            applicationBase.ApplicantId = applicant.Id;
            var application = await ApplyApiClient.CreateApplication(applicationBase);

            await ApplyClientFixture.CreateProgramChoices(application, 1);

            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);
            var effectiveDateExpected = _dataFakerFixture.Faker.Date.Past().AsUtc().ToStringOrDefault();

            var programChoices = await ApplyApiClient.GetProgramChoices(application.Id);

            // Act
            var result = await Client.UpdateProgramChoiceEffectiveDate(programChoices[0].Id, new ProgramChoiceUpdateInfo { EffectiveDate = effectiveDateExpected });

            // Assert
            result.Should().NotBeNull();
            result.EffectiveDate.Should().Be(effectiveDateExpected);
        }
    }
}
