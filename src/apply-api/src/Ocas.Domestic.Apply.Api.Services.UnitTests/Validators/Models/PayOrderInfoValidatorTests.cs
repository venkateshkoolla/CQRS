using System;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Services.Validators.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Models
{
    public class PayOrderInfoValidatorTests
    {
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly Faker _faker;

        public PayOrderInfoValidatorTests()
        {
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task PayOrderInfoValidator_ShouldPass_When_HostedTokenization()
        {
            // Arrange
            var model = _modelFakerFixture.GetPayOrderInfo().Generate(Constants.Payment.RuleSet.HostedTokenization);

            // Act
            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(new Dto.Order { FinalTotal = 95 });

            var validator = new PayOrderInfoValidator(domesticContext.Object, Guid.NewGuid());
            var result = await validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task PayOrderInfoValidator_ShouldPass_When_ZeroDollar()
        {
            // Arrange
            var model = _modelFakerFixture.GetPayOrderInfo().Generate(Constants.Payment.RuleSet.ZeroDollar);

            // Act
            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(new Dto.Order { FinalTotal = 0 });

            var validator = new PayOrderInfoValidator(domesticContext.Object, Guid.NewGuid());
            var result = await validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task PayOrderInfoValidator_ShouldFail_WhenMissing()
        {
            // Arrange
            var model = new PayOrderInfo();

            // Act
            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(new Dto.Order { FinalTotal = 95 });

            var validator = new PayOrderInfoValidator(domesticContext.Object, Guid.NewGuid());
            var result = await validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(4);
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Card Holder Name' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Card Number Token' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Csc' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Expiry Date' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task PayOrderInfoValidator_ShouldFail_When_ZeroDollar_Then_PayOrderInfo()
        {
            // Arrange
            var model = _modelFakerFixture.GetPayOrderInfo().Generate(Constants.Payment.RuleSet.HostedTokenization);

            // Act
            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(new Dto.Order { FinalTotal = 0 });

            var validator = new PayOrderInfoValidator(domesticContext.Object, Guid.NewGuid());
            var result = await validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "PayOrderInfo must be empty for zero dollar order");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task PayOrderInfoValidator_ShouldFail_WhenCscNaN()
        {
            // Arrange
            var model = _modelFakerFixture.GetPayOrderInfo().Generate(Constants.Payment.RuleSet.HostedTokenization);
            model.Csc = "ABCD";

            // Act
            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(new Dto.Order { FinalTotal = 95 });

            var validator = new PayOrderInfoValidator(domesticContext.Object, Guid.NewGuid());
            var result = await validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Csc' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task PayOrderInfoValidator_ShouldFail_WhenCscTooShort()
        {
            // Arrange
            var model = _modelFakerFixture.GetPayOrderInfo().Generate(Constants.Payment.RuleSet.HostedTokenization);
            model.Csc = "01";

            // Act
            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(new Dto.Order { FinalTotal = 95 });

            var validator = new PayOrderInfoValidator(domesticContext.Object, Guid.NewGuid());
            var result = await validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"The length of 'Csc' must be at least 3 characters. You entered {model.Csc.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task PayOrderInfoValidator_ShouldFail_WhenCscTooLong()
        {
            // Arrange
            var model = _modelFakerFixture.GetPayOrderInfo().Generate(Constants.Payment.RuleSet.HostedTokenization);
            model.Csc = "01234";

            // Act
            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(new Dto.Order { FinalTotal = 95 });

            var validator = new PayOrderInfoValidator(domesticContext.Object, Guid.NewGuid());
            var result = await validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"The length of 'Csc' must be 4 characters or fewer. You entered {model.Csc.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task PayOrderInfoValidator_ShouldFail_WhenExpiryNaN()
        {
            // Arrange
            var model = _modelFakerFixture.GetPayOrderInfo().Generate(Constants.Payment.RuleSet.HostedTokenization);
            model.ExpiryDate = "ABCD";

            // Act
            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(new Dto.Order { FinalTotal = 95 });

            var validator = new PayOrderInfoValidator(domesticContext.Object, Guid.NewGuid());
            var result = await validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Expiry Date' is not a date: {model.ExpiryDate}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task PayOrderInfoValidator_ShouldFail_WhenExpiryDateTooLong()
        {
            // Arrange
            var model = _modelFakerFixture.GetPayOrderInfo().Generate(Constants.Payment.RuleSet.HostedTokenization);
            model.ExpiryDate = _faker.Random.String(Constants.DateFormat.CcExpiry.Length + 1);

            // Act
            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(new Dto.Order { FinalTotal = 95 });

            var validator = new PayOrderInfoValidator(domesticContext.Object, Guid.NewGuid());
            var result = await validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Expiry Date' must be {Constants.DateFormat.CcExpiry.Length} characters in length. You entered {model.ExpiryDate.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task PayOrderInfoValidator_ShouldFail_WhenCardHolderTooLong()
        {
            // Arrange
            var model = _modelFakerFixture.GetPayOrderInfo().Generate(Constants.Payment.RuleSet.HostedTokenization);
            model.CardHolderName = _faker.Random.String(101);

            // Act
            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(new Dto.Order { FinalTotal = 95 });

            var validator = new PayOrderInfoValidator(domesticContext.Object, Guid.NewGuid());
            var result = await validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"The length of 'Card Holder Name' must be 40 characters or fewer. You entered {model.CardHolderName.Length} characters.");
        }
    }
}
