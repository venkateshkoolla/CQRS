using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public IList<TranscriptTransmission> MapTranscriptTransmissions(IList<Dto.TranscriptTransmission> list, IList<Dto.InstituteType> instituteTypes)
        {
            var transmissions = new List<TranscriptTransmission>();
            foreach (var model in list)
            {
                var transmission = _mapper.Map<TranscriptTransmission>(model);

                if (transmission.Code != Constants.TranscriptTransmissions.SendTranscriptNow && !transmission.EligibleUntil.HasValue)
                {
                    continue;
                }

                switch (model.InstitutionType)
                {
                    case InstitutionType.College:
                        transmission.InstituteTypeId = instituteTypes.FirstOrDefault(i => i.Code == Constants.InstituteTypes.College)?.Id;
                        break;
                    case InstitutionType.University:
                        transmission.InstituteTypeId = instituteTypes.FirstOrDefault(i => i.Code == Constants.InstituteTypes.University)?.Id;
                        break;
                    default:
                        transmission.InstituteTypeId = null;
                        break;
                }

                transmissions.Add(transmission);
            }

            return transmissions.OrderBy(t => t.EligibleUntil).ToList();
        }
    }
}
