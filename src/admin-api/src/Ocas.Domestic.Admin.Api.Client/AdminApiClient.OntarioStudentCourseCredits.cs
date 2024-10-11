using System;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Client
{
    public partial class AdminApiClient
    {
        public Task<OntarioStudentCourseCredit> PostOntarioStudentCourseCredit(OntarioStudentCourseCreditBase model)
        {
            return Post<OntarioStudentCourseCredit>(Constants.Route.OntarioStudentCourseCredits, model);
        }

        public Task<OntarioStudentCourseCredit> UpdateOntarioStudentCourseCredit(OntarioStudentCourseCredit model)
        {
            return Put<OntarioStudentCourseCredit>($"{Constants.Route.OntarioStudentCourseCredits}/{model.Id}", model);
        }

        public Task DeleteOntarioStudentCourseCredit(Guid ontarioStudentCourseCreditId)
        {
            return Delete($"{Constants.Route.OntarioStudentCourseCredits}/{ontarioStudentCourseCreditId}");
        }

        public Task GetOntarioHighSchoolCourseCode(string code)
        {
            return Get<OntarioHighSchoolCourseCode>($"{Constants.Route.OntarioStudentCourseCredits}/{code}");
        }
    }
}