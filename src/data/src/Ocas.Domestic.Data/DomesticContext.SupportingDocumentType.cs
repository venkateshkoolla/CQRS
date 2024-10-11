using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<SupportingDocumentType> GetSupportingDocumentType(Guid id, Locale locale)
        {
            return CrmExtrasProvider.GetSupportingDocumentType(id, locale);
        }

        public Task<IList<SupportingDocumentType>> GetSupportingDocumentTypes(Locale locale)
        {
            return CrmExtrasProvider.GetSupportingDocumentTypes(locale);
        }
    }
}
