﻿using System.Collections.Generic;
using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public IList<ApplicantMessage> MapApplicantMessages(IList<Dto.ApplicantMessage> list)
        {
            return _mapper.Map<IList<ApplicantMessage>>(list);
        }
    }
}
