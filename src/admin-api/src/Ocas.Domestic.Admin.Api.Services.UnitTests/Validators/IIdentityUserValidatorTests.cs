using System.Security.Principal;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators
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
                User = Mock.Of<IPrincipal>()
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
