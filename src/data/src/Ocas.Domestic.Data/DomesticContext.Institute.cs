using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Institute> GetInstitute(Guid id)
        {
            return CrmExtrasProvider.GetInstitute(id);
        }

        public Task<IList<Institute>> GetInstitutes()
        {
            return CrmExtrasProvider.GetInstitutes();
        }
    }
}
