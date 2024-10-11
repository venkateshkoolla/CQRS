using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.Models;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class FinancialTransactionsControllerTests : BaseTest<ApplyApiClient>
    {
        private readonly MonerisFixture _monerisFixture;

        public FinancialTransactionsControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _monerisFixture = XunitInjectionCollection.MonerisFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task GetFinancialTransactions_ShouldPass()
        {
            // Arrange
            var applicant = await ClientFixture.CreateNewApplicant();
            applicant = await ClientFixture.CreateProfile(applicant);
            await ClientFixture.CreateEducation(applicant.Id, Core.Enums.EducationType.International);
            var application = await ClientFixture.CreateApplication(applicant.Id);
            await ClientFixture.CreateProgramChoices(application);
            await Client.CompleteTranscripts(application.Id);

            await Client.GetShoppingCart(application.Id);
            var order = await Client.CreateOrder(new CreateOrderInfo { ApplicationId = application.Id });

            var token = await _monerisFixture.GetPaymentToken(TestConstants.Moneris.MasterCard);
            var payOrderInfo = new PayOrderInfo
            {
                CardHolderName = $"{applicant.FirstName} {applicant.LastName}",
                CardNumberToken = token,
                Csc = "123",
                ExpiryDate = DateTime.UtcNow.AddMonths(1).ToString(Constants.DateFormat.CcExpiry)
            };
            await Client.PayOrder(order.Id, payOrderInfo);

            // Act
            var transactions = await Client.GetFinancialTransactions(application.Id);

            // Assert
            transactions.Should().ContainSingle();
            var transaction = transactions.First();
            transaction.ApplicantId.Should().Be(applicant.Id);
            transaction.Receipt.Should().NotBeNull();
            transaction.Receipt.Cardholder.Should().Be(payOrderInfo.CardHolderName);
            transaction.Receipt.ResponseOrderId.Should().Be(order.Number);
            transaction.Receipt.ChargeTotal.Should().Be(order.Amount);
            transaction.OrderId.Should().Be(order.Id);
            transaction.TransactionAmount.Should().Be(order.Amount);
            transaction.TransactionType.Should().Be(Enums.TransactionType.Payment);
        }
    }
}
