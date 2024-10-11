using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.Core.Enums;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class SupportingDocumentsControllerTests : BaseTest<ApplyApiClient>
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly MonerisFixture _monerisFixture;
        private readonly EtmsFixture _etmsFixture;
        private readonly CrmDatabaseFixture _crmDatabaseFixture;

        public SupportingDocumentsControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _monerisFixture = XunitInjectionCollection.MonerisFixture;
            _etmsFixture = XunitInjectionCollection.EtmsFixture;
            _crmDatabaseFixture = XunitInjectionCollection.CrmDatabaseFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task GetSupportingDocumentFile_ShouldPass()
        {
            // Arrange
            var testUser = await IdentityUserFixture.GetApplicantUser(TestConstants.Identity.Providers.OcasApplicants.ApplicantWithSupportingDocuments, TestConstants.Identity.Providers.OcasApplicants.ApplicantWithSupportingDocumentsPw);
            Client.WithAccessToken(testUser.AccessToken);
            var applicant = await Client.GetCurrentApplicant();
            var supportingDocuments = await Client.GetSupportingDocuments(applicant.Id);
            var supportingDocumentId = supportingDocuments.First(x => x.Type == Enums.SupportingDocumentType.Other && !x.Processing).Id;

            // Act
            var supportingDocument = await Client.GetSupportingDocumentFile(supportingDocumentId);

            // Assert
            supportingDocument.Should().NotBeNull();
            supportingDocument.Data.Should().NotBeNull();
            supportingDocument.Name.Should().NotBeNull();
            supportingDocument.MimeType.Should().NotBeNull();
        }

        [Fact]
        [IntegrationTest]
        public async Task GetSupportingDocuments_ShouldPass()
        {
            // Arrange
            var testUser = await IdentityUserFixture.GetApplicantUser(TestConstants.Identity.Providers.OcasApplicants.ApplicantWithSupportingDocuments, TestConstants.Identity.Providers.OcasApplicants.ApplicantWithSupportingDocumentsPw);
            Client.WithAccessToken(testUser.AccessToken);
            var applicant = await Client.GetCurrentApplicant();

            // Act
            var supportingDocuments = await Client.GetSupportingDocuments(applicant.Id);

            // Assert
            supportingDocuments.Should().NotBeNullOrEmpty();
        }

        [Fact(Skip = "No fileshare")]
        [IntegrationTest]
        public async Task GetSupportingDocumentTranscript_ShouldPass()
        {
            // Arrange
            var reissueId = _modelFakerFixture.AllApplyLookups.TranscriptRequestStatuses.Single(x => x.Code == Constants.TranscriptRequestStatuses.RequestReissue).Id;
            var transcriptNotFoundId = _modelFakerFixture.AllApplyLookups.TranscriptRequestStatuses.Single(x => x.Code == Constants.TranscriptRequestStatuses.TranscriptNotFound).Id;
            var currentApplicant = await ClientFixture.CreateNewApplicant();

            // create education from Georgian college because only Georgian is set up to receive eTMS transcripts in DEV environment
            var education = await ClientFixture.CreateEducation(currentApplicant.Id, EducationType.CanadianCollege, true);
            var georgianCollege = _modelFakerFixture.AllApplyLookups.Colleges.Single(h => h.Code == "GEOR");
            var instituteFromId = georgianCollege.Id;
            if (education.InstituteId.Value != instituteFromId)
            {
                education.InstituteId = instituteFromId;
                await Client.UpdateEducation(education);
            }
            var application = await ClientFixture.CreateApplication(currentApplicant.Id);

            // Choose program
            var programChoices = new List<ProgramChoiceBase>();
            var programIntakes = await AlgoliaClient.GetProgramOfferings(application.ApplicationCycleId, 1, new List<Guid> { education.InstituteId.Value });
            foreach (var programIntake in programIntakes)
            {
                var programChoice = _modelFakerFixture.GetProgramChoiceBase().Generate();

                programChoice.IntakeId = programIntake.IntakeId;
                programChoice.EntryLevelId = programIntake.ProgramValidEntryLevelIds?.Any() != true
                    ? programIntake.ProgramEntryLevelId
                    : _dataFakerFixture.Faker.PickRandom(programIntake.ProgramValidEntryLevelIds);

                programChoices.Add(programChoice);
            }
            await Client.PutProgramChoices(application.Id, programChoices);
            await Client.CompletePrograms(application.Id);

            // Create a TR
            var destinations = programIntakes.Select(x => x.CollegeId).Distinct();
            var transcriptRequests = new List<TranscriptRequest>();
            foreach (var destination in destinations)
            {
                var transcriptRequestBase = _modelFakerFixture.GetTranscriptRequestBase().Generate(1, "default, College");
                transcriptRequestBase.ForEach(x => x.ApplicationId = application.Id);
                transcriptRequestBase.First().FromInstituteId = education.InstituteId.Value;
                transcriptRequestBase.First().ToInstituteId = destination;

                var transcriptRequest = await Client.CreateTranscriptRequests(transcriptRequestBase);
                transcriptRequests.Concat(transcriptRequest);
            }

            await Client.CompleteTranscripts(application.Id);

            await Client.GetShoppingCart(application.Id);
            var order = await Client.CreateOrder(new CreateOrderInfo { ApplicationId = application.Id });
            var token = await _monerisFixture.GetPaymentToken(TestConstants.Moneris.MasterCard);
            var payOrderInfo = new PayOrderInfo
            {
                CardHolderName = $"{currentApplicant.FirstName} {currentApplicant.LastName}",
                CardNumberToken = token,
                Csc = "123",
                ExpiryDate = DateTime.UtcNow.AddMonths(1).ToString(Constants.DateFormat.CcExpiry)
            };
            await Client.PayOrder(order.Id, payOrderInfo);

            // wait for async process to create and link eTMS record
            System.Threading.Thread.Sleep(60000);

            // Upload etms transcript
            var referenceNumber = await _crmDatabaseFixture.GetTranscriptRequestReferenceNumber(application.ApplicationNumber);
            referenceNumber.Should().NotBeNullOrEmpty();
            await _etmsFixture.FulfillTranscriptRequest(referenceNumber, application.ApplicationNumber, currentApplicant.BirthDate);

            // wait for biztalk to create transcript
            System.Threading.Thread.Sleep(60000);

            // Act
            var supportingDocuments = await Client.GetSupportingDocuments(currentApplicant.Id);

            // Assert
            supportingDocuments.Should().NotBeNullOrEmpty();
            supportingDocuments.Should().ContainSingle();
            supportingDocuments.Should().OnlyContain(x => x.Type == Enums.SupportingDocumentType.Transcript);
            supportingDocuments.Should().OnlyContain(x => !x.Processing);
            supportingDocuments.Should().OnlyContain(x => x.Name == georgianCollege.Name);
        }
    }
}
