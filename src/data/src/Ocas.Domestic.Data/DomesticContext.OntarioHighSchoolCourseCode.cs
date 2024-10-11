using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<IList<OntarioHighSchoolCourseCode>> GetOntarioHighSchoolCourseCodes()
        {
            return CrmExtrasProvider.GetOntarioHighSchoolCourseCodes();
        }

        public Task<OntarioHighSchoolCourseCode> GetOntarioHighSchoolCourseCode(string name)
        {
            return CrmExtrasProvider.GetOntarioHighSchoolCourseCode(name);
        }
    }
}
