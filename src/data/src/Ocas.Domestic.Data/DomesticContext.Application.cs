using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;
using Task = System.Threading.Tasks.Task;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Application> GetApplication(Guid id)
        {
            return CrmExtrasProvider.GetApplication(id);
        }

        public Task<IList<Application>> GetApplications(Guid applicantId)
        {
            return CrmExtrasProvider.GetApplications(applicantId);
        }

        public async Task<Application> CreateApplication(ApplicationBase application)
        {
            if (application.EffectiveDate.HasValue && application.EffectiveDate.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException($"EffectiveDate must be DateTimeKind.Utc but received: {application.EffectiveDate.Value.Kind}", nameof(application));
            }

            var entity = CrmMapper.MapApplication(application);
            var id = await CrmProvider.CreateEntity(entity);

            return await GetApplication(id);
        }

        public async Task<Application> UpdateApplication(Application application)
        {
            if (application.EffectiveDate.HasValue && application.EffectiveDate.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException($"EffectiveDate must be DateTimeKind.Utc but received: {application.EffectiveDate.Value.Kind}", nameof(application));
            }

            var crmEntity = CrmProvider.Applications.Single(x => x.Id == application.Id);

            CrmMapper.PatchApplication(application, crmEntity);
            await CrmProvider.UpdateEntity(crmEntity);

            return await GetApplication(application.Id);
        }

        public Task DeleteApplication(Guid id)
        {
            return CrmProvider.DeactivateEntity(ocaslr_application.EntityLogicalName, id);
        }

        public async Task TriggerDeclineEmail(Guid applicationId, string modifiedBy)
        {
            var application = CrmProvider.Applications.First(x => x.Id == applicationId);

            application.Ocaslr_eventtrigger = null;
            application.ocaslr_modifiedbyuser = modifiedBy;

            await CrmProvider.UpdateEntity(application);

            application.Ocaslr_eventtriggerEnum = ocaslr_application_Ocaslr_eventtrigger.SendDeclineEmail;
            await CrmProvider.UpdateEntity(application);
        }

        public async Task<bool> IsDuplicateApplication(Guid id, string number)
        {
            var applications = await CrmExtrasProvider.GetApplications(number);
            return applications.Any(a => a.Id != id);
        }
    }
}
