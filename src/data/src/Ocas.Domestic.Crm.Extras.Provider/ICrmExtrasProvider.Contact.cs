using System;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<ApplicantSummary> GetApplicantSummary(GetApplicantSummaryOptions options);
        Task<ApplicantCompletedSteps> GetCompletedSteps(Guid id);
        Task<Contact> GetContact(Guid id);
        Task<Contact> GetContact(string userName, string subjectId);
        Task<string> GetContactSubjectId(Guid id);
        Task<bool> IsActive(Guid id);
        Task<bool> IsDuplicateContact(Guid id, string emailAddress);
        Task<bool> IsDuplicateContact(Guid id, string firstName, string lastName, DateTime birthDate);
        Task<bool> IsDuplicateOen(Guid id, string oen);
        Task<bool> CanAccessApplicant(Guid applicantId, string partnerId, UserType userType);
    }
}
