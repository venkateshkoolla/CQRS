using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class TranscriptRequestTests : BaseTest
    {
        [Fact]
        public async Task GetTranscriptRequest_ShouldPass()
        {
            Contact applicant = null;
            Education education = null;
            Application application = null;
            TranscriptRequest transcriptRequest = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate());

                var educationBase = DataFakerFixture.Models.EducationBase.Generate("default, Canadian, Ontario, Highschool");
                educationBase.ApplicantId = applicant.Id;
                educationBase.ModifiedBy = applicant.Username;
                education = await Context.CreateEducation(educationBase);

                var applicationBase = DataFakerFixture.Models.ApplicationBase.Generate();
                applicationBase.ApplicantId = applicant.Id;
                applicationBase.ModifiedBy = applicant.Username;
                application = await Context.CreateApplication(applicationBase);

                var transcriptRequestBase = DataFakerFixture.Models.TranscriptRequestBase.Generate();
                transcriptRequestBase.ApplicantId = applicant.Id;
                transcriptRequestBase.ApplicationId = application.Id;
                transcriptRequestBase.EducationId = education.Id;
                transcriptRequestBase.ModifiedBy = applicant.Username;

                transcriptRequest = await Context.CreateTranscriptRequest(transcriptRequestBase);

                //Act
                var transcriptRequestRetrieved = await Context.GetTranscriptRequest(transcriptRequest.Id);

                //Assert
                transcriptRequestRetrieved.Should().BeEquivalentTo(transcriptRequest);
            }
            finally
            {
                // Cleanup
                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (education?.Id != null)
                    await Context.DeleteEducation(education.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);

                if (transcriptRequest?.Id != null)
                    await Context.DeleteTranscriptRequest(transcriptRequest);
            }
        }

        [Fact]
        public async Task GetTranscriptRequests_ShouldPass_When_ApplicantId()
        {
            Contact applicant = null;
            IList<Education> educations = new List<Education>();
            IList<Application> applications = new List<Application>();
            IList<TranscriptRequest> transcriptRequests = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate());

                var applicationCycleActiveStatus = DataFakerFixture.SeedData.ApplicationCycleStatuses.Single(x => x.Code == ((char)ApplicationCycleStatusCode.Active).ToString());
                var applicationCycles = DataFakerFixture.SeedData.ApplicationCycles.Where(a => a.StatusId == applicationCycleActiveStatus.Id).ToList();

                const int transcriptRequestCount = 2;
                var transcriptRequestBases = new List<TranscriptRequestBase>();
                foreach (var applicationCycle in applicationCycles)
                {
                    var applicationBase = DataFakerFixture.Models.ApplicationBase.Generate();
                    applicationBase.ApplicantId = applicant.Id;
                    applicationBase.ModifiedBy = applicant.Username;
                    applicationBase.ApplicationCycleId = applicationCycle.Id;
                    var application = await Context.CreateApplication(applicationBase);
                    applications.Add(application);

                    foreach (var transcriptRequestBase in DataFakerFixture.Models.TranscriptRequestBase.Generate(transcriptRequestCount))
                    {
                        var educationBase = DataFakerFixture.Models.EducationBase.Generate("default, Canadian, Ontario, Highschool");
                        educationBase.ApplicantId = applicant.Id;
                        educationBase.ModifiedBy = applicant.Username;
                        var education = await Context.CreateEducation(educationBase);
                        educations.Add(education);

                        transcriptRequestBase.ApplicantId = applicant.Id;
                        transcriptRequestBase.ApplicationId = application.Id;
                        transcriptRequestBase.EducationId = education.Id;
                        transcriptRequestBase.ModifiedBy = applicant.Username;
                        transcriptRequestBases.Add(transcriptRequestBase);
                        await Context.CreateTranscriptRequest(transcriptRequestBase);
                    }
                }

                //Act
                var options = new GetTranscriptRequestOptions { ApplicantId = applicant.Id };
                transcriptRequests = await Context.GetTranscriptRequests(options);

                //Assert
                transcriptRequests.Should().NotBeNullOrEmpty()
                    .And.HaveCount(transcriptRequestCount * applicationCycles.Count())
                    .And.OnlyContain(t => t.ApplicantId == applicant.Id);
                transcriptRequests.Should().BeEquivalentTo(transcriptRequestBases, opts => opts
                    .Excluding(t => t.FromSchoolName)
                    .Excluding(t => t.CreatedOn)
                    .Excluding(t => t.ModifiedOn))
                    .And.OnlyHaveUniqueItems(x => x.Id);
            }
            finally
            {
                // Cleanup
                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (applications.Any())
                {
                    foreach (var application in applications)
                    {
                        await Context.DeleteApplication(application.Id);
                    }
                }

                if (educations.Any())
                {
                    foreach (var education in educations)
                    {
                        await Context.DeleteEducation(education.Id);
                    }
                }

                if (transcriptRequests.Any())
                {
                    foreach (var transcriptRequest in transcriptRequests)
                    {
                        await Context.DeleteTranscriptRequest(transcriptRequest.Id);
                    }
                }
            }
        }

        [Fact]
        public async Task GetTranscriptRequests_ShouldPass_When_ApplicationId()
        {
            Contact applicant = null;
            IList<Education> educations = new List<Education>();
            Application application = null;
            IList<TranscriptRequest> transcriptRequests = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate());

                var applicationBase = DataFakerFixture.Models.ApplicationBase.Generate();
                applicationBase.ApplicantId = applicant.Id;
                applicationBase.ModifiedBy = applicant.Username;
                application = await Context.CreateApplication(applicationBase);

                const int transcriptRequestCount = 3;
                var transcriptRequestBases = DataFakerFixture.Models.TranscriptRequestBase.Generate(transcriptRequestCount);
                foreach (var transcriptRequestBase in transcriptRequestBases)
                {
                    var educationBase = DataFakerFixture.Models.EducationBase.Generate("default, Canadian, Ontario, Highschool");
                    educationBase.ApplicantId = applicant.Id;
                    educationBase.ModifiedBy = applicant.Username;
                    var education = await Context.CreateEducation(educationBase);
                    educations.Add(education);

                    transcriptRequestBase.ApplicantId = applicant.Id;
                    transcriptRequestBase.ApplicationId = application.Id;
                    transcriptRequestBase.EducationId = education.Id;
                    transcriptRequestBase.ModifiedBy = applicant.Username;
                    await Context.CreateTranscriptRequest(transcriptRequestBase);
                }

                //Act
                var options = new GetTranscriptRequestOptions { ApplicationId = application.Id };
                transcriptRequests = await Context.GetTranscriptRequests(options);

                //Assert
                transcriptRequests.Should().NotBeNullOrEmpty()
                    .And.HaveCount(transcriptRequestCount)
                    .And.OnlyContain(t => t.ApplicationId == application.Id);
                transcriptRequests.Should().BeEquivalentTo(transcriptRequestBases, opts => opts
                    .Excluding(t => t.FromSchoolName)
                    .Excluding(t => t.CreatedOn)
                    .Excluding(t => t.ModifiedOn))
                    .And.OnlyHaveUniqueItems(x => x.Id);
            }
            finally
            {
                // Cleanup
                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);

                if (educations.Any())
                {
                    foreach (var education in educations)
                    {
                        await Context.DeleteEducation(education.Id);
                    }
                }

                if (transcriptRequests.Any())
                {
                    foreach (var transcriptRequest in transcriptRequests)
                    {
                        await Context.DeleteTranscriptRequest(transcriptRequest.Id);
                    }
                }
            }
        }

        [Fact]
        public async Task GetTranscriptRequests_ShouldPass_When_EducationId()
        {
            Contact applicant = null;
            Education education = null;
            Application application = null;
            TranscriptRequest transcriptRequest = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate());

                var applicationCycleActiveStatus = DataFakerFixture.SeedData.ApplicationCycleStatuses.Single(x => x.Code == ((char)ApplicationCycleStatusCode.Active).ToString());
                var applicationCycle = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ApplicationCycles.Where(a => a.StatusId == applicationCycleActiveStatus.Id));

                var transcriptRequestBases = new List<TranscriptRequestBase>();
                var applicationBase = DataFakerFixture.Models.ApplicationBase.Generate();
                applicationBase.ApplicantId = applicant.Id;
                applicationBase.ModifiedBy = applicant.Username;
                applicationBase.ApplicationCycleId = applicationCycle.Id;
                application = await Context.CreateApplication(applicationBase);

                var educationBase = DataFakerFixture.Models.EducationBase.Generate("default, Canadian, Ontario, Highschool");
                educationBase.ApplicantId = applicant.Id;
                educationBase.ModifiedBy = applicant.Username;
                education = await Context.CreateEducation(educationBase);

                var transcriptRequestBase = DataFakerFixture.Models.TranscriptRequestBase.Generate();
                transcriptRequestBase.ApplicantId = applicant.Id;
                transcriptRequestBase.ApplicationId = application.Id;
                transcriptRequestBase.EducationId = education.Id;
                transcriptRequestBase.ModifiedBy = applicant.Username;
                transcriptRequestBases.Add(transcriptRequestBase);
                transcriptRequest = await Context.CreateTranscriptRequest(transcriptRequestBase);

                //Act
                var options = new GetTranscriptRequestOptions { EducationId = education.Id };
                var transcriptRequests = await Context.GetTranscriptRequests(options);

                //Assert
                transcriptRequests.Should().NotBeNullOrEmpty()
                    .And.ContainSingle()
                    .And.OnlyContain(t => t.EducationId == education.Id);
                var transcriptRequestRetrieved = transcriptRequests.First();
                transcriptRequestRetrieved.Should().BeEquivalentTo(transcriptRequest);
            }
            finally
            {
                // Cleanup
                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);

                if (education?.Id != null)
                    await Context.DeleteEducation(education.Id);

                if (transcriptRequest?.Id != null)
                    await Context.DeleteTranscriptRequest(transcriptRequest.Id);
            }
        }

        [Fact]
        public async Task CreateTranscriptRequest_ShouldPass_College()
        {
            Contact applicant = null;
            Education education = null;
            Application application = null;
            TranscriptRequest transcriptRequest = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate());

                var educationBase = DataFakerFixture.Models.EducationBase.Generate("default, Canadian, Ontario, College");
                educationBase.ApplicantId = applicant.Id;
                educationBase.ModifiedBy = applicant.Username;
                education = await Context.CreateEducation(educationBase);

                var applicationBase = DataFakerFixture.Models.ApplicationBase.Generate();
                applicationBase.ApplicantId = applicant.Id;
                applicationBase.ModifiedBy = applicant.Username;
                application = await Context.CreateApplication(applicationBase);

                var transcriptRequestBase = DataFakerFixture.Models.TranscriptRequestBase.Generate("default, College");
                transcriptRequestBase.ApplicantId = applicant.Id;
                transcriptRequestBase.ApplicationId = application.Id;
                transcriptRequestBase.EducationId = education.Id;
                transcriptRequestBase.ModifiedBy = applicant.Username;

                //Act
                transcriptRequest = await Context.CreateTranscriptRequest(transcriptRequestBase);

                //Assert
                CheckTranscriptRequestFields(transcriptRequest, transcriptRequestBase);
            }
            finally
            {
                // Cleanup
                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (education?.Id != null)
                    await Context.DeleteEducation(education.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);

                if (transcriptRequest?.Id != null)
                    await Context.DeleteTranscriptRequest(transcriptRequest);
            }
        }

        [Fact]
        public async Task CreateTranscriptRequest_ShouldPass_Highschool()
        {
            Contact applicant = null;
            Education education = null;
            Application application = null;
            TranscriptRequest transcriptRequest = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate());

                var educationBase = DataFakerFixture.Models.EducationBase.Generate("default, Canadian, Ontario, Highschool");
                educationBase.ApplicantId = applicant.Id;
                educationBase.ModifiedBy = applicant.Username;
                education = await Context.CreateEducation(educationBase);

                var applicationBase = DataFakerFixture.Models.ApplicationBase.Generate();
                applicationBase.ApplicantId = applicant.Id;
                applicationBase.ModifiedBy = applicant.Username;
                application = await Context.CreateApplication(applicationBase);

                var transcriptRequestBase = DataFakerFixture.Models.TranscriptRequestBase.Generate("default, Highschool");
                transcriptRequestBase.ApplicantId = applicant.Id;
                transcriptRequestBase.ApplicationId = application.Id;
                transcriptRequestBase.EducationId = education.Id;
                transcriptRequestBase.ModifiedBy = applicant.Username;

                //Act
                transcriptRequest = await Context.CreateTranscriptRequest(transcriptRequestBase);

                //Assert
                CheckTranscriptRequestFields(transcriptRequest, transcriptRequestBase);
            }
            finally
            {
                // Cleanup
                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (education?.Id != null)
                    await Context.DeleteEducation(education.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);

                if (transcriptRequest?.Id != null)
                    await Context.DeleteTranscriptRequest(transcriptRequest);
            }
        }

        [Fact]
        public async Task CreateTranscriptRequest_ShouldPass_University()
        {
            Contact applicant = null;
            Education education = null;
            Application application = null;
            TranscriptRequest transcriptRequest = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate());

                var educationBase = DataFakerFixture.Models.EducationBase.Generate("default, Canadian, Ontario, University");
                educationBase.ApplicantId = applicant.Id;
                educationBase.ModifiedBy = applicant.Username;
                education = await Context.CreateEducation(educationBase);

                var applicationBase = DataFakerFixture.Models.ApplicationBase.Generate();
                applicationBase.ApplicantId = applicant.Id;
                applicationBase.ModifiedBy = applicant.Username;
                application = await Context.CreateApplication(applicationBase);

                var transcriptRequestBase = DataFakerFixture.Models.TranscriptRequestBase.Generate("default, University");
                transcriptRequestBase.ApplicantId = applicant.Id;
                transcriptRequestBase.ApplicationId = application.Id;
                transcriptRequestBase.EducationId = education.Id;
                transcriptRequestBase.ModifiedBy = applicant.Username;

                //Act
                transcriptRequest = await Context.CreateTranscriptRequest(transcriptRequestBase);

                //Assert
                CheckTranscriptRequestFields(transcriptRequest, transcriptRequestBase);
            }
            finally
            {
                // Cleanup
                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (education?.Id != null)
                    await Context.DeleteEducation(education.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);

                if (transcriptRequest?.Id != null)
                    await Context.DeleteTranscriptRequest(transcriptRequest);
            }
        }

        [Fact]
        public async Task UpdateTranscriptRequest_ShouldPass()
        {
            Contact applicant = null;
            Education education = null;
            Application application = null;
            TranscriptRequest transcriptRequest = null;
            TranscriptRequest transcriptRequestUpdated = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate());

                var educationBase = DataFakerFixture.Models.EducationBase.Generate("default, Canadian, Ontario, Highschool");
                educationBase.ApplicantId = applicant.Id;
                educationBase.ModifiedBy = applicant.Username;
                education = await Context.CreateEducation(educationBase);

                var applicationBase = DataFakerFixture.Models.ApplicationBase.Generate();
                applicationBase.ApplicantId = applicant.Id;
                applicationBase.ModifiedBy = applicant.Username;
                application = await Context.CreateApplication(applicationBase);

                var transcriptRequestBase = DataFakerFixture.Models.TranscriptRequestBase.Generate();
                transcriptRequestBase.ApplicantId = applicant.Id;
                transcriptRequestBase.ApplicationId = application.Id;
                transcriptRequestBase.EducationId = education.Id;
                transcriptRequestBase.ModifiedBy = applicant.Username;
                transcriptRequest = await Context.CreateTranscriptRequest(transcriptRequestBase);

                //Act
                var transcriptRequestUpdates = DataFakerFixture.Models.TranscriptRequest.Generate();
                transcriptRequestUpdates.ApplicantId = applicant.Id;
                transcriptRequestUpdates.ApplicationId = application.Id;
                transcriptRequestUpdates.EducationId = education.Id;
                transcriptRequestUpdates.ModifiedBy = applicant.Username;
                transcriptRequestUpdates.Id = transcriptRequest.Id;

                transcriptRequestUpdated = await Context.UpdateTranscriptRequest(transcriptRequestUpdates);

                //Assert
                CheckTranscriptRequestFields(transcriptRequestUpdated, transcriptRequestUpdates);
                transcriptRequestUpdated.Id.Should().Be(transcriptRequest.Id);
            }
            finally
            {
                // Cleanup
                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (education?.Id != null)
                    await Context.DeleteEducation(education.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);

                if (transcriptRequest?.Id != null)
                    await Context.DeleteTranscriptRequest(transcriptRequest);
            }
        }

        [Fact]
        public async Task DeleteTranscriptRequest_ShouldPass_WhenId()
        {
            Contact applicant = null;
            Education education = null;
            Application application = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate());

                var educationBase = DataFakerFixture.Models.EducationBase.Generate("default, Canadian, Ontario, Highschool");
                educationBase.ApplicantId = applicant.Id;
                educationBase.ModifiedBy = applicant.Username;
                education = await Context.CreateEducation(educationBase);

                var applicationBase = DataFakerFixture.Models.ApplicationBase.Generate();
                applicationBase.ApplicantId = applicant.Id;
                applicationBase.ModifiedBy = applicant.Username;
                application = await Context.CreateApplication(applicationBase);

                var transcriptRequestBase = DataFakerFixture.Models.TranscriptRequestBase.Generate();
                transcriptRequestBase.ApplicantId = applicant.Id;
                transcriptRequestBase.ApplicationId = application.Id;
                transcriptRequestBase.EducationId = education.Id;
                transcriptRequestBase.ModifiedBy = applicant.Username;
                var transcriptRequest = await Context.CreateTranscriptRequest(transcriptRequestBase);

                //Act
                await Context.DeleteTranscriptRequest(transcriptRequest.Id);

                //Assert
                var transcriptRequestDelete = await Context.GetTranscriptRequest(transcriptRequest.Id);
                transcriptRequestDelete.Should().BeNull();
            }
            finally
            {
                // Cleanup
                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (education?.Id != null)
                    await Context.DeleteEducation(education.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);
            }
        }

        [Fact]
        public async Task DeleteTranscriptRequest_ShouldPass_WhenObject()
        {
            Contact applicant = null;
            Education education = null;
            Application application = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate());

                var educationBase = DataFakerFixture.Models.EducationBase.Generate("default, Canadian, Ontario, Highschool");
                educationBase.ApplicantId = applicant.Id;
                educationBase.ModifiedBy = applicant.Username;
                education = await Context.CreateEducation(educationBase);

                var applicationBase = DataFakerFixture.Models.ApplicationBase.Generate();
                applicationBase.ApplicantId = applicant.Id;
                applicationBase.ModifiedBy = applicant.Username;
                application = await Context.CreateApplication(applicationBase);

                var transcriptRequestBase = DataFakerFixture.Models.TranscriptRequestBase.Generate();
                transcriptRequestBase.ApplicantId = applicant.Id;
                transcriptRequestBase.ApplicationId = application.Id;
                transcriptRequestBase.EducationId = education.Id;
                transcriptRequestBase.ModifiedBy = applicant.Username;
                var transcriptRequest = await Context.CreateTranscriptRequest(transcriptRequestBase);

                //Act
                await Context.DeleteTranscriptRequest(transcriptRequest);

                //Assert
                var transcriptRequestDelete = await Context.GetTranscriptRequest(transcriptRequest.Id);
                transcriptRequestDelete.Should().BeNull();
            }
            finally
            {
                // Cleanup
                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (education?.Id != null)
                    await Context.DeleteEducation(education.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);
            }
        }

        private void CheckTranscriptRequestFields(TranscriptRequest transcriptRequest, TranscriptRequestBase transcriptRequestBase)
        {
            transcriptRequest.Id.Should().NotBeEmpty();
            transcriptRequest.ApplicantId.Should().Be(transcriptRequestBase.ApplicantId);
            transcriptRequest.ApplicationId.Should().Be(transcriptRequestBase.ApplicationId);
            transcriptRequest.EducationId.Should().Be(transcriptRequestBase.EducationId);
            transcriptRequest.EtmsTranscriptRequestId.Should().Be(transcriptRequestBase.EtmsTranscriptRequestId);
            transcriptRequest.FromSchoolId.Should().Be(transcriptRequestBase.FromSchoolId);
            transcriptRequest.FromSchoolName.Should().NotBeEmpty();
            transcriptRequest.FromSchoolType.Should().Be(transcriptRequestBase.FromSchoolType);
            transcriptRequest.Name.Should().Be(transcriptRequestBase.Name);
            transcriptRequest.PeteRequestLogId.Should().Be(transcriptRequestBase.PeteRequestLogId);
            transcriptRequest.ToSchoolId.Should().Be(transcriptRequestBase.ToSchoolId);
            transcriptRequest.ToSchoolName.Should().BeNullOrEmpty();
            transcriptRequest.TranscriptTransmissionId.Should().Be(transcriptRequestBase.TranscriptTransmissionId);
            transcriptRequest.TranscriptRequestStatusId.Should().Be(transcriptRequestBase.TranscriptRequestStatusId);
            transcriptRequest.TranscriptFee.Should().Be(transcriptRequestBase.TranscriptFee);
        }
    }
}
