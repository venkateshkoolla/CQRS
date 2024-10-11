using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<College> GetCollege(Guid collegeId);
        Task<IList<College>> GetColleges();
        Task<IList<College>> GetColleges(SchoolStatusCode statusCode);
    }
}
