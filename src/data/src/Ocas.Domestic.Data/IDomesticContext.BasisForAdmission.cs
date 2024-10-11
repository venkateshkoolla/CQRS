﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<BasisForAdmission> GetBasisForAdmission(Guid basisForAdmissionId, Locale locale);

        Task<IList<BasisForAdmission>> GetBasisForAdmissions(Locale locale);
    }
}
