﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<Education> GetEducation(Guid id);
        Task<IList<Education>> GetEducations(Guid applicantId);
    }
}
