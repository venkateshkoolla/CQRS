using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<Application> GetApplication(Guid id);
        Task<IList<Application>> GetApplications(Guid applicantId);
        Task<Application> CreateApplication(ApplicationBase application);
        Task<Application> UpdateApplication(Application application);
        Task DeleteApplication(Guid id);
        Task TriggerDeclineEmail(Guid applicationId, string modifiedBy);
        Task<bool> IsDuplicateApplication(Guid id, string number);
    }
}
