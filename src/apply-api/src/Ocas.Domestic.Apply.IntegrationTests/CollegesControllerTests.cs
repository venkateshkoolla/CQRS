using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class CollegesControllerTests : BaseTest<ApplyApiClient>
    {
        private readonly IdentityUserFixture _identityUserFixture;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly Faker _fakerFixture;

        public CollegesControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _fakerFixture = XunitInjectionCollection.DataFakerFixture.Faker;
        }

        [Fact]
        [IntegrationTest]
        public async Task GetColleges_ShouldPass()
        {
            // Act
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            var result = await Client.GetColleges();

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task GetCollegeTemplate_ShouldPass()
        {
            // Arrange
            var collegeId = _fakerFixture.PickRandom(_modelFakerFixture.AllApplyLookups.Colleges).Id;
            var templateKey = _fakerFixture.PickRandom<Enums.CollegeTemplateKey>();

            // Act
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            var result = await Client.GetTemplate(collegeId, templateKey);

            // Assert
            result.Key.Should().BeAssignableTo<Enums.CollegeTemplateKey>();
            result.Content.Should().NotBeNullOrEmpty();
        }
    }
}
