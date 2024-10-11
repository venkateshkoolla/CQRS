using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<OntarioStudentCourseCredit> GetOntarioStudentCourseCredit(Guid id);
        Task<IList<OntarioStudentCourseCredit>> GetOntarioStudentCourseCredits(GetOntarioStudentCourseCreditOptions options);
    }
}