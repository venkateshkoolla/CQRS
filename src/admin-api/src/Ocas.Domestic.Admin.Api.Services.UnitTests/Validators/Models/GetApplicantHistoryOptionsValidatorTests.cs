using System;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class GetApplicantHistoryOptionsValidatorTests
    {
        private readonly GetApplicantHistoryOptionsValidator _validator;

        public GetApplicantHistoryOptionsValidatorTests()
        {
            _validator = new GetApplicantHistoryOptionsValidator();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetApplicantHistoryOptionsValidator_ShouldPass()
        {
            // Arrange
            var model = new Faker<GetApplicantHistoryOptions>()
                        .RuleFor(x => x.FromDate, f => f.Date.Between(DateTime.Now.AddMonths(-2).AsUtc(), DateTime.Now.AddMonths(-3).AsUtc()).AsUtc().ToStringOrDefault())
                        .RuleFor(x => x.ToDate, f => f.Date.Between(DateTime.Now.AddMonths(-1).AsUtc(), DateTime.Now.AddMonths(-2).AsUtc()).AsUtc().ToStringOrDefault())
                        .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetApplicantHistoryOptionsValidator_ShouldPass_When_Dates_Empty()
        {
            // Arrange
            var model = new Faker<GetApplicantHistoryOptions>()
                        .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetApplicantHistoryOptionsValidator_ShouldPass_When_OnlyFromDate_Empty()
        {
            // Arrange
            var model = new Faker<GetApplicantHistoryOptions>()
                        .RuleFor(x => x.ToDate, f => f.Date.Past().AsUtc().ToStringOrDefault())
                        .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetApplicantHistoryOptionsValidator_ShouldPass_When_OnlyToDate_Empty()
        {
            // Arrange
            var model = new Faker<GetApplicantHistoryOptions>()
                        .RuleFor(x => x.FromDate, f => f.Date.Past().AsUtc().ToStringOrDefault())
                        .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetApplicantHistoryOptionsValidator_ShouldThrow_When_FromDate_FutureDate()
        {
            // Arrange
            var model = new Faker<GetApplicantHistoryOptions>()
                        .RuleFor(x => x.FromDate, f => f.Date.Future().AsUtc().ToStringOrDefault())
                        .RuleFor(x => x.ToDate, f => f.Date.Past().AsUtc().ToStringOrDefault())
                        .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "'From Date' can not be a future date");
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'From Date' {model.FromDate} can not be greater than 'To Date' {model.ToDate}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetApplicantHistoryOptionsValidator_ShouldThrow_When_ToDate_FutureDate()
        {
            // Arrange
            var model = new Faker<GetApplicantHistoryOptions>()
                        .RuleFor(x => x.FromDate, f => f.Date.Past().AsUtc().ToStringOrDefault())
                        .RuleFor(x => x.ToDate, f => f.Date.Future().AsUtc().ToStringOrDefault())
                        .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "'To Date' can not be a future date");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetApplicantHistoryOptionsValidator_ShouldThrow_When_FromDate_GreaterThan_ToDate()
        {
            // Arrange
            var model = new Faker<GetApplicantHistoryOptions>()
                        .RuleFor(x => x.FromDate, f => f.Date.Between(DateTime.Now.AddMonths(-2).AsUtc(), DateTime.Now.AddMonths(-3).AsUtc()).AsUtc().ToStringOrDefault())
                        .RuleFor(x => x.ToDate, f => f.Date.Between(DateTime.Now.AddMonths(-4).AsUtc(), DateTime.Now.AddMonths(-5).AsUtc()).AsUtc().ToStringOrDefault())
                        .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.ContainSingle(x => x.ErrorMessage == $"'From Date' {model.FromDate} can not be greater than 'To Date' {model.ToDate}");
        }
    }
}
