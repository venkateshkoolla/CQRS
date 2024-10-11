using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<BinaryDocument> GetSupportingDocumentFile(Guid id)
        {
            return Get<BinaryDocument>($"{Constants.Route.SupportingDocuments}/{id.ToString()}/{Constants.Actions.Download}");
        }

        public Task<IList<SupportingDocument>> GetSupportingDocuments(Guid applicantId)
        {
            return Get<IList<SupportingDocument>>(QueryHelpers.AddQueryString(Constants.Route.SupportingDocuments, "applicantId", applicantId.ToString()));
        }
    }
}
