using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Admin.TestFramework;
using Ocas.Domestic.Apply.Core.Extensions;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class GetApplicantBriefOptionsValidatorTests
    {
        private readonly Faker _faker;
        private readonly ILookupsCache _lookupsCache;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly GetApplicantBriefOptionsValidator _validator;

        private readonly Faker<GetApplicantBriefOptions> _modelFaker;

        public GetApplicantBriefOptionsValidatorTests()
        {
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _validator = new GetApplicantBriefOptionsValidator(_lookupsCache);

            _modelFaker = new Faker<GetApplicantBriefOptions>()
                .RuleFor(o => o.AccountNumber, f => f.Random.ReplaceNumbers("############"))
                .RuleFor(o => o.ApplicationCycleId, f => f.PickRandom(_modelFakerFixture.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id)
                .RuleFor(o => o.ApplicationNumber, f => f.Random.ReplaceNumbers("########"))
                .RuleFor(o => o.ApplicationStatusId, f => f.PickRandom(_modelFakerFixture.AllAdminLookups.ApplicationStatuses).Id)
                .RuleFor(o => o.BirthDate, f => f.Date.Past(2, DateTime.UtcNow).AsUtc().ToStringOrDefault())
                .RuleFor(o => o.Email, f => f.Internet.Email())
                .RuleFor(o => o.FirstName, f => f.Person.FirstName)
                .RuleFor(o => o.LastName, f => f.Person.LastName)
                .RuleFor(o => o.MiddleName, f => f.Person.FirstName)
                .RuleFor(o => o.OntarioEducationNumber, f => f.Random.ReplaceNumbers("#########"))
                .RuleFor(o => o.PhoneNumber, f => f.Random.ReplaceNumbers("##########"))
                .RuleFor(o => o.PreviousLastName, f => f.Person.LastName)
                .RuleFor(o => o.Mident, f => f.PickRandom(_modelFakerFixture.AllAdminLookups.HighSchools.Where(h => !string.IsNullOrEmpty(h.Mident) && h.Mident.Length == 6)).Mident);
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldPass_When_Full()
        {
            // Arrange
            var model = _modelFaker.Generate();

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldPass_When_Empty()
        {
            // Arrange
            var model = new GetApplicantBriefOptions();

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldPass_When_ApplicationStatusId_Null()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.ApplicationStatusId = null;

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_AccountNumber_Length()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.AccountNumber = _faker.Random.String2(15);

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
             .And.OnlyContain(x => x.ErrorMessage == $"'Account Number' must be 12 characters in length. You entered {model.AccountNumber.Length} characters.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_AccountNumber_Regex()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.AccountNumber = _faker.Random.String2(12);

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Account Number' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_ApplicationCycleId_Empty()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.ApplicationCycleId = Guid.Empty;

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Application Cycle Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_ApplicationCycleId_Invalid()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.ApplicationCycleId = _faker.Random.Guid();

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Application Cycle Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_ApplicationCycleId_Inactive()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.ApplicationCycleId = _faker.PickRandom(_modelFakerFixture.AllAdminLookups.ApplicationCycles.Where(a => a.Status != Constants.ApplicationCycleStatuses.Active)).Id;

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Application Cycle Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_ApplicationNumber_Length()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.ApplicationNumber = _faker.Random.String2(10);

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == $"'Application Number' must be between 8 and 9 characters. You entered {model.ApplicationNumber.Length} characters.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_ApplicationNumber_Regex()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.ApplicationNumber = _faker.Random.String2(9);

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Application Number' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_ApplicationStatusId_Empty_Guid()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.ApplicationStatusId = Guid.Empty;

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Application Status Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_ApplicationStatusId_NotValid()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.ApplicationStatusId = Guid.NewGuid();

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Application Status Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_BirthDate_NotADate()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.BirthDate = "NotADate";

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
                    .And.OnlyContain(x => x.ErrorMessage == $"'Birth Date' is not valid: {model.BirthDate}");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_BirthDate_InFuture()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.BirthDate = _faker.Date.Future().AsUtc().ToStringOrDefault();

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
                    .And.OnlyContain(x => x.ErrorMessage == $"'Birth Date' cannot be in future: {model.BirthDate}");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_FirstName_TooLong()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.FirstName = _faker.Random.String2(45);

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
             .And.OnlyContain(x => x.ErrorMessage == $"The length of 'First Name' must be 30 characters or fewer. You entered {model.FirstName.Length} characters.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_FirstName_Regex()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.FirstName = _faker.Random.String(15);

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
             .And.OnlyContain(x => x.ErrorMessage == "'First Name' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_LastName_TooLong()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.LastName = _faker.Random.String2(45);

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
             .And.OnlyContain(x => x.ErrorMessage == $"The length of 'Last Name' must be 30 characters or fewer. You entered {model.LastName.Length} characters.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_LastName_Regex()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.LastName = _faker.Random.String(15);

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
             .And.OnlyContain(x => x.ErrorMessage == "'Last Name' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_MiddleName_TooLong()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.MiddleName = _faker.Random.String2(45);

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
             .And.OnlyContain(x => x.ErrorMessage == $"The length of 'Middle Name' must be 30 characters or fewer. You entered {model.MiddleName.Length} characters.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_MiddleName_Regex()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.MiddleName = _faker.Random.String(15);

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
             .And.OnlyContain(x => x.ErrorMessage == "'Middle Name' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_Mident_Length()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.Mident = _faker.Random.String(10);

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
             .And.OnlyContain(x => x.ErrorMessage == $"'Mident' must be 6 characters in length. You entered {model.Mident.Length} characters.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_Mident_Invalid()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.Mident = _faker.Random.String(6).ToUpperInvariant();

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
             .And.OnlyContain(x => x.ErrorMessage == "'Mident' does not exist.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_OntarioEducationNumber_Length()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.OntarioEducationNumber = _faker.Random.String(10);

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
             .And.OnlyContain(x => x.ErrorMessage == $"'Ontario Education Number' must be 9 characters in length. You entered {model.OntarioEducationNumber.Length} characters.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_OntarioEducationNumber_Regex()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.OntarioEducationNumber = _faker.Random.String2(9);

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
             .And.OnlyContain(x => x.ErrorMessage == "'Ontario Education Number' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_PreviousLastName_TooLong()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.PreviousLastName = _faker.Random.String2(45);

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
             .And.OnlyContain(x => x.ErrorMessage == $"The length of 'Previous Last Name' must be 30 characters or fewer. You entered {model.PreviousLastName.Length} characters.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_PreviousLastName_Regex()
        {
            // Arrange
            var model = _modelFaker.Generate();
            model.PreviousLastName = _faker.Random.String(15);

            // Act
            var results = await _validator.ValidateAsync(model);

            // Assert
            results.IsValid.Should().BeFalse();
            results.Errors.Should().ContainSingle()
             .And.OnlyContain(x => x.ErrorMessage == "'Previous Last Name' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_Intl_Phone_Invalid()
        {
            // Arrange
            const string invalidChars = "5195551234x1234";
            var model = _modelFaker.Generate();

            model.PhoneNumber = invalidChars;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Phone Number' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_Intl_Phone_TooLong()
        {
            // Arrange
            const string invalidChars = "5195551234123456";
            var model = _modelFaker.Generate();

            model.PhoneNumber = invalidChars;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Phone Number' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetApplicantBriefOptionsValidator_ShouldFail_When_Intl_Phone_TooShort()
        {
            // Arrange
            const string invalidChars = "5551234";
            var model = _modelFaker.Generate();
            model.PhoneNumber = invalidChars;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Phone Number' is not in the correct format.");
        }
    }
}
