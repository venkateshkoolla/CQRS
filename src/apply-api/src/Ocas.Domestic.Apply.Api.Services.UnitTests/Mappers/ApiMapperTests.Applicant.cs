using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Mappers
{
    public partial class ApiMapperTests
    {
        [Fact]
        [UnitTest("Mappers")]
        public void MapApplicant_ShouldPass_When_AboriginalStatusId_NotShownInPortal()
        {
            // Arrange
            var aboriginalStatus = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.AboriginalStatuses.Where(x => !x.ShowInPortal));
            var contact = new Dto.Contact
            {
                BirthDate = _dataFakerFixture.Faker.Date.Past(18, DateTime.UtcNow),
                AboriginalStatusId = aboriginalStatus.Id
            };

            var mask = Convert.ToInt32(aboriginalStatus.ColtraneCode, 2);
            var selectedAboriginalStatuses = new List<Guid>();
            foreach (var portalAboriginalStatus in _models.AllApplyLookups.AboriginalStatuses.Where(x => x.ShowInPortal))
            {
                var maskValue = Convert.ToInt32(portalAboriginalStatus.ColtraneCode, 2);

                if ((mask & maskValue) != 0)
                {
                    selectedAboriginalStatuses.Add(portalAboriginalStatus.Id);
                }
            }

            // Act
            var applicant = _apiMapper.MapApplicant(contact, _models.AllApplyLookups.AboriginalStatuses, _models.AllApplyLookups.Titles);

            // Assert
            applicant.Should().NotBeNull();
            applicant.AboriginalStatuses.Should().BeEquivalentTo(selectedAboriginalStatuses);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapApplicant_ShouldPass_When_AboriginalStatusId_ShownInPortal()
        {
            // Arrange
            var aboriginalStatusId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.AboriginalStatuses.Where(x => x.ShowInPortal)).Id;
            var contact = new Dto.Contact
            {
                BirthDate = _dataFakerFixture.Faker.Date.Past(18, DateTime.UtcNow),
                AboriginalStatusId = aboriginalStatusId
            };

            // Act
            var applicant = _apiMapper.MapApplicant(contact, _models.AllApplyLookups.AboriginalStatuses, _models.AllApplyLookups.Titles);

            // Assert
            applicant.Should().NotBeNull();
            applicant.AboriginalStatuses.Should().ContainSingle()
                .And.OnlyContain(x => x == aboriginalStatusId);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapApplicant_ShouldPass_When_DoNotSendFalse_Then_AgreedToCaslTrue()
        {
            // Arrange
            var contact = new Dto.Contact
            {
                BirthDate = _dataFakerFixture.Faker.Date.Past(18, DateTime.UtcNow),
                DoNotSendMM = false,
                LastUsedInCampaign = _dataFakerFixture.Faker.Date.Past(1, DateTime.UtcNow)
            };

            // Act
            var applicant = _apiMapper.MapApplicant(contact, _models.AllApplyLookups.AboriginalStatuses, _models.AllApplyLookups.Titles);

            // Assert
            applicant.Should().NotBeNull();
            applicant.AgreedToCasl.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapApplicant_ShouldPass_When_DoNotSendNull_Then_AgreedToCaslNull()
        {
            // Arrange
            var contact = new Dto.Contact
            {
                BirthDate = _dataFakerFixture.Faker.Date.Past(18, DateTime.UtcNow),
                DoNotSendMM = null,
                LastUsedInCampaign = null
            };

            // Act
            var applicant = _apiMapper.MapApplicant(contact, _models.AllApplyLookups.AboriginalStatuses, _models.AllApplyLookups.Titles);

            // Assert
            applicant.Should().NotBeNull();
            applicant.AgreedToCasl.Should().BeNull();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapApplicant_ShouldPass_When_DoNotSendFlase_Then_AgreedToCaslNull()
        {
            // Arrange
            // this is how a new applicant looks
            var contact = new Dto.Contact
            {
                BirthDate = _dataFakerFixture.Faker.Date.Past(18, DateTime.UtcNow),
                DoNotSendMM = false,
                LastUsedInCampaign = null
            };

            // Act
            var applicant = _apiMapper.MapApplicant(contact, _models.AllApplyLookups.AboriginalStatuses, _models.AllApplyLookups.Titles);

            // Assert
            applicant.Should().NotBeNull();
            applicant.AgreedToCasl.Should().BeNull();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapApplicant_ShouldPass_When_DoNotSendTrue_Then_AgreedToCaslFalse()
        {
            // Arrange
            var contact = new Dto.Contact
            {
                BirthDate = _dataFakerFixture.Faker.Date.Past(18, DateTime.UtcNow),
                DoNotSendMM = true,
                LastUsedInCampaign = _dataFakerFixture.Faker.Date.Past()
            };

            // Act
            var applicant = _apiMapper.MapApplicant(contact, _models.AllApplyLookups.AboriginalStatuses, _models.AllApplyLookups.Titles);

            // Assert
            applicant.Should().NotBeNull();
            applicant.AgreedToCasl.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapApplicant_ShouldPass_When_TitleId_Known()
        {
            // Arrange
            var titleId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.Titles.Where(t => t.Code != Constants.Titles.Unknown)).Id;
            var contact = new Dto.Contact
            {
                BirthDate = _dataFakerFixture.Faker.Date.Past(18, DateTime.UtcNow),
                TitleId = titleId
            };

            // Act
            var applicant = _apiMapper.MapApplicant(contact, _models.AllApplyLookups.AboriginalStatuses, _models.AllApplyLookups.Titles);

            // Assert
            applicant.Should().NotBeNull();
            applicant.TitleId.Should().Be(titleId);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapApplicant_ShouldPass_When_TitleId_Unknown()
        {
            // Arrange
            var titleId = _models.AllApplyLookups.Titles.Single(t => t.Code == Constants.Titles.Unknown).Id;
            var contact = new Dto.Contact
            {
                BirthDate = _dataFakerFixture.Faker.Date.Past(18, DateTime.UtcNow),
                TitleId = titleId
            };

            // Act
            var applicant = _apiMapper.MapApplicant(contact, _models.AllApplyLookups.AboriginalStatuses, _models.AllApplyLookups.Titles);

            // Assert
            applicant.Should().NotBeNull();
            applicant.TitleId.Should().BeNull();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapApplicant_ShouldPass_When_Home_Matches_Mobile_Phone()
        {
            // Arrange
            var phone = _dataFakerFixture.Faker.Person.Phone;
            var contact = new Dto.Contact
            {
                BirthDate = _dataFakerFixture.Faker.Date.Past(18, DateTime.UtcNow),
                HomePhone = phone,
                MobilePhone = phone
            };

            // Act
            var applicant = _apiMapper.MapApplicant(contact, _models.AllApplyLookups.AboriginalStatuses, _models.AllApplyLookups.Titles);

            // Assert
            applicant.Should().NotBeNull();
            applicant.HomePhone.Should().BeNull();
            applicant.MobilePhone.Should().Be(phone);
        }
    }
}
