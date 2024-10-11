using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.Core.Enums;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.TestFramework
{
    public class ApplyApiClientFixture
    {
        private readonly ApplyApiClient _applyApiClient;
        private readonly AlgoliaFixture _algoliaFixture;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly Faker _faker;

        public ApplyApiClientFixture(ApplyApiClient applyApiClient, AlgoliaFixture algoliaFixture, ModelFakerFixture modelFakerFixture)
        {
            _applyApiClient = applyApiClient;
            _algoliaFixture = algoliaFixture;
            _modelFakerFixture = modelFakerFixture;
            _faker = new Faker();
        }

        public async Task<Applicant> CreateNewApplicant()
        {
            var model = _modelFakerFixture.GetApplicant().Generate();
            var applicant = await CreateNewApplicant(model);
            await AcceptLegal(applicant.Id);

            model.Id = applicant.Id;
            model.AccountNumber = applicant.AccountNumber;
            applicant = await CreateProfile(model);
            await CreateEducationStatus(applicant.Id);

            return applicant;
        }

        public async Task AcceptLegal(Guid applicantId)
        {
            var latestPrivacyStatement = await _applyApiClient.GetLatestPrivacyStatement();
            await _applyApiClient.AcceptPrivacyStatement(applicantId, latestPrivacyStatement.Id);
            await _applyApiClient.UpdateCommPrefs(applicantId, false);
        }

        public async Task<Applicant> CreateNewApplicant(Applicant applicant)
        {
            await IdentityClientFixture.CreateApplicant(applicant);
            var testUser = await IdentityUserFixtureBase.GetApplicantUser(applicant.Email, IdentityConstants.ValidPassword);
            _applyApiClient.WithAccessToken(testUser.AccessToken);
            return await _applyApiClient.PostCurrentApplicant(applicant);
        }

        public Task<Applicant> CreateProfile(Applicant applicant)
        {
            var updates = _modelFakerFixture.GetApplicant().Generate();
            updates.Id = applicant.Id;
            updates.AccountNumber = applicant.AccountNumber;
            updates.FirstName = applicant.FirstName;
            updates.LastName = applicant.LastName;
            updates.BirthDate = applicant.BirthDate;
            updates.UserName = applicant.UserName;
            updates.Email = applicant.Email;
            if (updates.DateOfArrival != null && updates.DateOfArrival.ToDateTime() < updates.BirthDate.ToDateTime())
            {
                updates.DateOfArrival = updates.BirthDate;
            }
            return _applyApiClient.UpdateApplicant(applicant.Id, updates);
        }

        public Task CreateEducationStatus(Guid applicantId)
        {
            var educationStatusInfo = new EducationStatusInfo
            {
                EnrolledInHighSchool = false,
                GraduatedHighSchool = true,
                GraduationHighSchoolDate = _faker.Date.Past(5).ToString(Constants.DateFormat.YearMonthDashed)
            };
            return _applyApiClient.UpdateEducationStatus(applicantId, educationStatusInfo);
        }

        public Task<Education> CreateEducation(Guid applicantId, EducationType? educationType = null, bool inOntario = false)
        {
            EducationBase educationBase;
            switch (educationType)
            {
                case EducationType.CanadianHighSchool:
                    educationBase = !inOntario
                                ? _modelFakerFixture.GetEducationBase().Generate("default, Canadian, Highschool")
                                : _modelFakerFixture.GetEducationBase().Generate("default, Canadian, Ontario, Highschool");
                    break;
                case EducationType.CanadianCollege:
                    educationBase = !inOntario
                               ? _modelFakerFixture.GetEducationBase().Generate("default, Canadian, College")
                               : _modelFakerFixture.GetEducationBase().Generate("default, Canadian, Ontario, College");
                    break;
                case EducationType.CanadianUniversity:
                    educationBase = !inOntario
                               ? _modelFakerFixture.GetEducationBase().Generate("default, Canadian, University")
                               : _modelFakerFixture.GetEducationBase().Generate("default, Canadian, Ontario, University");
                    break;
                case EducationType.AcademicUpgrading:
                    educationBase = _modelFakerFixture.GetEducationBase().Generate("default, AcademicUpgrading");
                    break;
                case EducationType.International:
                    educationBase = _modelFakerFixture.GetEducationBase().Generate("default, Intl");
                    break;
                default:
                    educationBase = _modelFakerFixture.GetEducationBase().Generate();
                    break;
            }

            educationBase.ApplicantId = applicantId;
            return _applyApiClient.PostEducation(educationBase);
        }

        public Task<Application> CreateApplication(Guid applicantId)
        {
            var applicationBase = _modelFakerFixture.GetApplicationBase().Generate();
            applicationBase.ApplicantId = applicantId;
            return _applyApiClient.CreateApplication(applicationBase);
        }

        public async Task<IList<ProgramChoice>> CreateProgramChoices(Application application, int count = 1, List<Guid> excludeColleges = null)
        {
            var programChoices = _modelFakerFixture.GetProgramChoiceBase().Generate(count);
            var programIntakes = await _algoliaFixture.GetProgramOfferings(application.ApplicationCycleId, count, excludeColleges);

            for (var i = 0; i < programChoices.Count; i++)
            {
                programChoices[i].IntakeId = programIntakes[i].IntakeId;
                programChoices[i].EntryLevelId = programIntakes[i].ProgramValidEntryLevelIds?.Any() != true
                    ? programIntakes[i].ProgramEntryLevelId
                    : _faker.PickRandom(programIntakes[i].ProgramValidEntryLevelIds);
            }

            return await _applyApiClient.PutProgramChoices(application.Id, programChoices);
        }

        public async Task<IList<ProgramChoice>> CreateProgramChoice(Application application, Guid collegeId)
        {
            var programChoice = _modelFakerFixture.GetProgramChoiceBase().Generate();
            var programIntake = await _algoliaFixture.GetCollegeOffering(application.ApplicationCycleId, collegeId, new List<Models.AlgoliaProgramIntake>());

            programChoice.IntakeId = programIntake.IntakeId;
            programChoice.EntryLevelId = programIntake.ProgramValidEntryLevelIds?.Any() != true
                ? programIntake.ProgramEntryLevelId
                : _faker.PickRandom(programIntake.ProgramValidEntryLevelIds);

            return await _applyApiClient.PutProgramChoices(application.Id, new List<ProgramChoiceBase> { programChoice });
        }
    }
}
