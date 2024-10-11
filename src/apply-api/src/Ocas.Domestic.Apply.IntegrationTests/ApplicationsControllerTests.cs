using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class ApplicationsControllerTests : BaseTest<ApplyApiClient>
    {
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly CrmDatabaseFixture _crmDatabaseFixture;

        public ApplicationsControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _crmDatabaseFixture = XunitInjectionCollection.CrmDatabaseFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task GetApplications_ShouldPass_When_ZeroApplications()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();

            // Act
            var result = await Client.GetApplications(currentApplicant.Id);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task GetApplications_ShouldPass_When_Applications()
        {
            // Arrange
            var applicationNewApplyStatusId = _modelFakerFixture.AllApplyLookups.ApplicationStatuses.First(s => s.Code == Constants.ApplicationStatuses.NewApply).Id;
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var currentApplication = await ClientFixture.CreateApplication(currentApplicant.Id);

            // Act
            var result = await Client.GetApplications(currentApplicant.Id);

            // Assert
            result.Should().ContainSingle();
            ApplicationShouldBe(result[0]);
            ApplicationBaseShouldBe(result[0], currentApplication, currentApplicant, applicationNewApplyStatusId);
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateApplication_ShouldPass()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var applicationNewApplyStatusId = _modelFakerFixture.AllApplyLookups.ApplicationStatuses.First(s => s.Code == Constants.ApplicationStatuses.NewApply).Id;
            var applicationBase = _modelFakerFixture.GetApplicationBase().Generate();
            applicationBase.ApplicantId = currentApplicant.Id;

            // Act
            var result = await Client.CreateApplication(applicationBase);

            // Assert
            ApplicationShouldBe(result);
            ApplicationBaseShouldBe(result, applicationBase, currentApplicant, applicationNewApplyStatusId);
        }

        [Fact]
        [IntegrationTest]
        public async Task CompleteProgramsTranscripts_ShouldPass()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var currentApplication = await ClientFixture.CreateApplication(currentApplicant.Id);
            await ClientFixture.CreateProgramChoices(currentApplication);

            // Act
            await Client.CompletePrograms(currentApplication.Id);
            var programsCompleted = await Client.GetApplications(currentApplicant.Id);

            await Client.CompleteTranscripts(currentApplication.Id);
            var transcriptsCompleted = await Client.GetApplications(currentApplicant.Id);

            // Assert
            currentApplication.ProgramsComplete.Should().BeFalse();
            currentApplication.TranscriptsComplete.Should().BeFalse();
            programsCompleted.Should().ContainSingle();
            programsCompleted.First().ProgramsComplete.Should().BeTrue();
            programsCompleted.First().TranscriptsComplete.Should().BeFalse();
            transcriptsCompleted.Should().ContainSingle();
            transcriptsCompleted.First().ProgramsComplete.Should().BeTrue();
            transcriptsCompleted.First().TranscriptsComplete.Should().BeTrue();
        }

        [Fact]
        [IntegrationTest]
        public async Task ClaimAndRemoveVoucher_ShouldPass()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var currentApplication = await ClientFixture.CreateApplication(currentApplicant.Id);
            var currentChoices = await ClientFixture.CreateProgramChoices(currentApplication);
            var shoppingCart = await Client.GetShoppingCart(currentApplication.Id);
            var order = await Client.CreateOrder(new CreateOrderInfo { ApplicationId = currentApplication.Id });

            var voucherCode = await _crmDatabaseFixture.CreateVoucher(currentChoices.First().CollegeId.Value, currentApplication.ApplicationCycleId);

            // Act
            await Client.ApplyVoucher(currentApplication.Id, voucherCode);
            var appliedShoppingCart = await Client.GetShoppingCart(currentApplication.Id);
            var appliedOrder = await Client.CreateOrder(new CreateOrderInfo { ApplicationId = currentApplication.Id });

            await Client.RemoveVoucher(currentApplication.Id, voucherCode);
            var removedShoppingCart = await Client.GetShoppingCart(currentApplication.Id);
            var removedOrder = await Client.CreateOrder(new CreateOrderInfo { ApplicationId = currentApplication.Id });

            // Assert
            shoppingCart.Should().NotBeNullOrEmpty()
                .And.HaveCount(2)
                .And.ContainSingle(d => d.Type == Enums.ShoppingCartItemType.ApplicationFee)
                .And.ContainSingle(d => d.Type == Enums.ShoppingCartItemType.ProgramChoice);
            order.Should().NotBeNull();
            order.Amount.Should().Be(shoppingCart.Sum(x => x.Amount));

            // voucher applied
            appliedShoppingCart.Should().NotBeNullOrEmpty()
                .And.HaveCount(3)
                .And.ContainSingle(d => d.Type == Enums.ShoppingCartItemType.ApplicationFee)
                .And.ContainSingle(d => d.Type == Enums.ShoppingCartItemType.ProgramChoice)
                .And.ContainSingle(d => d.Type == Enums.ShoppingCartItemType.Voucher);
            appliedShoppingCart.Sum(x => x.Amount).Should().Be(0);
            appliedOrder.Should().NotBeNull();
            appliedOrder.Amount.Should().Be(0);

            // voucher removed
            removedShoppingCart.Should().BeEquivalentTo(shoppingCart);
            removedOrder.Should().NotBeNull();
            removedOrder.Amount.Should().Be(removedShoppingCart.Sum(x => x.Amount));
        }

        private static void ApplicationBaseShouldBe(Application actual, ApplicationBase expected, Applicant expectedApplicant, Guid expectedApplicationStatusId)
        {
            actual.ApplicantId.Should().Be(expectedApplicant.Id);
            actual.ApplicationCycleId.Should().Be(expected.ApplicationCycleId);
            actual.ApplicationStatusId.Should().Be(expectedApplicationStatusId);
            actual.EffectiveDate.Should().Be(DateTime.UtcNow.ToStringOrDefault());
            actual.ModifiedBy.Should().Be(expectedApplicant.Email);
        }

        private static void ApplicationShouldBe(Application actual)
        {
            actual.Id.Should().NotBeEmpty();
            actual.ApplicationNumber.Should().NotBeEmpty();
            actual.ModifiedOn.Should().BeOnOrBefore(DateTime.UtcNow);
        }
    }
}
