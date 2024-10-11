using System;
using System.Linq;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class FinancialTransactionTests : BaseTest
    {
        [Fact]
        public async Task GetFinancialTransaction_ShouldPass_WhenPaymentAndVoucher()
        {
            var financialTransactions = await Context.GetFinancialTransactions(new GetFinancialTransactionOptions
            {
                Id = TestConstants.FinancialTransactions.PaymentWithVoucher
            });

            var ft = financialTransactions.Single();

            ft.Should().NotBeNull();
            ft.TransactionType.Should().Be(TransactionType.Payment);
            ft.OrderNumber.Should().Be("ORD-231034-J3C7C9");
            ft.PaymentType.Should().Be(OrderPaymentType.Online);
            ft.Receipt.Should().BeNull();
            ft.PaymentMethodId.Should().Be(DataFakerFixture.SeedData.PaymentMethods.Single(x => x.Code == TestConstants.PaymentMethods.Na).Id);
        }

        [Fact]
        public async Task GetFinancialTransaction_ShouldPass_WhenPaymentAndMoneris()
        {
            var financialTransactions = await Context.GetFinancialTransactions(new GetFinancialTransactionOptions
            {
                Id = TestConstants.FinancialTransactions.PaymentWithMoneris
            });

            var ft = financialTransactions.Single();

            ft.Should().NotBeNull();
            ft.TransactionType.Should().Be(TransactionType.Payment);
            ft.OrderNumber.Should().Be("ORD-231391-B5H8J7");
            ft.ChequeNumber.Should().Be("ORD-231391-B5H8J7");
            ft.PaymentType.Should().Be(OrderPaymentType.Online);
            ft.Receipt.Should().NotBeNull();
            ft.Name.Should().NotBeNullOrEmpty();
            ft.Receipt.Name.Should().NotBeNullOrEmpty();
            ft.Receipt.Name.Should().NotBe(ft.Name);
            ft.TransactionAmount.Should().Be(95.00M);
            ft.PaymentAmount.Should().Be(95.00M);
            ft.OrderAmount.Should().Be(95.00M);
            ft.PaymentMethodId.Should().Be(DataFakerFixture.SeedData.PaymentMethods.Single(x => x.Code == TestConstants.PaymentMethods.Mastercard).Id);
        }

        [Fact]
        public async Task GetFinancialTransaction_ShouldPass_WhenDeposit()
        {
            var financialTransactions = await Context.GetFinancialTransactions(new GetFinancialTransactionOptions
            {
                Id = TestConstants.FinancialTransactions.Deposit
            });

            var ft = financialTransactions.Single();

            ft.Should().NotBeNull();
            ft.TransactionType.Should().Be(TransactionType.Deposit);
            ft.OrderNumber.Should().BeNullOrEmpty();
            ft.TransactionApplicationNumber.Should().Be("180148275");
            ft.AccountBalanceAfterTransaction.Should().Be(222.00M);
            ft.TransactionAmount.Should().Be(222.00M);
            ft.PaymentMethodId.Should().Be(DataFakerFixture.SeedData.PaymentMethods.Single(x => x.Code == TestConstants.PaymentMethods.Visa).Id);
        }

        [Fact]
        public async Task GetFinancialTransaction_ShouldPass_WhenReturnedPayment()
        {
            var financialTransactions = await Context.GetFinancialTransactions(new GetFinancialTransactionOptions
            {
                Id = TestConstants.FinancialTransactions.ReturnedPayment
            });

            var ft = financialTransactions.Single();

            ft.Should().NotBeNull();
            ft.TransactionType.Should().Be(TransactionType.ReturnedPayment);
            ft.RelatedOrderNumber.Should().Be("ORD-191232-Z0R1Z8");
            ft.ReturnChargeApplied.Should().BeTrue();
            ft.ReturnedPaymentCharge.Should().Be(25.00M);
            ft.NsfBalanceAfterTransaction.Should().Be(25.00M);
            ft.ReturnedPayment.Should().Be(95.00M);
            ft.ReturnedPaymentAfterTransaction.Should().Be(95.00M);
        }

        [Fact]
        public async Task GetFinancialTransaction_ShouldPass_WhenReverseFullPayment()
        {
            var financialTransactions = await Context.GetFinancialTransactions(new GetFinancialTransactionOptions
            {
                Id = TestConstants.FinancialTransactions.ReverseFullPayment
            });

            var ft = financialTransactions.Single();

            ft.Should().NotBeNull();
            ft.TransactionType.Should().Be(TransactionType.ReverseFullPayment);
            ft.RelatedOrderNumber.Should().Be("ORD-191230-K5Q1M1");
            ft.Note.Should().Be("reversed to allow transfer");
            ft.TransactionAmount.Should().Be(95.00M);
            ft.PaymentAmount.Should().Be(95.00M);
        }

        [Fact]
        public async Task GetFinancialTransaction_ShouldPass_WhenRefund()
        {
            var financialTransactions = await Context.GetFinancialTransactions(new GetFinancialTransactionOptions
            {
                Id = TestConstants.FinancialTransactions.Refund
            });

            var ft = financialTransactions.Single();

            ft.Should().NotBeNull();
            ft.TransactionType.Should().Be(TransactionType.Refund);
            ft.OrderNumber.Should().Be("ORD-176716-J1L4T0");
            ft.PaymentMethodId.Should().Be(DataFakerFixture.SeedData.PaymentMethods.Single(x => x.Code == TestConstants.PaymentMethods.Cheque).Id);
            ft.OrderDetailId.Should().NotBeNull();
        }

        [Fact]
        public async Task GetFinancialTransaction_ShouldPass_WhenTransfer()
        {
            var financialTransactions = await Context.GetFinancialTransactions(new GetFinancialTransactionOptions
            {
                Id = TestConstants.FinancialTransactions.Transfer
            });

            var ft = financialTransactions.Single();

            ft.Should().NotBeNull();
            ft.TransactionType.Should().Be(TransactionType.Transfer);
            ft.Name.Should().Contain("ORD-234534-P4Q7X2");
            ft.ApplicantId.Should().Be(new Guid("FE72CDEA-00B4-4479-8CFA-56BC42DA3714"));
            ft.ApplicantFirstName.Should().Be("Test");
            ft.ApplicantLastName.Should().Be("test");
            ft.ApplicationApplicantId.Should().Be(new Guid("6CA87A89-DB89-4AD1-A92B-2F7479CD2380"));
            ft.ApplicationFirstName.Should().Be("Test");
            ft.ApplicationLastName.Should().Be("Test");
            ft.TransactionAmount.Should().Be(95.00M);
        }

        [Fact]
        public async Task GetFinancialTransaction_ShouldPass_WhenReleaseOverpayment()
        {
            var financialTransactions = await Context.GetFinancialTransactions(new GetFinancialTransactionOptions
            {
                Id = TestConstants.FinancialTransactions.ReleaseOverpayment
            });

            var ft = financialTransactions.Single();

            ft.Should().NotBeNull();
            ft.TransactionType.Should().Be(TransactionType.ReleaseOverpayment);
            ft.OrderNumber.Should().BeNullOrEmpty();
            ft.OverPaymentBalanceBeforeTransaction.Should().Be(85.00M);
            ft.PaymentMethodId.Should().Be(DataFakerFixture.SeedData.PaymentMethods.Single(x => x.Code == TestConstants.PaymentMethods.Cheque).Id);
        }

        [Fact]
        public async Task GetFinancialTransaction_ShouldPass_WhenReleaseFundonDeposit()
        {
            var financialTransactions = await Context.GetFinancialTransactions(new GetFinancialTransactionOptions
            {
                Id = TestConstants.FinancialTransactions.ReleaseFundonDeposit
            });

            var ft = financialTransactions.Single();

            ft.Should().NotBeNull();
            ft.TransactionType.Should().Be(TransactionType.ReleaseFundonDeposit);
            ft.OrderNumber.Should().BeNullOrEmpty();
            ft.AccountBalanceBeforeTransaction.Should().Be(20.00M);
            ft.PaymentMethodId.Should().Be(DataFakerFixture.SeedData.PaymentMethods.Single(x => x.Code == TestConstants.PaymentMethods.Cheque).Id);
        }
    }
}
