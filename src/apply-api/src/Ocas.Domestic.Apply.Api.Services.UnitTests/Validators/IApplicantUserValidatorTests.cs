using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Services.Validators;
using Ocas.Domestic.Apply.Core.Messages;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators
{
    public class IApplicantUserValidatorTests : IClassFixture<IApplicantUserValidator>
    {
        private readonly IApplicantUserValidator _validator;

        public IApplicantUserValidatorTests(IApplicantUserValidator validator)
        {
            _validator = validator;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass()
        {
            var message = new MyRequest
            {
                ApplicantId = Guid.NewGuid()
            };

            var result = await _validator.ValidateAsync(message);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenNoUser()
        {
            var message = new MyRequest
            {
                ApplicantId = Guid.Empty
            };

            var result = await _validator.ValidateAsync(message);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Applicant Id' must not be empty.");
        }

        private class MyRequest : IApplicantUser
        {
            public Guid ApplicantId { get; set; }

            public IPrincipal User => new ClaimsPrincipal();
        }
    }
}
