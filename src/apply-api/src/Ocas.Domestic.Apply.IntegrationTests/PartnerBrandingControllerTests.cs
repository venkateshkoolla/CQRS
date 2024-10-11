using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class PartnerBrandingControllerTests : BaseTest<ApplyApiClient>
    {
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly Faker _fakerFixture;

        public PartnerBrandingControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _fakerFixture = XunitInjectionCollection.DataFakerFixture.Faker;
        }

        [Fact]
        [IntegrationTest]
        public async Task GetPartnerBranding_ShouldPass_When_CollegeBranding()
        {
            // Arrange
            var code = _fakerFixture.PickRandom(_modelFakerFixture.AllApplyLookups.Colleges.Where(c => c.AllowCba)).Code;

            // Act
            var result = await Client.GetPartnerBranding(code.ToLowerInvariant());

            // Assert
            result.Partner.Should().Be(code.ToUpperInvariant());
            result.Type.Should().Be(PartnerBrandingType.College);
        }

        [Fact]
        [IntegrationTest]
        public void GetPartnerBranding_ShouldThrow_When_CollegeBranding_NotAllowed()
        {
            // Arrange
            var code = _fakerFixture.PickRandom(_modelFakerFixture.AllApplyLookups.Colleges.Where(c => !c.AllowCba)).Code;

            // Act
            Func<Task> action = () => Client.GetPartnerBranding(code);

            // Assert
            var result = action.Should().Throw<StatusCodeException>();
            result.Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Which.OcasHttpResponseMessage.Should().NotBeNull();
            result.Which.OcasHttpResponseMessage.Errors.Should().NotBeEmpty()
                .And.Contain(x => x.Message == $"Partner branding for code '{code}' not found.");
        }

        [Fact]
        [IntegrationTest]
        public async Task GetPartnerBranding_ShouldPass_When_PartnerBranding()
        {
            // Arrange
            var code = _fakerFixture.PickRandom(_modelFakerFixture.AllApplyLookups.ReferralPartners.Where(c => c.AllowCba)).Code;

            // Act
            var result = await Client.GetPartnerBranding(code.ToLowerInvariant());

            // Assert
            result.Partner.Should().Be(code.ToUpperInvariant());
            result.Type.Should().Be(PartnerBrandingType.Referral);
        }

        [Fact]
        [IntegrationTest]
        public void GetPartnerBranding_ShouldThrow_When_PartnerBranding_NotAllowed()
        {
            // Arrange
            var code = _fakerFixture.PickRandom(_modelFakerFixture.AllApplyLookups.ReferralPartners.Where(c => !c.AllowCba)).Code;

            // Act
            Func<Task> action = () => Client.GetPartnerBranding(code);

            // Assert
            var result = action.Should().Throw<StatusCodeException>();
            result.Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Which.OcasHttpResponseMessage.Should().NotBeNull();
            result.Which.OcasHttpResponseMessage.Errors.Should().NotBeEmpty()
                .And.Contain(x => x.Message == $"Partner branding for code '{code}' not found.");
        }

        [Fact]
        [IntegrationTest]
        public void GetPartnerBranding_ShouldThrow_When_Code_NotFound()
        {
            // Arrange
            const string code = "ASDF";

            // Act
            Func<Task> action = () => Client.GetPartnerBranding(code);

            // Assert
            var result = action.Should().Throw<StatusCodeException>();
            result.Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Which.OcasHttpResponseMessage.Should().NotBeNull();
            result.Which.OcasHttpResponseMessage.Errors.Should().NotBeEmpty()
                .And.Contain(x => x.Message == $"Partner branding for code '{code}' not found.");
        }
    }
}
