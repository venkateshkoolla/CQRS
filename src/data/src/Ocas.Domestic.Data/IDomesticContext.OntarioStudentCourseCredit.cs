using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<OntarioStudentCourseCredit> CreateOntarioStudentCourseCredit(OntarioStudentCourseCreditBase ontarioStudentCourseCreditBase);
        Task DeleteOntarioStudentCourseCredit(Guid ontarioStudentCourseCreditId);
        Task<OntarioStudentCourseCredit> GetOntarioStudentCourseCredit(Guid id);
        Task<IList<OntarioStudentCourseCredit>> GetOntarioStudentCourseCredits(GetOntarioStudentCourseCreditOptions options);
        Task<OntarioStudentCourseCredit> UpdateOntarioStudentCourseCredit(OntarioStudentCourseCredit ontarioStudentCourseCredit);
    }
}
