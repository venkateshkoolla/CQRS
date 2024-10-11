using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Ocas.Domestic.Apply.Admin.TestFramework;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Services.Validators.Messages;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class OfflinePaymentInfoValidatorTests
    {
        private readonly OfflinePaymentInfoValidator _validator;
        private readonly ModelFakerFixture _modelFaker;
        private readonly Faker _faker;

        public OfflinePaymentInfoValidatorTests()
        {
            _validator = new OfflinePaymentInfoValidator(XunitInjectionCollection.LookupsCache);
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OfflinePaymentInfoValidator_ShouldPass()
        {
            // Arrange
            var model = new OfflinePaymentInfo
            {
                PaymentMethodId = _faker.PickRandom(_modelFaker.AllAdminLookups.PaymentMethods).Id,
                Amount = 95.00M,
                Notes = _faker.Random.Words(5)
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OfflinePaymentInfoValidator_ShouldFail_When_PaymentMethod_NotExists()
        {
            // Arrange
            var model = new OfflinePaymentInfo
            {
                PaymentMethodId = Guid.NewGuid(),
                Amount = 95.00M,
                Notes = _faker.Random.Words(5)
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"'Payment Method Id' does not exist: {model.PaymentMethodId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OfflinePaymentInfoValidator_ShouldFail_When_PaymentMethod_Empty()
        {
            // Arrange
            var model = new OfflinePaymentInfo
            {
                PaymentMethodId = Guid.Empty,
                Amount = 95.00M,
                Notes = _faker.Random.Words(5)
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Payment Method Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OfflinePaymentInfoValidator_ShouldFail_When_Amount_Invalid()
        {
            // Arrange
            var model = new OfflinePaymentInfo
            {
                PaymentMethodId = _faker.PickRandom(_modelFaker.AllAdminLookups.PaymentMethods).Id,
                Amount = 95.12345M,
                Notes = _faker.Random.Words(5)
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Amount' cannot have more than 2 decimals.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OfflinePaymentInfoValidator_ShouldFail_When_Notes_Invalid_With_Payment()
        {
            // Arrange
            var model = new OfflinePaymentInfo
            {
                PaymentMethodId = _faker.PickRandom(_modelFaker.AllAdminLookups.PaymentMethods
                .Where(z => z.Code == Constants.PaymentMethods.Cheque
                || z.Code == Constants.PaymentMethods.MoneyOrder
                || z.Code == Constants.PaymentMethods.InteracOnline
                || z.Code == Constants.PaymentMethods.OnlineBanking)).Id,
                Amount = 95.00M,
                Notes = string.Empty
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"Payment number is required to proceed with offline payment for this Payment Type: {model.PaymentMethodId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OfflinePaymentInfoValidator_ShouldPass_When_Notes_Valid_With_Payment()
        {
            // Arrange
            var model = new OfflinePaymentInfo
            {
                PaymentMethodId = _faker.PickRandom(_modelFaker.AllAdminLookups.PaymentMethods.Where(z => z.Code != "CH" && z.Code != "MO" && z.Code != "P" && z.Code != "OB")).Id,
                Amount = 95.00M,
                Notes = string.Empty
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
