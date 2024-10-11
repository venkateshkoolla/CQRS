using System.Collections.Generic;
using System.Linq;
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
    public class OcasUserTests : BaseTest<ApplyApiClient>
    {
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly Faker _fakerFixture;
        private readonly CrmDatabaseFixture _crmDatabaseFixture;
        private readonly IdentityUserFixture _identityUserFixture;

        public OcasUserTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _fakerFixture = XunitInjectionCollection.DataFakerFixture.Faker;
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
            _crmDatabaseFixture = XunitInjectionCollection.CrmDatabaseFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task OcasUser_SmokeTest_ShouldPass()
        {
            // Arrange
            var testApplicant = await CreateNewApplicantAsOcasUser();
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
                .And.ContainSingle(d => d.Type == Enums.ShoppingCartItemType.ApplicationFee)
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

        private async Task<TestApplicantInfo> CreateNewApplicantAsOcasUser()
        {
            // applicant & profile
            var applicant = await ClientFixture.CreateNewApplicant();

            // switch to OCAS user
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            var currentApplicant = await Client.GetApplicant(applicant.Id);
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
