﻿using System.Collections.Generic;
using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public IList<University> MapUniversity(IList<Dto.University> list)
        {
            return _mapper.Map<IList<University>>(list);
        }
    }
}
