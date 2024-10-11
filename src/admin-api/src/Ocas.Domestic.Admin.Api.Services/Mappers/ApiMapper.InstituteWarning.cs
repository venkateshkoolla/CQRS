using System;
using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public IList<InstituteWarning> MapInstituteWarnings(IList<Dto.TranscriptRequestException> list, IList<Guid> educationExceptions)
        {
            return list.Select((transcriptRequestException) =>
            {
                var instituteWarning = _mapper.Map<InstituteWarning>(transcriptRequestException);
                instituteWarning.Type = educationExceptions.Contains(instituteWarning.Id)
                    ? InstituteWarningType.Education
                    : InstituteWarningType.Transcript;
                return instituteWarning;
            }).ToList();
        }
    }
}