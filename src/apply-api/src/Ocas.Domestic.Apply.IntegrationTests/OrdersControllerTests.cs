using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.Core.Enums;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Apply.TestFramework.Models;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class OrdersControllerTests : BaseTest<ApplyApiClient>
    {
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly Faker _fakerFixture;
        private readonly MonerisFixture _monerisFixture;
        private readonly CrmDatabaseFixture _crmDatabaseFixture;

        public OrdersControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _fakerFixture = XunitInjectionCollection.DataFakerFixture.Faker;
            _monerisFixture = XunitInjectionCollection.MonerisFixture;
            _crmDatabaseFixture = XunitInjectionCollection.CrmDatabaseFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task MonerisTokenFetch_ShouldPass()
        {
            // Arrange & Act
            var token = await _monerisFixture.GetPaymentToken(TestConstants.Moneris.MasterCard);

            // Assert
            token.Should().NotBeNullOrEmpty();
            token.Should().StartWith("ot-");
        }

        [Fact]
        [IntegrationTest]
        public async Task MonerisPayment_ShouldPass()
        {
            // Arrange
            var token = await _monerisFixture.GetPaymentToken(TestConstants.Moneris.MasterCard);
            var testApplicant = await CreateTestApplicant();
            var payOrderInfo = new PayOrderInfo
            {
                CardHolderName = $"{testApplicant.FirstName} {testApplicant.LastName}",
                CardNumberToken = token,
                Csc = "123",
                ExpiryDate = DateTime.UtcNow.AddMonths(1).ToString(Constants.DateFormat.CcExpiry)
            };
            var applicantId = testApplicant.Id;
            var applicationId = testApplicant.Application.Id;

            // Act
            var preShoppingCart = await Client.GetShoppingCart(applicationId);
            var preOrder = await Client.CreateOrder(new CreateOrderInfo { ApplicationId = applicationId });

            var paymentTransaction = await Client.PayOrder(preOrder.Id, payOrderInfo);

            var postShoppingCart = await Client.GetShoppingCart(applicationId);
            var transactions = await Client.GetFinancialTransactions(applicationId);

            // Assert
            token.Should().NotBeNullOrEmpty();
            token.Should().StartWith("ot-");
            preShoppingCart.Should().NotBeNullOrEmpty()
                .And.HaveCount(2)
                .And.ContainSingle(d => d.Type == Enums.ShoppingCartItemType.ApplicationFee)
                .And.ContainSingle(d => d.Type == Enums.ShoppingCartItemType.ProgramChoice);
            preOrder.Should().NotBeNull();
            preOrder.Amount.Should().Be(preShoppingCart.Sum(x => x.Amount));

            paymentTransaction.ApplicantId.Should().Be(applicantId);
            paymentTransaction.Receipt.Should().NotBeNull();
            paymentTransaction.Receipt.Cardholder.Should().Be(payOrderInfo.CardHolderName);
            paymentTransaction.Receipt.ResponseOrderId.Should().Be(preOrder.Number);
            paymentTransaction.Receipt.ChargeTotal.Should().Be(preOrder.Amount);
            paymentTransaction.OrderId.Should().Be(preOrder.Id);
            paymentTransaction.TransactionAmount.Should().Be(preOrder.Amount);
            paymentTransaction.TransactionType.Should().Be(Enums.TransactionType.Payment);

            postShoppingCart.Should().BeEmpty();

            transactions.Should().ContainSingle();
            var transaction = transactions.First();
            transaction.ApplicantId.Should().Be(applicantId);
            transaction.Receipt.Should().NotBeNull();
            transaction.Receipt.Cardholder.Should().Be(payOrderInfo.CardHolderName);
            transaction.Receipt.ResponseOrderId.Should().Be(preOrder.Number);
            transaction.Receipt.ChargeTotal.Should().Be(preOrder.Amount);
            transaction.OrderId.Should().Be(preOrder.Id);
            transaction.TransactionAmount.Should().Be(preOrder.Amount);
            transaction.TransactionType.Should().Be(Enums.TransactionType.Payment);
        }

        [Fact]
        [IntegrationTest]
        public async Task PayOrder_ShouldThrow_WhenOrderCancelled()
        {
            // Arrange
            var token = await _monerisFixture.GetPaymentToken(TestConstants.Moneris.Visa); // Visa required to trigger declined response
            var testApplicant = await CreateTestApplicant();
            var payOrderInfo = new PayOrderInfo
            {
                CardHolderName = $"{testApplicant.FirstName} {testApplicant.LastName}",
                CardNumberToken = token,
                Csc = "123",
                ExpiryDate = DateTime.UtcNow.AddMonths(1).ToString(Constants.DateFormat.CcExpiry)
            };
            var applicantId = testApplicant.Id;
            var applicationId = testApplicant.Application.Id;
            var preShoppingCart = await Client.GetShoppingCart(applicationId);
            var adjustmentNeeded = TestConstants.Moneris.AmountForDeclinedResponse - preShoppingCart.Sum(x => x.Amount);

            await _crmDatabaseFixture.UpdateApplicantBalance(applicantId, adjustmentNeeded);

            var order = await Client.CreateOrder(new CreateOrderInfo { ApplicationId = applicationId });

            // Act
            StatusCodeException statusCodeException = null;
            try
            {
                await Client.PayOrder(order.Id, payOrderInfo);
            }
            catch (StatusCodeException e)
            {
                // the card should be declined
                statusCodeException = e;
            }

            // second attempt should fail because order is cancelled
            Func<Task> action = () => Client.PayOrder(order.Id, payOrderInfo);

            // Assert
            token.Should().NotBeNullOrEmpty();
            token.Should().StartWith("ot-");
            order.Should().NotBeNull();
            order.Amount.Should().Be(TestConstants.Moneris.AmountForDeclinedResponse);

            statusCodeException.Should().NotBeNull();
            statusCodeException.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            action.Should().Throw<StatusCodeException>()
                .Where(x => x.StatusCode == HttpStatusCode.BadRequest)
                .Where(x => x.Message.Contains($"Order is cancelled: {order.Id}"));
        }

        [Fact]
        [IntegrationTest]
        public async Task GetOrder_ShouldPass()
        {
            // Arrange
            var applicant = await ClientFixture.CreateNewApplicant();
            var application = await ClientFixture.CreateApplication(applicant.Id);
            await ClientFixture.CreateProgramChoices(application);
            await Client.CompleteTranscripts(application.Id);
            await Client.GetShoppingCart(application.Id);
            var newOrder = await Client.CreateOrder(new CreateOrderInfo { ApplicationId = application.Id });

            // Act
            var order = await Client.GetOrder(newOrder.Id);

            // Assert
            order.Should().NotBeNull();
            order.Should().BeEquivalentTo(newOrder);
        }

        [Fact]
        [IntegrationTest]
        public async Task ZeroDollarPayment_ShouldPass()
        {
            // Arrange
            var testApplicant = await CreateTestApplicant();
            var applicantId = testApplicant.Id;
            var applicationId = testApplicant.Application.Id;

            var voucherCode = await _crmDatabaseFixture.CreateVoucher(
                testApplicant.Application.ProgramChoices[0].CollegeId,
                testApplicant.Application.ProgramChoices[0].ApplicationCycleId);

            // Act
            var preShoppingCart = await Client.GetShoppingCart(applicationId);
            var preOrder = await Client.CreateOrder(new CreateOrderInfo { ApplicationId = applicationId });

            await Client.ApplyVoucher(applicationId, voucherCode);
            var postShoppingCart = await Client.GetShoppingCart(applicationId);
            var postOrder = await Client.CreateOrder(new CreateOrderInfo { ApplicationId = applicationId });

            var paymentTransaction = await Client.PayOrder(postOrder.Id);
            var transactions = await Client.GetFinancialTransactions(applicationId);

            // Assert
            preShoppingCart.Should().NotBeNullOrEmpty()
                .And.HaveCount(2)
                .And.ContainSingle(d => d.Type == Enums.ShoppingCartItemType.ProgramChoice);
            preOrder.Should().NotBeNull();
            preOrder.Amount.Should().Be(preShoppingCart.Sum(x => x.Amount));

            postShoppingCart.Should().NotBeNullOrEmpty()
                .And.HaveCount(3)
                .And.ContainSingle(d => d.Type == Enums.ShoppingCartItemType.ApplicationFee)
                .And.ContainSingle(d => d.Type == Enums.ShoppingCartItemType.ProgramChoice);
            postShoppingCart.Sum(x => x.Amount).Should().Be(0M);
            postOrder.Should().NotBeNull();
            postOrder.Amount.Should().Be(0M);

            paymentTransaction.ApplicantId.Should().Be(applicantId);
            paymentTransaction.Receipt.Should().BeNull();
            paymentTransaction.OrderId.Should().Be(postOrder.Id);
            paymentTransaction.TransactionAmount.Should().BeNull();
            paymentTransaction.TransactionType.Should().Be(Enums.TransactionType.Payment);

            transactions.Should().ContainSingle();
            var transaction = transactions.First();
            transaction.ApplicantId.Should().Be(applicantId);
            transaction.Receipt.Should().BeNull();
            transaction.OrderId.Should().Be(postOrder.Id);
            transaction.TransactionAmount.Should().BeNull();
            transaction.TransactionType.Should().Be(Enums.TransactionType.Payment);
        }

        private async Task<TestApplicantInfo> CreateTestApplicant()
        {
            // applicant & profile
            var applicant = await ClientFixture.CreateNewApplicant();
            applicant = await ClientFixture.CreateProfile(applicant);

            // education status
            await ClientFixture.CreateEducationStatus(applicant.Id);

            // education
            await ClientFixture.CreateEducation(applicant.Id, EducationType.International);

            // application
            var application = await ClientFixture.CreateApplication(applicant.Id);

            // program choices
            var programChoices = _modelFakerFixture.GetProgramChoiceBase().Generate(1);
            var programIntakes = await AlgoliaClient.GetProgramOfferings(application.ApplicationCycleId);
            programChoices[0].IntakeId = programIntakes[0].IntakeId;
            programChoices[0].EntryLevelId = programIntakes[0].ProgramValidEntryLevelIds?.Any() != true
                ? programIntakes[0].ProgramEntryLevelId
                : _fakerFixture.PickRandom(programIntakes[0].ProgramValidEntryLevelIds);
            await Client.PutProgramChoices(application.Id, programChoices);

            // completed steps
            await Client.CompleteTranscripts(application.Id);

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Applicant, TestApplicantInfo>()
                    .ForMember(x => x.Application, opts => opts.Ignore());

                cfg.CreateMap<Application, TestApplicationInfo>()
                    .ForMember(x => x.AlternateProgramChoice, opts => opts.Ignore())
                    .ForMember(x => x.Offers, opts => opts.Ignore())
                    .ForMember(x => x.ProgramChoices, opts => opts.Ignore());
            }).CreateMapper();

            var testApplicantInfo = mapper.Map<Applicant, TestApplicantInfo>(applicant);
            testApplicantInfo.Application = mapper.Map<Application, TestApplicationInfo>(application);

            testApplicantInfo.Application.ProgramChoices = new List<AlgoliaProgramIntake>
            {
                programIntakes[0]
            };

            return testApplicantInfo;
        }
    }
}
