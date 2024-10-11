using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Client
{
    public partial class AdminApiClient
    {
        public Task<Application> UpdateApplicationNumber(Guid applicationId, ApplicationNumberUpdateInfo applicationInfo)
        {
            return Put<Application>($"{Constants.Route.Applications}/{applicationId}/{Constants.Actions.Number}", applicationInfo);
        }

        public Task<Application> UpdateApplicationEffectiveDate(Guid applicationId, ApplicationEffectiveDateUpdateInfo applicationInfo)
        {
            return Put<Application>($"{Constants.Route.Applications}/{applicationId}/{Constants.Actions.EffectiveDate}", applicationInfo);
        }

        public Task<IList<ApplicationSummary>> PayOrder(Guid applicationId, OfflinePaymentInfo offlinePaymentInfo)
        {
            return Post<IList<ApplicationSummary>>($"{Constants.Route.Applications}/{applicationId}/{Constants.Actions.Pay}", offlinePaymentInfo);
        }

        public Task<IList<OfferHistory>> GetOfferHistories(Guid applicationId)
        {
            return Get<IList<OfferHistory>>($"{Constants.Route.Applications}/{applicationId}/{Constants.Actions.OfferHistories}");
        }

        public Task<PagedResult<CollegeTransmissionHistory>> GetCollegeTransmissionHistories(Guid applicationId, GetCollegeTransmissionHistoryOptions options)
        {
            var queryParams = new Dictionary<string, string>();

            if (options.Page.HasValue) queryParams.Add("page", options.Page.ToString());
            if (options.PageSize.HasValue) queryParams.Add("pageSize", options.PageSize.ToString());
            if (!string.IsNullOrEmpty(options.FromDate)) queryParams.Add("fromDate", options.FromDate);
            if (!string.IsNullOrEmpty(options.ToDate)) queryParams.Add("toDate", options.ToDate);
            if (options.Activity != null) queryParams.Add("activity", options.Activity.ToString());

            return Get<PagedResult<CollegeTransmissionHistory>>(QueryHelpers.AddQueryString($"{Constants.Route.Applications}/{applicationId}/{Constants.Actions.TransmissionHistories}", queryParams));
        }

        public Task<ApplicationSummary> CreateProgramChoice(Guid applicationId, CreateProgramChoiceRequest programChoice)
        {
            return Post<ApplicationSummary>($"{Constants.Route.Applications}/{applicationId}/{Constants.Actions.ProgramChoice}", programChoice);
        }
    }
}
