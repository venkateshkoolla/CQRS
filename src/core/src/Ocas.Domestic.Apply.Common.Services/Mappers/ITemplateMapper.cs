using System.Collections.Generic;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.Apply.Models.Templates;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Services.Mappers
{
    public interface ITemplateMapper
    {
        StandardizedTestViewModel MapStandardizedTest(Dto.Test model, IList<Country> countries, IList<ProvinceState> provinceStates, IList<City> cities, IList<LookupItem> testTypes);

        HighSchoolGradesViewModel MapHighSchoolGrades(
            Dto.AcademicRecord academicRecord,
            IList<Dto.Transcript> transcripts,
            IList<Dto.OntarioStudentCourseCredit> grades,
            IList<HighSchool> highSchools,
            IList<LookupItem> highSkillsMajors,
            IList<LookupItem> highestEducations,
            IList<LookupItem> literacyTests,
            IList<LookupItem> communityInvolvements,
            IList<LookupItem> courseStatuses,
            IList<LookupItem> courseTypes,
            IList<LookupItem> courseDeliveries,
            IList<LookupItem> gradeTypes,
            string locale);
    }
}
