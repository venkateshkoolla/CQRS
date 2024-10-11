using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Services.Validators;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators
{
    public class IIdentityUserValidatorTests : IClassFixture<IIdentityUserValidator>
    {
        private readonly IIdentityUserValidator _validator;

        public IIdentityUserValidatorTests(IIdentityUserValidator validator)
        {
            _validator = validator;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass()
        {
            var message = new MyRequest
            {
                User = new ClaimsPrincipal()
            };

            var result = await _validator.ValidateAsync(message);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenNoUser()
        {
            var message = new MyRequest();

            var result = await _validator.ValidateAsync(message);

            result.IsValid.Should().BeFalse();
        }

        private class MyRequest : IIdentityUser
        {
            public IPrincipal User { get; set; }
        }
    }
}
