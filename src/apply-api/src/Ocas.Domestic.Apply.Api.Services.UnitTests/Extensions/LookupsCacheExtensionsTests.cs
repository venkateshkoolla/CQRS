using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Services.Extensions;
using Ocas.Domestic.Apply.Core;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Extensions
{
    public class LookupsCacheExtensionsTests
    {
        private readonly Faker _faker;
        private readonly ILookupsCache _lookupsCache;

        public LookupsCacheExtensionsTests()
        {
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
        }

        [Fact]
        [UnitTest("Extensions")]
        public async Task GetSourceId_ShouldPass_When_EmptyPartnerCode()
        {
            // Arrange
            var sources = await _lookupsCache.GetSources(Constants.Localization.FallbackLocalization);
            var expectedSourceId = sources.First(x => x.Code == Constants.Sources.A2C2).Id;

            // Act
            var sourceId = await _lookupsCache.GetSourceId(string.Empty);

            // Assert
            sourceId.Should().Be(expectedSourceId);
        }

        [Fact]
        [UnitTest("Extensions")]
        public async Task GetSourceId_ShouldPass_When_College_AllowedCba_And_AllowCbaReferralCodeAsSource()
        {
            // Arrange
            var sources = await _lookupsCache.GetSources(Constants.Localization.FallbackLocalization);
            var expectedSourceId = sources.First(s => s.Code == Constants.Sources.CBUIUNKNOWN).Id;

            var colleges = await _lookupsCache.GetColleges(Constants.Localization.FallbackLocalization);
            var collegeCode = _faker.PickRandom(colleges.Where(c => c.AllowCba && c.AllowCbaReferralCodeAsSource)).Code;

            // Act
            var sourceId = await _lookupsCache.GetSourceId(collegeCode);

            // Assert
            sourceId.Should().Be(expectedSourceId);
        }

        [Fact]
        [UnitTest("Extensions")]
        public async Task GetSourceId_ShouldPass_When_College_AllowedCba_And_NotAllowCbaReferralCodeAsSource()
        {
            // Arrange
            var sources = await _lookupsCache.GetSources(Constants.Localization.FallbackLocalization);
            var expectedSourceId = sources.First(s => s.Code == Constants.Sources.CBUI).Id;

            var colleges = await _lookupsCache.GetColleges(Constants.Localization.FallbackLocalization);
            var collegeCode = _faker.PickRandom(colleges.Where(c => c.AllowCba && !c.AllowCbaReferralCodeAsSource)).Code;

            // Act
            var sourceId = await _lookupsCache.GetSourceId(collegeCode);

            // Assert
            sourceId.Should().Be(expectedSourceId);
        }

        [Fact]
        [UnitTest("Extensions")]
        public async Task GetSourceId_ShouldPass_When_College_NotAllowedCba()
        {
            // Arrange
            var sources = await _lookupsCache.GetSources(Constants.Localization.FallbackLocalization);
            var expectedSourceId = sources.First(s => s.Code == Constants.Sources.A2C2).Id;

            var colleges = await _lookupsCache.GetColleges(Constants.Localization.FallbackLocalization);
            var collegeCode = _faker.PickRandom(colleges.Where(c => !c.AllowCba)).Code;

            // Act
            var sourceId = await _lookupsCache.GetSourceId(collegeCode);

            // Assert
            sourceId.Should().Be(expectedSourceId);
        }

        [Fact]
        [UnitTest("Extensions")]
        public async Task GetSourceId_ShouldPass_When_ReferralPartner_AllowedCba_And_AllowCbaReferralCodeAsSource()
        {
            // Arrange
            var referralPartners = await _lookupsCache.GetReferralPartners();
            var referralPartnerCode = _faker.PickRandom(referralPartners.Where(c => c.AllowCba && c.AllowCbaReferralCodeAsSource)).Code;

            var sources = await _lookupsCache.GetSources(Constants.Localization.FallbackLocalization);
            var expectedSourceId = sources.First(s => s.Code == Constants.Sources.CBUI + referralPartnerCode).Id;

            // Act
            var sourceId = await _lookupsCache.GetSourceId(referralPartnerCode);

            // Assert
            sourceId.Should().Be(expectedSourceId);
        }

        [Fact]
        [UnitTest("Extensions")]
        public async Task GetSourceId_ShouldPass_When_ReferralPartner_AllowedCba_And_NotAllowCbaReferralCodeAsSource()
        {
            // Arrange
            var sources = await _lookupsCache.GetSources(Constants.Localization.FallbackLocalization);
            var expectedSourceId = sources.First(s => s.Code == Constants.Sources.CBUI).Id;

            var referralPartners = await _lookupsCache.GetReferralPartners();
            var referralPartnerCode = _faker.PickRandom(referralPartners.Where(c => c.AllowCba && !c.AllowCbaReferralCodeAsSource)).Code;

            // Act
            var sourceId = await _lookupsCache.GetSourceId(referralPartnerCode);

            // Assert
            sourceId.Should().Be(expectedSourceId);
        }

        [Fact]
        [UnitTest("Extensions")]
        public async Task GetSourceId_ShouldPass_When_ReferralPartner_NotAllowedCba()
        {
            // Arrange
            var sources = await _lookupsCache.GetSources(Constants.Localization.FallbackLocalization);
            var expectedSourceId = sources.First(s => s.Code == Constants.Sources.A2C2).Id;

            var referralPartners = await _lookupsCache.GetReferralPartners();
            var referralPartnerCode = _faker.PickRandom(referralPartners.Where(c => !c.AllowCba)).Code;

            // Act
            var sourceId = await _lookupsCache.GetSourceId(referralPartnerCode);

            // Assert
            sourceId.Should().Be(expectedSourceId);
        }

        [Fact]
        [UnitTest("Extensions")]
        public async Task GetSourcePartnerId_ShouldPass_When_EmptyPartnerCode()
        {
            // Arrange
            var sources = await _lookupsCache.GetSources(Constants.Localization.FallbackLocalization);
            var expectedSourceId = sources.First(x => x.Code == Constants.Sources.A2C2).Id;

            // Act
            var(sourceId, sourcePartnerId) = await _lookupsCache.GetSourcePartnerId(string.Empty);

            // Assert
            sourceId.Should().Be(expectedSourceId);
            sourcePartnerId.Should().BeNull();
        }

        [Fact]
        [UnitTest("Extensions")]
        public async Task GetSourcePartnerId_ShouldPass_When_College_AllowedCba()
        {
            // Arrange
            var colleges = await _lookupsCache.GetColleges(Constants.Localization.FallbackLocalization);
            var college = _faker.PickRandom(colleges.Where(c => c.AllowCba));

            // Act
            var(sourceId, sourcePartnerId) = await _lookupsCache.GetSourcePartnerId(college.Code);

            // Assert
            sourcePartnerId.Should().Be(college.Id);
        }

        [Fact]
        [UnitTest("Extensions")]
        public async Task GetSourcePartnerId_ShouldPass_When_College_NotAllowedCba()
        {
            // Arrange
            var colleges = await _lookupsCache.GetColleges(Constants.Localization.FallbackLocalization);
            var college = _faker.PickRandom(colleges.Where(c => !c.AllowCba));

            // Act
            var(sourceId, sourcePartnerId) = await _lookupsCache.GetSourcePartnerId(college.Code);

            // Assert
            sourcePartnerId.Should().BeNull();
        }

        [Fact]
        [UnitTest("Extensions")]
        public async Task GetSourcePartnerId_ShouldPass_When_ReferralPartner_AllowedCba()
        {
            // Arrange
            var referralPartners = await _lookupsCache.GetReferralPartners();
            var referralPartner = _faker.PickRandom(referralPartners.Where(c => c.AllowCba));

            // Act
            var(sourceId, sourcePartnerId) = await _lookupsCache.GetSourcePartnerId(referralPartner.Code);

            // Assert
            sourcePartnerId.Should().Be(referralPartner.Id);
        }

        [Fact]
        [UnitTest("Extensions")]
        public async Task GetSourcePartnerId_ShouldPass_When_ReferralPartner_NotAllowedCba()
        {
            // Arrange
            var referralPartners = await _lookupsCache.GetReferralPartners();
            var referralPartner = _faker.PickRandom(referralPartners.Where(c => !c.AllowCba));

            // Act
            var(sourceId, sourcePartnerId) = await _lookupsCache.GetSourcePartnerId(referralPartner.Code);

            // Assert
            sourcePartnerId.Should().BeNull();
        }
    }
}
