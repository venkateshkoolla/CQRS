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
    public class GetFinancialTransactionsValidatorTests : IClassFixture<GetFinancialTransactionsValidator>
    {
        private readonly GetFinancialTransactionsValidator _validator;

        public GetFinancialTransactionsValidatorTests(GetFinancialTransactionsValidator validator)
        {
            _validator = validator;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetFinancialTransactionsValidator_ShouldPass()
        {
            // Arrange
            var model = new Faker<GetFinancialTransactions>()
                .RuleFor(o => o.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(o => o.User, _ => Mock.Of<IPrincipal>())
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetFinancialTransactionsValidator_ShouldFail_When_ApplicationId_Empty()
        {
            // Arrange
            var model = new Faker<GetFinancialTransactions>()
                .RuleFor(o => o.ApplicationId, _ => Guid.Empty)
                .RuleFor(o => o.User, _ => Mock.Of<IPrincipal>())
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.ContainSingle(x => x.ErrorMessage == "'Application Id' must not be empty.");
        }
    }
}
