using System;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Client
{
    public partial class AdminApiClient
    {
        public Task<BinaryDocument> GetSupportingDocumentFile(Guid id)
        {
            return Get<BinaryDocument>($"{Constants.Route.SupportingDocuments}/{id}/download");
        }
    }
}
