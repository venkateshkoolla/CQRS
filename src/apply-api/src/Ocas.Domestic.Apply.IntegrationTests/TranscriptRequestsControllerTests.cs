using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class TranscriptRequestsControllerTests : BaseTest<ApplyApiClient>
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ModelFakerFixture _modelFakerFixture;

        public TranscriptRequestsControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task DeleteTranscriptRequest_ShouldPass()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();

            var educationBase = _modelFakerFixture.GetEducationBase().Generate("default, Canadian, Ontario, Highschool");
            educationBase.ApplicantId = currentApplicant.Id;
            educationBase.InstituteId = _dataFakerFixture.Faker.PickRandom(_modelFakerFixture.AllApplyLookups.HighSchools.Where(h => h.HasEtms && h.TranscriptFee.HasValue && h.TranscriptFee > 0)).Id;
            var education = await Client.PostEducation(educationBase);

            var applicantBase = _modelFakerFixture.GetApplicationBase().Generate();
            applicantBase.ApplicantId = currentApplicant.Id;
            var application = await Client.CreateApplication(applicantBase);

            var transcriptRequestBase = _modelFakerFixture.GetTranscriptRequestBase().Generate(1, "default, Highschool");
            transcriptRequestBase.ForEach(x => x.ApplicationId = application.Id);
            transcriptRequestBase.First().FromInstituteId = education.InstituteId.Value;
            var transcriptRequests = await Client.CreateTranscriptRequests(transcriptRequestBase);

            // wait for async audit log process to log details of TR creation before we delete it
            await Task.Delay(60000);

            // Act
            await Client.DeleteTranscriptRequest(transcriptRequests.First().Id);

            // Assert
            var getTranscriptRequests = await Client.GetTranscriptRequests(application.Id);
            getTranscriptRequests.Should().BeEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task GetTranscriptRequests_ShouldPass()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();

            var educationBase = _modelFakerFixture.GetEducationBase().Generate("default, Canadian, Ontario, Highschool");
            educationBase.ApplicantId = currentApplicant.Id;
            educationBase.InstituteId = _dataFakerFixture.Faker.PickRandom(_modelFakerFixture.AllApplyLookups.HighSchools.Where(h => h.HasEtms && h.TranscriptFee.HasValue && h.TranscriptFee > 0)).Id;
            var education = await Client.PostEducation(educationBase);

            var applicantBase = _modelFakerFixture.GetApplicationBase().Generate();
            applicantBase.ApplicantId = currentApplicant.Id;
            var application = await Client.CreateApplication(applicantBase);

            var transcriptRequestBase = _modelFakerFixture.GetTranscriptRequestBase().Generate(1, "default, Highschool");
            transcriptRequestBase.ForEach(x => x.ApplicationId = application.Id);
            transcriptRequestBase.First().FromInstituteId = education.InstituteId.Value;
            var createdTranscriptRequests = await Client.CreateTranscriptRequests(transcriptRequestBase);

            // Act
            var transcriptRequests = await Client.GetTranscriptRequests(application.Id);

            // Assert
            var transmissionSendNowId = _modelFakerFixture.AllApplyLookups.TranscriptTransmissions.Single(x => x.Code == Constants.TranscriptTransmissions.SendTranscriptNow).Id;
            var requestStatusPaymentId = _modelFakerFixture.AllApplyLookups.TranscriptRequestStatuses.Single(x => x.Code == Constants.TranscriptRequestStatuses.WaitingPayment).Id;

            transcriptRequests.Should().NotBeNullOrEmpty()
                .And.ContainSingle()
                .And.OnlyContain(tr => tr.RequestStatusId == requestStatusPaymentId)
                .And.OnlyContain(tr => tr.TransmissionId == transmissionSendNowId && tr.ToInstituteId == null);
            transcriptRequests.Should().BeEquivalentTo(createdTranscriptRequests);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetTranscriptRequests_ShouldPass_WhenApplicantId()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();

            var educationBase = _modelFakerFixture.GetEducationBase().Generate("default, Canadian, Ontario, Highschool");
            educationBase.ApplicantId = currentApplicant.Id;
            educationBase.InstituteId = _dataFakerFixture.Faker.PickRandom(_modelFakerFixture.AllApplyLookups.HighSchools.Where(h => h.HasEtms && h.TranscriptFee.HasValue && h.TranscriptFee > 0)).Id;
            var education = await Client.PostEducation(educationBase);

            // create 2 applications
            var applicationBase = _modelFakerFixture.GetApplicationBase().Generate();
            applicationBase.ApplicantId = currentApplicant.Id;
            var application = await Client.CreateApplication(applicationBase);

            var activeStatusId = _dataFakerFixture.SeedData.ApplicationCycleStatuses.First(x => x.Code == Constants.ApplicationCycleStatuses.Active).Id;
            applicationBase.ApplicationCycleId = _dataFakerFixture.SeedData.ApplicationCycles.First(x => x.StatusId == activeStatusId && x.Id != applicationBase.ApplicationCycleId).Id;
            var application2 = await Client.CreateApplication(applicationBase);

            // create 2 TRs on separate applications
            var transcriptRequestBase = _modelFakerFixture.GetTranscriptRequestBase().Generate(1, "default, Highschool");
            transcriptRequestBase.ForEach(x => x.ApplicationId = application.Id);
            transcriptRequestBase.First().FromInstituteId = education.InstituteId.Value;
            var transcriptRequest = await Client.CreateTranscriptRequests(transcriptRequestBase);

            transcriptRequestBase.ForEach(x => x.ApplicationId = application2.Id);
            var transcriptRequest2 = await Client.CreateTranscriptRequests(transcriptRequestBase);

            // Act
            var transcriptRequests = await Client.GetTranscriptRequests(null, currentApplicant.Id);

            // Assert
            var transmissionSendNowId = _modelFakerFixture.AllApplyLookups.TranscriptTransmissions.Single(x => x.Code == Constants.TranscriptTransmissions.SendTranscriptNow).Id;
            var requestStatusPaymentId = _modelFakerFixture.AllApplyLookups.TranscriptRequestStatuses.Single(x => x.Code == Constants.TranscriptRequestStatuses.WaitingPayment).Id;

            transcriptRequests.Should().NotBeNullOrEmpty()
                .And.HaveCount(2)
                .And.OnlyContain(tr => tr.RequestStatusId == requestStatusPaymentId)
                .And.OnlyContain(tr => tr.TransmissionId == transmissionSendNowId && tr.ToInstituteId == null);

            var transcriptRequestFetch = transcriptRequests.Where(x => x.ApplicationId == application.Id).ToList();
            transcriptRequestFetch.Should().BeEquivalentTo(transcriptRequest);

            var transcriptRequestFetch2 = transcriptRequests.Where(x => x.ApplicationId == application2.Id).ToList();
            transcriptRequestFetch2.Should().BeEquivalentTo(transcriptRequest2);
        }
    }

    public class TranscriptRequestsControllerCollegeTests : BaseTest<ApplyApiClient>
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ModelFakerFixture _modelFakerFixture;

        public TranscriptRequestsControllerCollegeTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateTranscriptRequest_ShouldPass_When_College()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var education = await ClientFixture.CreateEducation(currentApplicant.Id, Core.Enums.EducationType.CanadianCollege, true);
            var instituteFromId = _dataFakerFixture.Faker
                .PickRandom(_modelFakerFixture.AllApplyLookups.Colleges.Where(h =>
                    h.HasEtms && h.TranscriptFee.HasValue && h.TranscriptFee > 0)).Id;
            if (education.InstituteId.Value != instituteFromId)
            {
                education.InstituteId = instituteFromId;
                await Client.UpdateEducation(education);
            }

            var application = await ClientFixture.CreateApplication(currentApplicant.Id);
            var choices = await ClientFixture.CreateProgramChoices(application, 1, new List<Guid> { education.InstituteId.Value });
            await Client.CompletePrograms(application.Id);
            var instituteToId = choices.Select(x => x.CollegeId).First();

            var transcriptRequestBase = _modelFakerFixture.GetTranscriptRequestBase().Generate(1, "default, College");
            transcriptRequestBase.ForEach(x => x.ApplicationId = application.Id);
            transcriptRequestBase.First().ToInstituteId = instituteToId;
            transcriptRequestBase.First().FromInstituteId = education.InstituteId.Value;

            // Act
            var transcriptRequest = await Client.CreateTranscriptRequests(transcriptRequestBase);

            // Assert
            var requestStatusPaymentId = _modelFakerFixture.AllApplyLookups.TranscriptRequestStatuses
                .Single(x => x.Code == Constants.TranscriptRequestStatuses.WaitingPayment).Id;

            transcriptRequest.Should().NotBeNull();
            transcriptRequest.First().RequestStatusId.Should().Be(requestStatusPaymentId);
            transcriptRequest.First().TransmissionId.Should().Be(transcriptRequestBase.First().TransmissionId);
        }
    }

    public class TranscriptRequestsControllerHsTests : BaseTest<ApplyApiClient>
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ModelFakerFixture _modelFakerFixture;

        public TranscriptRequestsControllerHsTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateTranscriptRequest_ShouldPass_When_Highschool()
        {
            // Arrange

            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var education = await ClientFixture.CreateEducation(currentApplicant.Id, Core.Enums.EducationType.CanadianHighSchool, true);
            var instituteFromId = _dataFakerFixture.Faker
                .PickRandom(_modelFakerFixture.AllApplyLookups.HighSchools.Where(h =>
                    h.HasEtms && h.TranscriptFee.HasValue && h.TranscriptFee > 0)).Id;
            if (education.InstituteId.Value != instituteFromId)
            {
                education.InstituteId = instituteFromId;
                await Client.UpdateEducation(education);
            }

            var application = await ClientFixture.CreateApplication(currentApplicant.Id);
            await ClientFixture.CreateProgramChoices(application);
            await Client.CompletePrograms(application.Id);

            var transcriptRequestBase = _modelFakerFixture.GetTranscriptRequestBase().Generate(1, "default, Highschool");
            transcriptRequestBase.ForEach(x => x.ApplicationId = application.Id);
            transcriptRequestBase.First().FromInstituteId = instituteFromId;

            // Act
            var transcriptRequest = await Client.CreateTranscriptRequests(transcriptRequestBase);

            // Assert
            var transmissionSendNowId = _modelFakerFixture.AllApplyLookups.TranscriptTransmissions
                .Single(x => x.Code == Constants.TranscriptTransmissions.SendTranscriptNow).Id;
            var requestStatusPaymentId = _modelFakerFixture.AllApplyLookups.TranscriptRequestStatuses
                .Single(x => x.Code == Constants.TranscriptRequestStatuses.WaitingPayment).Id;

            transcriptRequest.Should().NotBeNull();
            transcriptRequest.First().RequestStatusId.Should().Be(requestStatusPaymentId);
            transcriptRequest.First().TransmissionId.Should().Be(transmissionSendNowId);
        }
    }

    public class TranscriptRequestsControllerUniversityTests : BaseTest<ApplyApiClient>
    {
        private readonly ModelFakerFixture _modelFakerFixture;

        public TranscriptRequestsControllerUniversityTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateTranscriptRequest_ShouldPass_When_University()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var education = await ClientFixture.CreateEducation(currentApplicant.Id, Core.Enums.EducationType.CanadianUniversity, true);
            var application = await ClientFixture.CreateApplication(currentApplicant.Id);

            var choices = await ClientFixture.CreateProgramChoices(application);
            await Client.CompletePrograms(application.Id);
            var instituteToId = choices.Select(x => x.CollegeId).First();

            var transcriptRequestBase = _modelFakerFixture.GetTranscriptRequestBase().Generate(1, "default, University");
            transcriptRequestBase.ForEach(x => x.ApplicationId = application.Id);
            transcriptRequestBase.First().ToInstituteId = instituteToId;
            transcriptRequestBase.First().FromInstituteId = education.InstituteId.Value;

            // Act
            var transcriptRequest = await Client.CreateTranscriptRequests(transcriptRequestBase);

            // Assert
            var requestStatusPaymentId = _modelFakerFixture.AllApplyLookups.TranscriptRequestStatuses
                .Single(x => x.Code == Constants.TranscriptRequestStatuses.WaitingPayment).Id;

            transcriptRequest.Should().NotBeNull();
            transcriptRequest.First().RequestStatusId.Should().Be(requestStatusPaymentId);
            transcriptRequest.First().TransmissionId.Should().Be(transcriptRequestBase.First().TransmissionId);
        }
    }

    public class TranscriptRequestsControllerPaymentTests : BaseTest<ApplyApiClient>
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly MonerisFixture _monerisFixture;

        public TranscriptRequestsControllerPaymentTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _monerisFixture = XunitInjectionCollection.MonerisFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateTranscriptRequest_ShouldPass_When_ZeroPaid()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var education = await ClientFixture.CreateEducation(currentApplicant.Id, Core.Enums.EducationType.CanadianCollege, true);
            var instituteFromId = _dataFakerFixture.Faker
                .PickRandom(_modelFakerFixture.AllApplyLookups.Colleges.Where(h =>
                    h.HasEtms && h.TranscriptFee.HasValue && h.TranscriptFee == 0)).Id;
            if (education.InstituteId.Value != instituteFromId)
            {
                education.InstituteId = instituteFromId;
                await Client.UpdateEducation(education);
            }

            var application = await ClientFixture.CreateApplication(currentApplicant.Id);

            // Generate enough PCs so that we can guarantee that we are applying to multiple colleges
            var choices = await ClientFixture.CreateProgramChoices(application, 1, new List<Guid> { education.InstituteId.Value });
            await Client.CompletePrograms(application.Id);
            var instituteToId = choices.Select(x => x.CollegeId).First();

            var transcriptRequestBase = _modelFakerFixture.GetTranscriptRequestBase().Generate(1, "default, College");
            transcriptRequestBase.ForEach(x => x.ApplicationId = application.Id);
            transcriptRequestBase.First().FromInstituteId = education.InstituteId.Value;
            transcriptRequestBase.First().ToInstituteId = instituteToId;
            await Client.CreateTranscriptRequests(transcriptRequestBase);
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

            var transcriptRequestBaseAfterPayment =
                _modelFakerFixture.GetTranscriptRequestBase().Generate(1, "default, College");
            transcriptRequestBaseAfterPayment.ForEach(x => x.ApplicationId = application.Id);
            transcriptRequestBaseAfterPayment.First().FromInstituteId = education.InstituteId.Value;
            transcriptRequestBaseAfterPayment.First().ToInstituteId = instituteToId;
            await Client.CreateTranscriptRequests(transcriptRequestBaseAfterPayment);

            // Act
            var transcriptRequestAfterPayment = await Client.CreateTranscriptRequests(transcriptRequestBaseAfterPayment);

            // Assert
            var requestStatusPaymentId = _modelFakerFixture.AllApplyLookups.TranscriptRequestStatuses
                .Single(x => x.Code == Constants.TranscriptRequestStatuses.RequestInit).Id;

            transcriptRequestAfterPayment.Should().NotBeNull();
            transcriptRequestAfterPayment.First().RequestStatusId.Should().Be(requestStatusPaymentId);
            transcriptRequestAfterPayment.First().TransmissionId.Should().Be(transcriptRequestBaseAfterPayment.First().TransmissionId);
        }

        [Fact]
        [IntegrationTest]
        public async Task PayForTranscriptRequest_ShouldPass()
        {
            // Arrange
            var waitingPaymentApprovalStatusId = _modelFakerFixture.AllApplyLookups.TranscriptRequestStatuses.Single(x => x.Code == Constants.TranscriptRequestStatuses.WaitingPaymentApproval).Id;
            var inProgressStatusId = _modelFakerFixture.AllApplyLookups.TranscriptRequestStatuses.Single(x => x.Code == Constants.TranscriptRequestStatuses.RequestInit).Id;
            var currentApplicant = await ClientFixture.CreateNewApplicant();

            var educationBase = _modelFakerFixture.GetEducationBase().Generate("default, Canadian, Ontario, College");
            educationBase.ApplicantId = currentApplicant.Id;
            educationBase.InstituteId = _dataFakerFixture.Faker
                .PickRandom(_modelFakerFixture.AllApplyLookups.Colleges.Where(h =>
                    h.HasEtms && h.TranscriptFee.HasValue && h.TranscriptFee == 0)).Id;
            var education = await Client.PostEducation(educationBase);

            var applicantBase = _modelFakerFixture.GetApplicationBase().Generate();
            applicantBase.ApplicantId = currentApplicant.Id;
            var application = await Client.CreateApplication(applicantBase);

            // Generate enough PCs so that we can guarantee that we are applying to multiple colleges
            var choices = await ClientFixture.CreateProgramChoices(application, Constants.ProgramChoices.MaxTotalChoices, new List<Guid> { educationBase.InstituteId.Value });
            await Client.CompletePrograms(application.Id);

            // Create a TR for each destination college
            var destinations = choices.Select(x => x.CollegeId).Distinct();
            var transcriptRequests = new List<TranscriptRequest>();
            foreach (var destination in destinations)
            {
                var transcriptRequestBase =
                    _modelFakerFixture.GetTranscriptRequestBase().Generate(1, "default, College");
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

            // Act
            var transcriptRequestsAfterPayment = await Client.GetTranscriptRequests(application.Id);

            // Assert
            transcriptRequestsAfterPayment.Should().NotBeNullOrEmpty();
            transcriptRequestsAfterPayment.Should().HaveCountGreaterThan(0);
            transcriptRequestsAfterPayment.Should().OnlyContain(x =>
                x.RequestStatusId == waitingPaymentApprovalStatusId || x.RequestStatusId == inProgressStatusId);
        }
    }

    public class TranscriptRequestsControllerReissueTests : BaseTest<ApplyApiClient>
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly MonerisFixture _monerisFixture;
        private readonly CrmDatabaseFixture _crmDatabaseFixture;

        public TranscriptRequestsControllerReissueTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _monerisFixture = XunitInjectionCollection.MonerisFixture;
            _crmDatabaseFixture = XunitInjectionCollection.CrmDatabaseFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task ReissueTranscriptRequest_ShouldPass()
        {
            // Arrange
            var reissueId = _modelFakerFixture.AllApplyLookups.TranscriptRequestStatuses.Single(x => x.Code == Constants.TranscriptRequestStatuses.RequestReissue).Id;
            var transcriptNotFoundId = _modelFakerFixture.AllApplyLookups.TranscriptRequestStatuses.Single(x => x.Code == Constants.TranscriptRequestStatuses.TranscriptNotFound).Id;
            var currentApplicant = await ClientFixture.CreateNewApplicant();

            var education = await ClientFixture.CreateEducation(currentApplicant.Id, Core.Enums.EducationType.CanadianCollege, true);
            var instituteFromId = _dataFakerFixture.Faker
                .PickRandom(_modelFakerFixture.AllApplyLookups.Colleges.Where(h =>
                    h.HasEtms && h.TranscriptFee.HasValue && h.TranscriptFee == 0)).Id;
            if (education.InstituteId.Value != instituteFromId)
            {
                education.InstituteId = instituteFromId;
                await Client.UpdateEducation(education);
            }

            var application = await ClientFixture.CreateApplication(currentApplicant.Id);

            // Generate enough PCs so that we can guarantee that we are applying to multiple colleges
            var choices = await ClientFixture.CreateProgramChoices(application, Constants.ProgramChoices.MaxTotalChoices, new List<Guid> { education.InstituteId.Value });
            await Client.CompletePrograms(application.Id);

            // Create a TR for each destination college
            var transcriptRequestBases = new List<TranscriptRequestBase>();
            foreach (var destination in choices.Select(x => x.CollegeId).Distinct())
            {
                var transcriptRequestBase =
                    _modelFakerFixture.GetTranscriptRequestBase().Generate("default, College");
                transcriptRequestBase.ApplicationId = application.Id;
                transcriptRequestBase.FromInstituteId = education.InstituteId.Value;
                transcriptRequestBase.ToInstituteId = destination;

                transcriptRequestBases.Add(transcriptRequestBase);
            }
            var transcriptRequests = await Client.CreateTranscriptRequests(transcriptRequestBases);

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

            // wait for async process to create and link etms record
            await Task.Delay(60000);

            // Fake "Transcript Not Found" TR reponses
            foreach (var transcriptRequest in transcriptRequests)
            {
                await _crmDatabaseFixture.UpdateTranscriptRequestStatus(transcriptRequest.Id, transcriptNotFoundId);
            }

            // Act
            var transcriptRequestsAfterReissue = await Client.ReissueTranscriptRequest(transcriptRequests.First().Id);

            // Assert
            transcriptRequestsAfterReissue.Should().NotBeNullOrEmpty();
            transcriptRequestsAfterReissue.Should().HaveCountGreaterThan(0);
            transcriptRequestsAfterReissue.Should().OnlyContain(x => x.RequestStatusId == reissueId);
            transcriptRequestsAfterReissue.Should().OnlyContain(x =>
                x.EtmsTranscriptRequestId == transcriptRequestsAfterReissue.First().EtmsTranscriptRequestId);
        }
    }
}