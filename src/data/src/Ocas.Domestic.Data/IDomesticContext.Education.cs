using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<Education> GetEducation(Guid educationId);
        Task<IList<Education>> GetEducations(Guid applicantId);
        Task<Education> CreateEducation(EducationBase educationBase);
        Task DeleteEducation(Guid educationId);
        Task<Education> UpdateEducation(Education education);
    }
}
