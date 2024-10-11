using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<BasisForAdmission> GetBasisForAdmission(Guid basisForAdmissionId, Locale locale)
        {
            return CrmExtrasProvider.GetBasisForAdmission(basisForAdmissionId, locale);
        }

        public Task<IList<BasisForAdmission>> GetBasisForAdmissions(Locale locale)
        {
            return CrmExtrasProvider.GetBasisForAdmissions(locale);
        }
    }
}
