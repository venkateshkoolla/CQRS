using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.Apply.Services.Extensions;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Services.Mappers
{
    public partial class ApiMapperBase
    {
        public async Task<Education> MapEducation(Dto.Education entity, IList<LookupItem> instituteTypes, IList<College> colleges, IList<HighSchool> highSchools, IList<University> universities, IDomesticContext domesticContext)
        {
            var model = _mapper.Map<Education>(entity);
            model.TranscriptFee = null;

            if (model.InstituteId.HasValue && model.InstituteTypeId.HasValue)
            {
                var instituteType = instituteTypes.FirstOrDefault(i => i.Id == model.InstituteTypeId);

                if (instituteType != null)
                {
                    switch (instituteType.Code)
                    {
                        case Constants.InstituteTypes.College:
                            var college = colleges.FirstOrDefault(i => i.Id == model.InstituteId)
                                ?? _mapper.Map<College>(await domesticContext.GetCollege(model.InstituteId.Value));

                            model.TranscriptFee = college.HasEtms ? college.TranscriptFee ?? 0 : (decimal?)null;
                            model.Address = college.Address;
                            break;
                        case Constants.InstituteTypes.HighSchool:
                            var highSchool = highSchools.FirstOrDefault(i => i.Id == model.InstituteId)
                                ?? _mapper.Map<HighSchool>(await domesticContext.GetHighSchool(model.InstituteId.Value, Constants.Localization.FallbackLocalization.ToLocaleEnum()));

                            model.TranscriptFee = highSchool.HasEtms ? highSchool.TranscriptFee ?? 0 : (decimal?)null;
                            model.Address = highSchool.Address;
                            break;
                        case Constants.InstituteTypes.University:
                            var university = universities.FirstOrDefault(i => i.Id == model.InstituteId)
                                ?? _mapper.Map<University>(await domesticContext.GetUniversity(model.InstituteId.Value));

                            model.TranscriptFee = university.HasEtms ? university.TranscriptFee ?? 0 : (decimal?)null;
                            model.Address = university.Address;
                            break;
                    }
                }
            }

            return model;
        }
    }
}
