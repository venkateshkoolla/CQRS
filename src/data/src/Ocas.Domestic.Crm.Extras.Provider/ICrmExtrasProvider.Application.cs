using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<Application> GetApplication(Guid id);
        Task<IList<Application>> GetApplications(Guid applicantId);
        Task<IList<Application>> GetApplications(string number);
    }
}
