using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Core.Messages;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Messages
{
    public class VerifyApplicantOenValidatorTests : IClassFixture<VerifyApplicantOenValidator>
    {
        private readonly VerifyApplicantOenValidator _validator;

        public VerifyApplicantOenValidatorTests(VerifyApplicantOenValidator validator)
        {
            _validator = validator;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task VerifyApplicantOenValidator_ShouldPass()
        {
            var user = new Mock<IPrincipal>();

            var model = new Faker<VerifyApplicantOen>()
                .RuleFor(o => o.User, _ => user.Object)
                .RuleFor(o => o.ApplicantId, _ => Guid.NewGuid())
                .RuleFor(o => o.Oen, _ => string.Empty)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task VerifyApplicantOenValidator_ShouldFail_WhenEmptyApplicantId()
        {
            var user = new Mock<IPrincipal>();

            var model = new Faker<VerifyApplicantOen>()
                .RuleFor(o => o.User, _ => user.Object)
                .RuleFor(o => o.ApplicantId, _ => Guid.Empty)
                .RuleFor(o => o.Oen, _ => string.Empty)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Applicant Id' must not be empty.");
        }
    }
}
