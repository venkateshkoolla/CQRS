using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public class ApplicationsControllerTests : BaseTest
    {
        private readonly IdentityUserFixture _identityUserFixture;
        private readonly AdminTestFramework.ModelFakerFixture _modelFakerFixture;
        private readonly Faker _fakerFixture;
        private readonly CrmDatabaseFixture _crmDatabaseFixture;

        public ApplicationsControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.DataFakerFixture, XunitInjectionCollection.IdentityUserFixture, XunitInjectionCollection.ModelFakerFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _fakerFixture = XunitInjectionCollection.DataFakerFixture.Faker;
            _crmDatabaseFixture = XunitInjectionCollection.CrmDatabaseFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateApplicationNumber_ShouldPass()
        {
            // Arrange
            var applicant = await ApplyClientFixture.CreateNewApplicant();
            var applicationBase = _modelFakerFixture.GetApplicationBase().Generate();
            applicationBase.ApplicantId = applicant.Id;
            var before = await ApplyApiClient.CreateApplication(applicationBase);
            var applicationCycle = _modelFakerFixture.AllAdminLookups.ApplicationCycles.Single(x => x.Id == applicationBase.ApplicationCycleId);

            var number = await _crmDatabaseFixture.CreateApplicationNumber(applicationCycle.Year);
            var applicationInfo = new ApplicationNumberUpdateInfo
            {
                Number = number
            };

            // Act
            Client.WithAccessToken(_identityUserFixture.OcasBoUser.AccessToken);
            var after = await Client.UpdateApplicationNumber(before.Id, applicationInfo);

            // Assert
            after.Should().NotBeNull();
            after.Should().BeEquivalentTo(before, opts => opts
            .Excluding(x => x.ModifiedOn)
            .Excluding(x => x.ModifiedBy)
            .Excluding(x => x.ApplicationNumber));
            after.ApplicationNumber.Should().Be(number);
            after.ModifiedOn.Should().BeOnOrAfter(before.ModifiedOn);
            after.ModifiedBy.Should().Be(TestConstants.Identity.Providers.OcasAdfs.QaBoUsername);
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateApplicationEffectiveDate_ShouldPass()
        {
            // Arrange
            var applicant = await ApplyClientFixture.CreateNewApplicant();
            var applicationBase = _modelFakerFixture.GetApplicationBase().Generate();
            applicationBase.ApplicantId = applicant.Id;
            var before = await ApplyApiClient.CreateApplication(applicationBase);

            var applicationInfo = new ApplicationEffectiveDateUpdateInfo
            {
                EffectiveDate = _fakerFixture.Date.Past().AsUtc().ToStringOrDefault()
            };

            // Act
            Client.WithAccessToken(_identityUserFixture.OcasBoUser.AccessToken);
            var after = await Client.UpdateApplicationEffectiveDate(before.Id, applicationInfo);

            // Assert
            after.Should().NotBeNull();
            after.Should().BeEquivalentTo(before, opts => opts
            .Excluding(x => x.ModifiedOn)
            .Excluding(x => x.ModifiedBy)
            .Excluding(x => x.EffectiveDate));
            after.EffectiveDate.Should().Be(applicationInfo.EffectiveDate);
            after.ModifiedOn.Should().BeOnOrAfter(before.ModifiedOn);
            after.ModifiedBy.Should().Be(TestConstants.Identity.Providers.OcasAdfs.QaBoUsername);
        }

        [Fact]
        [IntegrationTest]
        public async Task OfflinePayment_ShouldPass()
        {
            // Arrange
            var testApplicant = await ApplyClientFixture.CreateNewApplicant();
            await ApplyClientFixture.CreateEducation(testApplicant.Id, Apply.Core.Enums.EducationType.CanadianHighSchool, true);
            var application = await ApplyClientFixture.CreateApplication(testApplicant.Id);
            await ApplyClientFixture.CreateProgramChoices(application, 1);
            await ApplyApiClient.CompleteTranscripts(application.Id);

            var offlinePaymentInfo = new OfflinePaymentInfo
            {
                PaymentMethodId = _fakerFixture.PickRandom(_modelFakerFixture.AllAdminLookups.PaymentMethods).Id,
                Amount = 95M,
                Notes = _fakerFixture.Random.String2(25)
            };
            Client.WithAccessToken(_identityUserFixture.OcasBoUser.AccessToken);

            // Act
            var applicationSummaries = await Client.PayOrder(application.Id, offlinePaymentInfo);

            // Assert
            applicationSummaries.Should().ContainSingle();

            var applicationSummary = applicationSummaries.First();
            applicationSummary.Application.Should().NotBeNull();
            applicationSummary.Application.Id.Should().Be(application.Id);
            applicationSummary.Application.ApplicantId.Should().Be(testApplicant.Id);

            applicationSummary.FinancialTransactions.Should().ContainSingle();
            var financialTransaction = applicationSummary.FinancialTransactions.First();

            financialTransaction.ApplicantId.Should().Be(testApplicant.Id);
            financialTransaction.TransactionType.Should().Be(Apply.Enums.TransactionType.Payment);
            financialTransaction.PaymentType.Should().Be(Apply.Enums.OrderPaymentType.Offline);
            financialTransaction.PaymentAmount.Should().Be(offlinePaymentInfo.Amount);

            applicationSummary.ShoppingCartDetails.Should().BeEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task OfflinePayment_ShouldPass_With_Partial_Payment()
        {
            // Arrange
            var testApplicant = await ApplyClientFixture.CreateNewApplicant();
            await ApplyClientFixture.CreateEducation(testApplicant.Id, Apply.Core.Enums.EducationType.CanadianHighSchool, true);
            var application = await ApplyClientFixture.CreateApplication(testApplicant.Id);
            await ApplyClientFixture.CreateProgramChoices(application, 1);
            await ApplyApiClient.CompleteTranscripts(application.Id);

            Client.WithAccessToken(_identityUserFixture.OcasBoUser.AccessToken);
            var offlinePaymentInfo = new OfflinePaymentInfo
            {
                PaymentMethodId = _fakerFixture.PickRandom(_modelFakerFixture.AllAdminLookups.PaymentMethods).Id,
                Amount = 25M,
                Notes = _fakerFixture.Random.String2(25)
            };

            // Act
            var applicationSummaries = await Client.PayOrder(application.Id, offlinePaymentInfo);

            // Assert
            applicationSummaries.Should().ContainSingle();

            var applicationSummary = applicationSummaries.First();
            applicationSummary.Application.Should().NotBeNull();
            applicationSummary.Application.Id.Should().Be(application.Id);
            applicationSummary.Application.ApplicantId.Should().Be(testApplicant.Id);

            applicationSummary.FinancialTransactions.Should().ContainSingle();
            var financialTransaction = applicationSummary.FinancialTransactions.First();

            financialTransaction.ApplicantId.Should().Be(testApplicant.Id);
            financialTransaction.TransactionType.Should().Be(Apply.Enums.TransactionType.Deposit);
            financialTransaction.PaymentType.Should().Be(Apply.Enums.OrderPaymentType.Offline);
            financialTransaction.PaymentAmount.Should().Be(offlinePaymentInfo.Amount);

            applicationSummary.ShoppingCartDetails.Should().HaveCount(3)
                .And.Contain(c => c.Type == Apply.Enums.ShoppingCartItemType.AccountCredit && c.Amount == -offlinePaymentInfo.Amount);
        }

        [Fact]
        [IntegrationTest]
        public async Task OfflinePayment_ShouldPass_With_Zero_Dollar_Order()
        {
            // Arrange
            var testApplicant = await ApplyClientFixture.CreateNewApplicant();
            await ApplyClientFixture.CreateEducation(testApplicant.Id, Apply.Core.Enums.EducationType.CanadianHighSchool, true);
            var application = await ApplyClientFixture.CreateApplication(testApplicant.Id);
            await ApplyClientFixture.CreateProgramChoices(application, 1);
            await ApplyApiClient.CompleteTranscripts(application.Id);

            Client.WithAccessToken(_identityUserFixture.OcasBoUser.AccessToken);

            // Pay order in full
            await Client.PayOrder(application.Id, new OfflinePaymentInfo { PaymentMethodId = _fakerFixture.PickRandom(_modelFakerFixture.AllAdminLookups.PaymentMethods).Id, Amount = 95M, Notes = _fakerFixture.Random.String2(25) });

            var offlinePaymentInfo = new OfflinePaymentInfo
            {
                PaymentMethodId = _fakerFixture.PickRandom(_modelFakerFixture.AllAdminLookups.PaymentMethods).Id,
                Amount = 25M,
                Notes = _fakerFixture.Random.String2(25)
            };

            // Act
            var applicationSummaries = await Client.PayOrder(application.Id, offlinePaymentInfo);

            // Assert
            applicationSummaries.Should().ContainSingle();

            var applicationSummary = applicationSummaries.First();
            applicationSummary.Application.Should().NotBeNull();
            applicationSummary.Application.Id.Should().Be(application.Id);
            applicationSummary.Application.ApplicantId.Should().Be(testApplicant.Id);

            applicationSummary.FinancialTransactions.Should().HaveCount(2);
            applicationSummary.FinancialTransactions.Should().ContainSingle(f =>
                f.ApplicantId == testApplicant.Id
                && f.TransactionType == Apply.Enums.TransactionType.Deposit
                && f.PaymentType == Apply.Enums.OrderPaymentType.Offline
                && f.PaymentAmount == offlinePaymentInfo.Amount);

            applicationSummary.ShoppingCartDetails.Should().ContainSingle()
                .And.Contain(c => c.Type == Apply.Enums.ShoppingCartItemType.AccountCredit && c.Amount == -offlinePaymentInfo.Amount);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetCollegeTransmissionHistories_ShouldPass()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            // Act
            var collegeTransmissionHistories = await Client.GetCollegeTransmissionHistories(TestConstants.Application.ApplicationIdWithCollegeTransmissions, new GetCollegeTransmissionHistoryOptions());

            // Assert
            collegeTransmissionHistories.Items.Should().NotBeNullOrEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task GetCollegeTransmissionHistories_ShouldPass_With_Filters()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            // Act
            var collegeTransmissionHistories = await Client.GetCollegeTransmissionHistories(TestConstants.Application.ApplicationIdWithCollegeTransmissions, new GetCollegeTransmissionHistoryOptions { FromDate = "2019-10-09", ToDate = "2019-10-10" });

            // Assert
            collegeTransmissionHistories.Items.Should().NotBeNullOrEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task GetOfferHistories_ShouldPass()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            // Act
            var offerHistories = await Client.GetOfferHistories(TestConstants.Application.ApplicationIdWithOffers);

            // Assert
            offerHistories.Should().NotBeNullOrEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateProgramChoice_ShouldPass()
        {
            // Arrange
            var testApplicant = await ApplyClientFixture.CreateNewApplicant();
            await ApplyClientFixture.CreateEducation(testApplicant.Id, Apply.Core.Enums.EducationType.CanadianHighSchool, true);
            var application = await ApplyClientFixture.CreateApplication(testApplicant.Id);

            var programIntakes = await AlgoliaClient.GetProgramOfferings(application.ApplicationCycleId);
            var programIntake = programIntakes.First();

            var request = new CreateProgramChoiceRequest
            {
                ProgramId = programIntake.ProgramId,
                ApplicationId = application.Id,
                EntryLevelId = programIntake.ProgramValidEntryLevelIds?.Any() != true
                ? programIntake.ProgramEntryLevelId
                : _fakerFixture.PickRandom(programIntake.ProgramValidEntryLevelIds),
                StartDate = programIntake.IntakeStartDate
            };

            Client.WithAccessToken(_identityUserFixture.OcasBoUser.AccessToken);

            // Act
            var applicationSummary = await Client.CreateProgramChoice(application.Id, request);

            // Assert
            applicationSummary.Application.Should().NotBeNull();
            applicationSummary.Application.Id.Should().Be(application.Id);
            applicationSummary.Application.ApplicantId.Should().Be(testApplicant.Id);

            applicationSummary.ProgramChoices.Should().ContainSingle()
                .And.Contain(p => p.ProgramId == request.ProgramId && p.EntryLevelId == request.EntryLevelId);
        }
    }
}
