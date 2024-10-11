using System;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<Contact> GetContact(Guid id);
        Task<Contact> GetContact(string username = null, string subjectId = null);
        Task<string> GetContactSubjectId(Guid id);
        Task<bool> IsActive(Guid id);
        Task<bool> IsDuplicateContact(Guid id, string emailAddress);
        Task<bool> IsDuplicateContact(Guid id, string firstName, string lastName, DateTime birthDate);
        Task<bool> IsDuplicateOen(Guid id, string oen);
        Task<bool> CanAccessApplicant(Guid applicantId, string partnerId, UserType userType);
        Task<Contact> CreateContact(ContactBase contactBase);
        Task<ApplicantSummary> GetApplicantSummary(GetApplicantSummaryOptions options);
        Task<Contact> UpdateContact(Contact contact);
        Task DeleteContact(Guid id);
        Task<CompletedSteps?> GetCompletedStep(Guid contactId);
        Task<CompletedSteps?> UpdateCompletedSteps(Guid contactId);
    }
}
