using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public Application MapApplication(Dto.Application model)
        {
            var result = _mapper.Map<Application>(model);

            result.ProgramsComplete = false;
            result.TranscriptsComplete = false;

            if (model.CompletedSteps is null)
            {
                return result;
            }

            if (model.CompletedSteps.Value >= (int)ApplicationCompletedSteps.ProgramChoice)
            {
                result.ProgramsComplete = true;
            }

            if (model.CompletedSteps.Value >= (int)ApplicationCompletedSteps.TranscriptRequests)
            {
                result.TranscriptsComplete = true;
            }

            return result;
        }
    }
}
