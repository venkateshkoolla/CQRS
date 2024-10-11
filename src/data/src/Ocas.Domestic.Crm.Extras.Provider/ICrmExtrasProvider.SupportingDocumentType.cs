using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<SupportingDocumentType> GetSupportingDocumentType(Guid id, Locale locale);
        Task<IList<SupportingDocumentType>> GetSupportingDocumentTypes(Locale locale);
    }
}
