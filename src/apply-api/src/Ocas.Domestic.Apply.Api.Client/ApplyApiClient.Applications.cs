using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<IList<Application>> GetApplications(Guid applicantId)
        {
            return Get<IList<Application>>(QueryHelpers.AddQueryString(Constants.Route.Applications, "applicantId", applicantId.ToString()));
        }

        public Task<Application> CreateApplication(ApplicationBase model)
        {
            return Post<Application>(Constants.Route.Applications, model);
        }

        public Task<IList<ProgramChoice>> PutProgramChoices(Guid applicationId, IList<ProgramChoiceBase> programChoices)
        {
            return Put<IList<ProgramChoice>>($"{Constants.Route.Applications}/{applicationId.ToString()}/{Constants.Route.ProgramChoices}", programChoices);
        }

        public Task CompletePrograms(Guid applicationId)
        {
            return Post($"{Constants.Route.Applications}/{applicationId.ToString()}/{Constants.Actions.CompletePrograms}");
        }

        public Task CompleteTranscripts(Guid applicationId)
        {
            return Post($"{Constants.Route.Applications}/{applicationId.ToString()}/{Constants.Actions.CompleteTranscripts}");
        }

        public Task<IList<ShoppingCartDetail>> GetShoppingCart(Guid applicationId)
        {
            return Get<IList<ShoppingCartDetail>>($"{Constants.Route.Applications}/{applicationId.ToString()}/{Constants.Actions.ShoppingCart}");
        }

        public Task ApplyVoucher(Guid applicationId, string code)
        {
            return Post(QueryHelpers.AddQueryString($"{Constants.Route.Applications}/{applicationId.ToString()}/{Constants.Actions.Voucher}", "code", code));
        }

        public Task RemoveVoucher(Guid applicationId, string code)
        {
            return Delete(QueryHelpers.AddQueryString($"{Constants.Route.Applications}/{applicationId.ToString()}/{Constants.Actions.Voucher}", "code", code));
        }
    }
}
