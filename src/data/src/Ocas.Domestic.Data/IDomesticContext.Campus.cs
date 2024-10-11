using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<Campus> GetCampus(Guid campusId);
        Task<IList<Campus>> GetCampuses();
        Task<IList<Campus>> GetCampuses(Guid collegeId);
    }
}
