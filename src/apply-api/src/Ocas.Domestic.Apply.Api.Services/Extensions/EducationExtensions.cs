using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Core.Enums;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;

namespace Ocas.Domestic.Apply.Api.Services.Extensions
{
    public static class EducationExtensions
    {
        public static EducationType? GetEducationType(this EducationBase education, IList<Country> countries, IList<LookupItem> instituteTypes)
        {
            if (education is null) return null;

            if (education.AcademicUpgrade == true) return EducationType.AcademicUpgrading;

            var country = countries.FirstOrDefault(x => x.Id == education.CountryId);

            if (country == null) return null;

            if (country.Code != Constants.Countries.Canada)
            {
                return EducationType.International;
            }

            var instituteType = instituteTypes.FirstOrDefault(x => x.Id == education.InstituteTypeId);

            if (instituteType is null) return null;

            switch (instituteType.Code)
            {
                case Constants.InstituteTypes.HighSchool:
                    return EducationType.CanadianHighSchool;
                case Constants.InstituteTypes.College:
                    return EducationType.CanadianCollege;
                case Constants.InstituteTypes.University:
                    return EducationType.CanadianUniversity;
                default:
                    return null;
            }
        }
    }
}
