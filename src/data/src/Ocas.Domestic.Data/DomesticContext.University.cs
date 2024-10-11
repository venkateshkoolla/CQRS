using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<University> GetUniversity(Guid universityId)
        {
            return CrmExtrasProvider.GetUniversity(universityId);
        }

        public Task<IList<University>> GetUniversities()
        {
            return CrmExtrasProvider.GetUniversities();
        }
    }
}
