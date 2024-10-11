using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<OntarioHighSchoolCourseCode> GetOntarioHighSchoolCourseCode(string name);
        Task<IList<OntarioHighSchoolCourseCode>> GetOntarioHighSchoolCourseCodes();
    }
}
