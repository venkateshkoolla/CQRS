using System;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<Applicant> GetCurrentApplicant()
        {
            return Get<Applicant>($"{Constants.Route.Applicants}/current");
        }

        public Task<Applicant> GetApplicant(Guid applicantId)
        {
            return Get<Applicant>($"{Constants.Route.Applicants}/{applicantId}");
        }

        [ObsoleteAttribute("This function is obsolete. Use GetApplicant instead.", true)]
        public Task<Applicant> GetApplicantAsOcas(Guid applicantId)
        {
            return Get<Applicant>($"{Constants.Route.Applicants}/{applicantId}");
        }

        public Task<Applicant> PostCurrentApplicant(ApplicantBase model)
        {
            return Post<Applicant>($"{Constants.Route.Applicants}/current", model);
        }

        public Task<Applicant> UpdateApplicant(Guid applicantGuid, Applicant model)
        {
            return Put<Applicant>($"{Constants.Route.Applicants}/{applicantGuid}", model);
        }

        public Task AcceptPrivacyStatement(Guid applicantGuid, Guid privacyStatementId)
        {
            return Put($"{Constants.Route.Applicants}/{applicantGuid}/privacy-statement", new { privacyStatementId });
        }

        public Task UpdateCommPrefs(Guid applicantGuid, bool agreedToCasl)
        {
            return Put($"{Constants.Route.Applicants}/{applicantGuid}/comm-prefs", new { agreedToCasl });
        }

        public Task UpdateEducationStatus(Guid applicantGuid, EducationStatusInfo educationStatusInfo)
        {
            return Put($"{Constants.Route.Applicants}/{applicantGuid}/edu-status", educationStatusInfo);
        }

        public Task<OcasVerificationDetails> VerifyEmailApplicant(Guid applicantGuid, string email)
        {
            return Get<OcasVerificationDetails>($"{Constants.Route.Applicants}/{applicantGuid}/verify-email/{email}");
        }

        public Task<OcasVerificationDetails> VerifyOenApplicant(Guid applicantGuid, string oen)
        {
            return Get<OcasVerificationDetails>($"{Constants.Route.Applicants}/{applicantGuid}/verify-oen/{oen}");
        }

        public Task VerifyCurrentApplicantProfile(Guid applicantId)
        {
            return Post($"{Constants.Route.Applicants}/{applicantId}/verify-profile");
        }

        public Task UpdateInternationalCreditAssessment(Guid applicantGuid, IntlCredentialAssessment intlCredentialAssessment)
        {
            return Put($"{Constants.Route.Applicants}/{applicantGuid}/intl-cred", intlCredentialAssessment);
        }
    }
}
