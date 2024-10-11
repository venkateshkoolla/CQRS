using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Messages
{
    public class UpdateEducationStatusValidatorTests : IClassFixture<UpdateEducationStatusValidator>
    {
        private readonly UpdateEducationStatusValidator _validator;

        public UpdateEducationStatusValidatorTests(UpdateEducationStatusValidator validator)
        {
            _validator = validator;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass_WhenEnrolled()
        {
            var user = new Mock<IPrincipal>();

            var model = new Faker<UpdateEducationStatus>()
                .RuleFor(o => o.User, _ => user.Object)
                .RuleFor(o => o.ApplicantId, Guid.NewGuid())
                .RuleFor(o => o.EnrolledInHighSchool, true)
                .RuleFor(o => o.GraduatedHighSchool, (bool?)null)
                .RuleFor(o => o.GraduationHighSchoolDate, f => f.Date.Future(1).ToUniversalTime().ToStringOrDefault(Constants.DateFormat.YearMonthDashed))
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass_WhenNotEnrolled()
        {
            var user = new Mock<IPrincipal>();

            var model = new Faker<UpdateEducationStatus>()
                .RuleFor(o => o.User, _ => user.Object)
                .RuleFor(o => o.ApplicantId, Guid.NewGuid())
                .RuleFor(o => o.EnrolledInHighSchool, false)
                .RuleFor(o => o.GraduatedHighSchool, f => f.Random.Bool())
                .RuleFor(o => o.GraduationHighSchoolDate, string.Empty)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_NoEnrolled()
        {
            var user = new Mock<IPrincipal>();

            var model = new Faker<UpdateEducationStatus>()
                .RuleFor(o => o.User, _ => user.Object)
                .RuleFor(o => o.ApplicantId, Guid.NewGuid())
                .RuleFor(o => o.EnrolledInHighSchool, (bool?)null)
                .RuleFor(o => o.GraduatedHighSchool, (bool?)null)
                .RuleFor(o => o.GraduationHighSchoolDate, string.Empty)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Enrolled In High School' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenNotEnrolled_EmptyGraduated()
        {
            var user = new Mock<IPrincipal>();

            var model = new Faker<UpdateEducationStatus>()
                .RuleFor(o => o.User, _ => user.Object)
                .RuleFor(o => o.ApplicantId, Guid.NewGuid())
                .RuleFor(o => o.EnrolledInHighSchool, false)
                .RuleFor(o => o.GraduatedHighSchool, (bool?)null)
                .RuleFor(o => o.GraduationHighSchoolDate, string.Empty)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Graduated High School' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenEnrolled_NoGraduationDate()
        {
            var user = new Mock<IPrincipal>();

            var model = new Faker<UpdateEducationStatus>()
                .RuleFor(o => o.User, _ => user.Object)
                .RuleFor(o => o.ApplicantId, Guid.NewGuid())
                .RuleFor(o => o.EnrolledInHighSchool, true)
                .RuleFor(o => o.GraduatedHighSchool, (bool?)null)
                .RuleFor(o => o.GraduationHighSchoolDate, string.Empty)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Graduation High School Date' must be in correct format of yyyy-MM.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenEnrolled_InvalidGraduationDate()
        {
            var user = new Mock<IPrincipal>();

            var model = new Faker<UpdateEducationStatus>()
                .RuleFor(o => o.User, _ => user.Object)
                .RuleFor(o => o.ApplicantId, Guid.NewGuid())
                .RuleFor(o => o.EnrolledInHighSchool, true)
                .RuleFor(o => o.GraduatedHighSchool, (bool?)null)
                .RuleFor(o => o.GraduationHighSchoolDate, "NotADate")
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Graduation High School Date' must be in correct format of yyyy-MM.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenEnrolled_PastGraduationDate()
        {
            var user = new Mock<IPrincipal>();

            var refDate = DateTime.UtcNow.ToDateInEstAsUtc().ToStringOrDefault(Constants.DateFormat.YearMonthDashed);
            var model = new Faker<UpdateEducationStatus>()
                .RuleFor(o => o.User, _ => user.Object)
                .RuleFor(o => o.ApplicantId, Guid.NewGuid())
                .RuleFor(o => o.EnrolledInHighSchool, true)
                .RuleFor(o => o.GraduatedHighSchool, (bool?)null)
                .RuleFor(o => o.GraduationHighSchoolDate, f => f.Date.Past(2, refDate.ToDateTime(Constants.DateFormat.YearMonthDashed)).ToUniversalTime().ToStringOrDefault(Constants.DateFormat.YearMonthDashed))
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Graduation High School Date' must be in the future.");
        }
    }
}
