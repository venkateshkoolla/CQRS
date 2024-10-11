using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Ocas.Domestic.Apply.Models;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Mappers
{
    public partial class ApiMapperTests
    {
        [Fact]
        [UnitTest("Mappers")]
        public void MapCollegeTransmissionHistories_ShouldPass()
        {
            // Arrange
            var coltraneXciDs = new List<long> { 1000000, 1000010, 1000020, 1000030 };

            var colleges = _models.AllAdminLookups.Colleges;
            var dtoInsertTransmissions = new Faker<Dto.CollegeTransmission>()
                .RuleFor(x => x.Id, f => f.Random.Number(1, 1000))
                .RuleFor(x => x.ColtraneXcId, f => f.PickRandom(coltraneXciDs))
                .RuleFor(x => x.TransactionType, _ => 'I')
                .RuleFor(x => x.Data, f => f.Random.String(10))
                .RuleFor(x => x.LastLoadDateTime, f => f.Date.Past().AsUtc())
                .RuleFor(x => x.CollegeCode, f => f.PickRandom(colleges).Code)
                .RuleFor(x => x.BusinessKey, Guid.NewGuid())
                .Generate(10);

            var dtoUpdateTransmissions = new Faker<Dto.CollegeTransmission>()
                .RuleFor(x => x.Id, f => f.Random.Number(1001, 2000))
                .RuleFor(x => x.ColtraneXcId, f => f.PickRandom(dtoInsertTransmissions.Select(x => x.ColtraneXcId)))
                .RuleFor(x => x.TransactionType, _ => 'U')
                .RuleFor(x => x.Data, f => f.Random.String(10))
                .RuleFor(x => x.LastLoadDateTime, f => f.Date.Past().AsUtc())
                .RuleFor(x => x.CollegeCode, f => f.PickRandom(colleges).Code)
                .RuleFor(x => x.BusinessKey, Guid.NewGuid())
                .Generate(10);

            var dtoDeleteTransmissions = new Faker<Dto.CollegeTransmission>()
                .RuleFor(x => x.Id, f => f.Random.Number(2001, 3000))
                .RuleFor(x => x.ColtraneXcId, f => f.PickRandom(dtoInsertTransmissions.Select(x => x.ColtraneXcId)))
                .RuleFor(x => x.TransactionType, _ => 'D')
                .RuleFor(x => x.Data, f => f.Random.String(10))
                .RuleFor(x => x.LastLoadDateTime, f => f.Date.Past().AsUtc())
                .RuleFor(x => x.CollegeCode, f => f.PickRandom(colleges).Code)
                .RuleFor(x => x.BusinessKey, Guid.NewGuid())
                .Generate(10);

            var allCollegeTransmissions = dtoInsertTransmissions.Concat(dtoUpdateTransmissions).Concat(dtoDeleteTransmissions).ToList();

            // Act
            var transmissions = _apiMapper.MapCollegeTransmissionHistories(allCollegeTransmissions, colleges, allCollegeTransmissions, new TranslationsDictionary(new Dictionary<string, string>()));

            // Assert
            transmissions.Should().NotBeNullOrEmpty();

            var insertTransmissions = transmissions.Where(x => x.TransactionType == 'I');
            insertTransmissions.Should().OnlyContain(x => x.Details == null);
            insertTransmissions.Select(x => x.ContextId).Should().BeEquivalentTo(dtoInsertTransmissions.Select(x => x.BusinessKey));
            insertTransmissions.Select(x => x.Sent).Should().BeEquivalentTo(dtoInsertTransmissions.Select(x => x.LastLoadDateTime));

            var deleteTransmissions = transmissions.Where(x => x.TransactionType == 'D');
            deleteTransmissions.Should().OnlyContain(x => x.Details != null && x.Details.Count > 0);
            deleteTransmissions.Select(x => x.ContextId).Should().BeEquivalentTo(dtoDeleteTransmissions.Select(x => x.BusinessKey));
            deleteTransmissions.Select(x => x.Sent).Should().BeEquivalentTo(dtoDeleteTransmissions.Select(x => x.LastLoadDateTime));

            var updateTransmissions = transmissions.Where(x => x.TransactionType == 'U');
            updateTransmissions.Should().OnlyContain(x => x.Details != null && x.Details.Count > 0);
            updateTransmissions.Select(x => x.ContextId).Should().BeEquivalentTo(dtoUpdateTransmissions.Select(x => x.BusinessKey));
            updateTransmissions.Select(x => x.Sent).Should().BeEquivalentTo(dtoUpdateTransmissions.Select(x => x.LastLoadDateTime));
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapCollegeTransmissionHistories_With_Description_ShouldPass()
        {
            // Arrange
            var coltraneXciDs = new List<long> { 1000000, 1000010, 1000020, 1000030 };

            var colleges = _models.AllAdminLookups.Colleges;
            var dtoInsertTransmissions = new Faker<Dto.CollegeTransmission>()
                .RuleFor(x => x.Id, f => f.Random.Number(1, 1000))
                .RuleFor(x => x.ColtraneXcId, f => f.PickRandom(coltraneXciDs))
                .RuleFor(x => x.TransactionCode, Constants.CollegeTransmissionCodes.Applicant)
                .RuleFor(x => x.TransactionType, Constants.CollegeTransmissionTransactionTypes.Insert)
                .RuleFor(x => x.Data, f => f.Random.String(10))
                .RuleFor(x => x.LastLoadDateTime, f => f.Date.Past().AsUtc())
                .RuleFor(x => x.CollegeCode, f => f.PickRandom(colleges).Code)
                .RuleFor(x => x.BusinessKey, Guid.NewGuid())
                .Generate(10);

            var dtoUpdateTransmissions = new Faker<Dto.CollegeTransmission>()
                .RuleFor(x => x.Id, f => f.Random.Number(1001, 2000))
                .RuleFor(x => x.ColtraneXcId, f => f.PickRandom(dtoInsertTransmissions.Select(x => x.ColtraneXcId)))
                .RuleFor(x => x.TransactionCode, Constants.CollegeTransmissionCodes.Applicant)
                .RuleFor(x => x.TransactionType, Constants.CollegeTransmissionTransactionTypes.Update)
                .RuleFor(x => x.Data, f => f.Random.String(10))
                .RuleFor(x => x.LastLoadDateTime, f => f.Date.Past().AsUtc())
                .RuleFor(x => x.CollegeCode, f => f.PickRandom(colleges).Code)
                .RuleFor(x => x.BusinessKey, Guid.NewGuid())
                .Generate(10);

            var allCollegeTransmissions = dtoInsertTransmissions.Concat(dtoUpdateTransmissions).ToList();

            var translations = new TranslationsDictionary(new Dictionary<string, string>
            {
                { "report.transmission.headers.ActivityCode_ACI", "Application sent to {0}" },
                { "report.transmission.headers.ActivityCode_ACU", "Account Information Updates sent to {0}" }
            });

            // Act
            var transmissions = _apiMapper.MapCollegeTransmissionHistories(allCollegeTransmissions, colleges, allCollegeTransmissions, translations);

            // Assert
            transmissions.Should().NotBeNullOrEmpty();

            var insertTransmissions = transmissions.Where(x => x.TransactionType == 'I');
            insertTransmissions.Should().OnlyContain(x => x.Details == null);
            insertTransmissions.Select(x => x.ContextId).Should().BeEquivalentTo(dtoInsertTransmissions.Select(x => x.BusinessKey));
            insertTransmissions.Select(x => x.Sent).Should().BeEquivalentTo(dtoInsertTransmissions.Select(x => x.LastLoadDateTime));
            insertTransmissions.Select(x => x.Description).Should().BeEquivalentTo(dtoInsertTransmissions.Select(x => string.Format("Application sent to {0}", colleges.FirstOrDefault(y => y.Code == x.CollegeCode).Name)));

            var updateTransmissions = transmissions.Where(x => x.TransactionType == 'U');
            updateTransmissions.Should().OnlyContain(x => x.Details != null && x.Details.Count > 0);
            updateTransmissions.Select(x => x.ContextId).Should().BeEquivalentTo(dtoUpdateTransmissions.Select(x => x.BusinessKey));
            updateTransmissions.Select(x => x.Sent).Should().BeEquivalentTo(dtoUpdateTransmissions.Select(x => x.LastLoadDateTime));
            updateTransmissions.Select(x => x.Description).Should().BeEquivalentTo(dtoUpdateTransmissions.Select(x => string.Format("Account Information Updates sent to {0}", colleges.FirstOrDefault(y => y.Code == x.CollegeCode).Name)));
        }
    }
}
