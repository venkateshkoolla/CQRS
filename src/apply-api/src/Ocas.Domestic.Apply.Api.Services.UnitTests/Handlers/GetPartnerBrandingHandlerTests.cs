using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class GetPartnerBrandingHandlerTests
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupCache;
        private readonly ILogger<GetPartnerBrandingHandler> _logger;
        private readonly ModelFakerFixture _models;

        public GetPartnerBrandingHandlerTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupCache = XunitInjectionCollection.LookupsCache;
            _logger = Mock.Of<ILogger<GetPartnerBrandingHandler>>();
            _models = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetPartnerBrandingHandler_ShouldPass_WhenCollegeBranding()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.Colleges.Where(c => c.AllowCba));
            var request = new GetPartnerBranding
            {
                Code = college.Code
            };

            var handler = new GetPartnerBrandingHandler(_logger, _lookupCache);

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Partner.Should().Be(college.Code);
            response.Type.Should().Be(PartnerBrandingType.College);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetPartnerBrandingHandler_ShouldThrow_WhenCollegeBranding_DoesNotAllowCba()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.Colleges.Where(c => !c.AllowCba));
            var request = new GetPartnerBranding
            {
                Code = college.Code
            };

            var handler = new GetPartnerBrandingHandler(_logger, _lookupCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .And.Message.Should().Be($"Partner branding for code '{request.Code}' not found.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetPartnerBrandingHandler_ShouldPass_WhenPartnerBranding()
        {
            // Arrange
            var referralPartner = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.ReferralPartners.Where(c => c.AllowCba));
            var request = new GetPartnerBranding
            {
                Code = referralPartner.Code
            };

            var handler = new GetPartnerBrandingHandler(_logger, _lookupCache);

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Partner.Should().Be(referralPartner.Code);
            response.Type.Should().Be(PartnerBrandingType.Referral);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetPartnerBrandingHandler_ShouldThrow_WhenPartnerBranding_DoesNotAllowCba()
        {
            // Arrange
            var referralPartner = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.ReferralPartners.Where(c => !c.AllowCba));
            var request = new GetPartnerBranding
            {
                Code = referralPartner.Code
            };

            var handler = new GetPartnerBrandingHandler(_logger, _lookupCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .And.Message.Should().Be($"Partner branding for code '{request.Code}' not found.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetPartnerBrandingHandler_ShouldThrow_WhenCode_DoesNot_Exist()
        {
            // Arrange
            var request = new GetPartnerBranding
            {
                Code = "ASDF"
            };

            var handler = new GetPartnerBrandingHandler(_logger, _lookupCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .And.Message.Should().Be($"Partner branding for code '{request.Code}' not found.");
        }
    }
}
