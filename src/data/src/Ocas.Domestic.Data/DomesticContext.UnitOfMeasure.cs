using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<UnitOfMeasure> GetUnitOfMeasure(Guid unitOfMeasureId, Locale locale)
        {
            return CrmExtrasProvider.GetUnitOfMeasure(unitOfMeasureId, locale);
        }

        public Task<IList<UnitOfMeasure>> GetUnitOfMeasures(Locale locale)
        {
            return CrmExtrasProvider.GetUnitOfMeasures(locale);
        }
    }
}
