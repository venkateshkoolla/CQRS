using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public TranscriptRequest MapTranscriptRequest(Dto.TranscriptRequest model, IList<LookupItem> instituteTypes, IList<TranscriptTransmission> transmissions)
        {
            var transcriptRequest = _mapper.Map<TranscriptRequest>(model);
            transcriptRequest.TransmissionId = model.TranscriptTransmissionId ?? transmissions.First(x => x.Code == Constants.TranscriptTransmissions.SendTranscriptNow).Id;

            switch (model.FromSchoolType)
            {
                case TranscriptSchoolType.College:
                    transcriptRequest.FromInstituteTypeId = instituteTypes.FirstOrDefault(i => i.Code == Constants.InstituteTypes.College)?.Id;
                    break;
                case TranscriptSchoolType.HighSchool:
                    transcriptRequest.FromInstituteTypeId = instituteTypes.FirstOrDefault(i => i.Code == Constants.InstituteTypes.HighSchool)?.Id;
                    break;
                case TranscriptSchoolType.University:
                    transcriptRequest.FromInstituteTypeId = instituteTypes.FirstOrDefault(i => i.Code == Constants.InstituteTypes.University)?.Id;
                    break;
            }

            return transcriptRequest;
        }
    }
}
