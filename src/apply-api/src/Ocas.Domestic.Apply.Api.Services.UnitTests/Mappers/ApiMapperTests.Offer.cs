using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Ocas.Domestic.Apply.Core.Extensions;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Mappers
{
    public partial class ApiMapperTests
    {
        [Fact]
        [UnitTest("Mappers")]
        public void MapOffers_ShouldPass_When_Empty()
        {
            // Arrange
            var dtoOffers = new List<Dto.Offer>();

            // Act
            var offers = _apiMapper.MapOffers(dtoOffers, _models.AllApplyLookups.OfferStates, _models.AllApplyLookups.ApplicationStatuses, _appSettingsExtras);

            // Assert
            offers.Should().BeEmpty();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapOffers_ShouldPass_When_DeleteOffer_Then_CanRespond_False()
        {
            // Arrange
            var deletedOffer = _models.AllApplyLookups.OfferStates.First(x => x.Code == Constants.Offers.State.Deleted);
            var activeApplication = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.Active);

            var dtoOffer = new Dto.Offer
            {
                OfferStateId = deletedOffer.Id,
                ApplicationStatusId = activeApplication.Id,
                HardExpiryDate = _dataFakerFixture.Faker.Date.Future(1, DateTime.UtcNow.ToDateInEstAsUtc())
            };
            var dtoOffers = new List<Dto.Offer> { dtoOffer };

            // Act
            var offers = _apiMapper.MapOffers(dtoOffers, _models.AllApplyLookups.OfferStates, _models.AllApplyLookups.ApplicationStatuses, _appSettingsExtras);

            // Assert
            offers.Should().NotBeNullOrEmpty()
                .And.ContainSingle();
            var offer = offers.First();
            offer.CanRespond.Should().BeFalse();
            offer.OfferLockReleaseDate.Should().BeNull();
            offer.IsSoftExpired.Should().BeFalse();
            offer.IsHardExpired.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapOffers_ShouldPass_When_NewApplyApplication_Then_CanRespond_False()
        {
            // Arrange
            var activeOffer = _models.AllApplyLookups.OfferStates.First(x => x.Code == Constants.Offers.State.Active);
            var newApplyApplication = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.NewApply);

            var dtoOffer = new Dto.Offer
            {
                OfferStateId = activeOffer.Id,
                ApplicationStatusId = newApplyApplication.Id,
                HardExpiryDate = _dataFakerFixture.Faker.Date.Future(1, DateTime.UtcNow.ToDateInEstAsUtc())
            };
            var dtoOffers = new List<Dto.Offer> { dtoOffer };

            // Act
            var offers = _apiMapper.MapOffers(dtoOffers, _models.AllApplyLookups.OfferStates, _models.AllApplyLookups.ApplicationStatuses, _appSettingsExtras);

            // Assert
            offers.Should().NotBeNullOrEmpty()
                .And.ContainSingle();
            var offer = offers.First();
            offer.CanRespond.Should().BeFalse();
            offer.OfferLockReleaseDate.Should().BeNull();
            offer.IsSoftExpired.Should().BeFalse();
            offer.IsHardExpired.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapOffers_ShouldPass_When_Active_Then_CanRespond_True()
        {
            // Arrange
            var activeOffer = _models.AllApplyLookups.OfferStates.First(x => x.Code == Constants.Offers.State.Active);
            var activeApplication = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.Active);

            var dtoOffer = new Dto.Offer
            {
                OfferStateId = activeOffer.Id,
                ApplicationStatusId = activeApplication.Id,
                HardExpiryDate = _dataFakerFixture.Faker.Date.Future(1, DateTime.UtcNow.ToDateInEstAsUtc())
            };
            var dtoOffers = new List<Dto.Offer> { dtoOffer };

            // Act
            var offers = _apiMapper.MapOffers(dtoOffers, _models.AllApplyLookups.OfferStates, _models.AllApplyLookups.ApplicationStatuses, _appSettingsExtras);

            // Assert
            offers.Should().NotBeNullOrEmpty()
                .And.ContainSingle();
            var offer = offers.First();
            offer.CanRespond.Should().BeTrue();
            offer.OfferLockReleaseDate.Should().BeNull();
            offer.IsSoftExpired.Should().BeFalse();
            offer.IsHardExpired.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapOffers_ShouldPass_When_SoftExpiryDate_Past_Then_IsSoftExpired_True()
        {
            // Arrange
            var activeOffer = _models.AllApplyLookups.OfferStates.First(x => x.Code == Constants.Offers.State.Active);
            var activeApplication = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.Active);

            var dtoOffer = new Dto.Offer
            {
                OfferStateId = activeOffer.Id,
                ApplicationStatusId = activeApplication.Id,
                HardExpiryDate = _dataFakerFixture.Faker.Date.Future(1, DateTime.UtcNow.ToDateInEstAsUtc()),
                SoftExpiryDate = _dataFakerFixture.Faker.Date.Past(1, DateTime.UtcNow.ToDateInEstAsUtc())
            };
            var dtoOffers = new List<Dto.Offer> { dtoOffer };

            // Act
            var offers = _apiMapper.MapOffers(dtoOffers, _models.AllApplyLookups.OfferStates, _models.AllApplyLookups.ApplicationStatuses, _appSettingsExtras);

            // Assert
            offers.Should().NotBeNullOrEmpty()
                .And.ContainSingle();
            var offer = offers.First();
            offer.CanRespond.Should().BeTrue();
            offer.OfferLockReleaseDate.Should().BeNull();
            offer.IsSoftExpired.Should().BeTrue();
            offer.IsHardExpired.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapOffers_ShouldPass_When_SoftExpiryDate_Future_Then_IsSoftExpired_False()
        {
            // Arrange
            var activeOffer = _models.AllApplyLookups.OfferStates.First(x => x.Code == Constants.Offers.State.Active);
            var activeApplication = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.Active);

            var dtoOffer = new Dto.Offer
            {
                OfferStateId = activeOffer.Id,
                ApplicationStatusId = activeApplication.Id,
                HardExpiryDate = _dataFakerFixture.Faker.Date.Future(1, DateTime.UtcNow.ToDateInEstAsUtc()),
                SoftExpiryDate = _dataFakerFixture.Faker.Date.Future(1, DateTime.UtcNow.ToDateInEstAsUtc())
            };
            var dtoOffers = new List<Dto.Offer> { dtoOffer };

            // Act
            var offers = _apiMapper.MapOffers(dtoOffers, _models.AllApplyLookups.OfferStates, _models.AllApplyLookups.ApplicationStatuses, _appSettingsExtras);

            // Assert
            offers.Should().NotBeNullOrEmpty()
                .And.ContainSingle();
            var offer = offers.First();
            offer.CanRespond.Should().BeTrue();
            offer.OfferLockReleaseDate.Should().BeNull();
            offer.IsSoftExpired.Should().BeFalse();
            offer.IsHardExpired.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapOffers_ShouldPass_When_HardExpiryDate_Past_Then_IsHardExpired_True()
        {
            // Arrange
            var activeOffer = _models.AllApplyLookups.OfferStates.First(x => x.Code == Constants.Offers.State.Active);
            var activeApplication = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.Active);

            var dtoOffer = new Dto.Offer
            {
                OfferStateId = activeOffer.Id,
                ApplicationStatusId = activeApplication.Id,
                HardExpiryDate = _dataFakerFixture.Faker.Date.Past(1, DateTime.UtcNow.ToDateInEstAsUtc())
            };
            var dtoOffers = new List<Dto.Offer> { dtoOffer };

            // Act
            var offers = _apiMapper.MapOffers(dtoOffers, _models.AllApplyLookups.OfferStates, _models.AllApplyLookups.ApplicationStatuses, _appSettingsExtras);

            // Assert
            offers.Should().NotBeNullOrEmpty()
                .And.ContainSingle();
            var offer = offers.First();
            offer.CanRespond.Should().BeFalse();
            offer.OfferLockReleaseDate.Should().BeNull();
            offer.IsSoftExpired.Should().BeFalse();
            offer.IsHardExpired.Should().BeTrue();
        }
    }
}
