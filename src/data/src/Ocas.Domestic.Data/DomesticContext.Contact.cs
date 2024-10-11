using System;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Contact = Ocas.Domestic.Models.Contact;
using Task = System.Threading.Tasks.Task;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Contact> GetContact(Guid id)
        {
            return CrmExtrasProvider.GetContact(id);
        }

        public Task<Contact> GetContact(string username = null, string subjectId = null)
        {
            return CrmExtrasProvider.GetContact(username, subjectId);
        }

        public Task<string> GetContactSubjectId(Guid id)
        {
            return CrmExtrasProvider.GetContactSubjectId(id);
        }

        public Task<bool> IsActive(Guid id)
        {
            return CrmExtrasProvider.IsActive(id);
        }

        public Task<bool> IsDuplicateContact(Guid id, string emailAddress)
        {
            return CrmExtrasProvider.IsDuplicateContact(id, emailAddress);
        }

        public Task<bool> IsDuplicateContact(Guid id, string firstName, string lastName, DateTime birthDate)
        {
            if (birthDate.Kind != DateTimeKind.Utc) throw new ArgumentException($"birthDate must be DateTimeKind.Utc but received: {birthDate.Kind}", nameof(birthDate));

            return CrmExtrasProvider.IsDuplicateContact(id, firstName, lastName, birthDate);
        }

        public async Task<bool> IsDuplicateOen(Guid id, string oen)
        {
            return !string.IsNullOrEmpty(oen) && oen != Constants.Contact.OntarioEducationNumberDefault && await CrmExtrasProvider.IsDuplicateOen(id, oen);
        }

        public Task<bool> CanAccessApplicant(Guid applicantId, string partnerId, UserType userType)
        {
            return CrmExtrasProvider.CanAccessApplicant(applicantId, partnerId, userType);
        }

        public async Task<Contact> CreateContact(ContactBase contactBase)
        {
            if (contactBase.BirthDate.Kind != DateTimeKind.Utc) throw new ArgumentException($"ContactBase.BirthDate must be DateTimeKind.Utc but received: {contactBase.BirthDate.Kind}", nameof(contactBase));
            if (contactBase.LastLogin.HasValue && contactBase.LastLogin.Value.Kind != DateTimeKind.Utc) throw new ArgumentException($"ContactBase.LastLogin must be DateTimeKind.Utc but received: {contactBase.LastLogin.Value.Kind}", nameof(contactBase));

            var crmEntity = CrmMapper.MapContactBase(contactBase);
            var id = await CrmProvider.CreateEntity(crmEntity);

            return await GetContact(id);
        }

        public Task<ApplicantSummary> GetApplicantSummary(GetApplicantSummaryOptions options)
        {
            return CrmExtrasProvider.GetApplicantSummary(options);
        }

        public async Task<Contact> UpdateContact(Contact contact)
        {
            if (contact.BirthDate.Kind != DateTimeKind.Utc) throw new ArgumentException($"ContactBase.BirthDate must be DateTimeKind.Utc but received: {contact.BirthDate.Kind}", nameof(contact));
            if (contact.LastLogin.HasValue && contact.LastLogin.Value.Kind != DateTimeKind.Utc) throw new ArgumentException($"ContactBase.LastLogin must be DateTimeKind.Utc but received: {contact.LastLogin.Value.Kind}", nameof(contact));
            if (contact.DateOfArrival.HasValue && contact.DateOfArrival.Value.Kind != DateTimeKind.Utc) throw new ArgumentException($"ContactBase.DateOfArrival must be DateTimeKind.Utc but received: {contact.DateOfArrival.Value.Kind}", nameof(contact));
            if (contact.MailingAddress?.CountryId != null && string.IsNullOrEmpty(contact.MailingAddress.Country)) throw new ArgumentException("Address.Country must be set", nameof(contact));
            if (contact.MailingAddress?.ProvinceStateId != null && string.IsNullOrEmpty(contact.MailingAddress.ProvinceState)) throw new ArgumentException("Address.ProvinceState must be set", nameof(contact));

            var crmEntity = CrmProvider.Contacts.First(x => x.Id == contact.Id);

            CrmMapper.PatchContact(contact, crmEntity);
            await CrmProvider.UpdateEntity(crmEntity);

            return await GetContact(crmEntity.Id);
        }

        public Task DeleteContact(Guid id)
        {
            return CrmProvider.DeactivateEntity(Crm.Entities.Contact.EntityLogicalName, id);
        }

        public async Task<CompletedSteps?> GetCompletedStep(Guid contactId)
        {
            var step = await CrmExtrasProvider.GetCompletedSteps(contactId);
            return step.NewCompletedSteps;
        }

        public async Task<CompletedSteps?> UpdateCompletedSteps(Guid contactId)
        {
            var applicantCompletedSteps = await CrmExtrasProvider.GetCompletedSteps(contactId);

            if (!applicantCompletedSteps.HasChanged)
            {
                return applicantCompletedSteps.NewCompletedSteps;
            }

            var crmEntity = CrmProvider.Contacts.First(x => x.Id == contactId);

            crmEntity.OCASLR_CompletedStepsEnum = (Contact_OCASLR_CompletedSteps?)applicantCompletedSteps.NewCompletedSteps;

            await CrmProvider.UpdateEntity(crmEntity);

            return applicantCompletedSteps.NewCompletedSteps;
        }
    }
}
