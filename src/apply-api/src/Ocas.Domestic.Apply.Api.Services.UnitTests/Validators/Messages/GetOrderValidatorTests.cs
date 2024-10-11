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
    public class GetOrderValidatorTests : IClassFixture<GetOrderValidator>
    {
        private readonly GetOrderValidator _validator;
        private readonly IPrincipal _user;

        public GetOrderValidatorTests(GetOrderValidator validator)
        {
            _validator = validator;
            _user = Mock.Of<IPrincipal>();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetOrderValidator_ShouldPass()
        {
            // Arrange
            var model = new Faker<GetOrder>()
                .RuleFor(o => o.OrderId, _ => Guid.NewGuid())
                .RuleFor(o => o.User, _ => _user)
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetOrderValidator_ShouldFail_When_OrderId_Empty()
        {
            // Arrange
            var model = new Faker<GetOrder>()
                .RuleFor(o => o.OrderId, _ => Guid.Empty)
                .RuleFor(o => o.User, _ => _user)
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.ContainSingle(x => x.ErrorMessage == "'Order Id' must not be empty.");
        }
    }
}
