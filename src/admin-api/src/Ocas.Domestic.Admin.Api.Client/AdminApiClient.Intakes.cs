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
        public Task<IList<IntakeBrief>> GetIntakes(Guid applicationCycleId, Guid? collegeId, GetIntakesOptions options)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "applicationCycleMasterId", applicationCycleId.ToString() }
            };

            if (collegeId.HasValue) queryParams.Add("collegeId", collegeId.Value.ToString());
            if (!string.IsNullOrEmpty(options?.ProgramCode)) queryParams.Add("programcode", options.ProgramCode);
            if (!string.IsNullOrEmpty(options?.ProgramTitle)) queryParams.Add("programtitle", options.ProgramTitle);
            if (options?.DeliveryId.HasValue == true) queryParams.Add("deliveryid", options.DeliveryId.ToString());
            if (options?.CampusId.HasValue == true) queryParams.Add("campusid", options.CampusId.ToString());
            if (!string.IsNullOrEmpty(options?.StartDate)) queryParams.Add("startdate", options.StartDate);
            if (options?.SortBy != null) queryParams.Add("sortby", options.SortBy.ToString());
            if (options?.SortDirection != null) queryParams.Add("sortdirection", options.SortDirection.ToString());
            if (!string.IsNullOrEmpty(options?.Props)) queryParams.Add("props", options.Props);

            return Get<IList<IntakeBrief>>(QueryHelpers.AddQueryString(Constants.Route.Intakes, queryParams));
        }

        public Task<PagedResult<IntakeApplicant>> GetIntakeApplicants(Guid intakeId, GetIntakeApplicantOptions options)
        {
            var queryParams = new Dictionary<string, string>();

            if (options != null)
            {
                if (options.Page != null) queryParams.Add("page", options.Page.ToString());
                if (options.PageSize != null) queryParams.Add("pageSize", options.PageSize.ToString());
                if (options.SortBy != null) queryParams.Add("sortby", options.SortBy.ToString());
                if (options.SortDirection != null) queryParams.Add("sortDirection", options.SortDirection.ToString());
            }

            return Get<PagedResult<IntakeApplicant>>(QueryHelpers.AddQueryString($"{Constants.Route.Intakes}/{intakeId}/applicants", queryParams));
        }

        public Task<BinaryDocument> IntakeExport(Guid applicationCycleId, Guid collegeId, GetIntakesOptions options)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "applicationCycleMasterId", applicationCycleId.ToString() },
                { "collegeId", collegeId.ToString() }
            };

            if (options != null)
            {
                if (!string.IsNullOrEmpty(options.ProgramCode)) queryParams.Add("programcode", options.ProgramCode);
                if (!string.IsNullOrEmpty(options.ProgramTitle)) queryParams.Add("programtitle", options.ProgramTitle);
                if (options.DeliveryId.HasValue) queryParams.Add("deliveryid", options.DeliveryId.ToString());
                if (options.CampusId.HasValue) queryParams.Add("campusId", options.CampusId.ToString());
                if (!string.IsNullOrEmpty(options.StartDate)) queryParams.Add("startdate", options.StartDate);
                if (options.SortBy != null) queryParams.Add("sortby", options.SortBy.ToString());
                if (options.SortDirection != null) queryParams.Add("sortdirection", options.SortDirection.ToString());
            }

            return Get<BinaryDocument>(QueryHelpers.AddQueryString($"{Constants.Route.Intakes}/export", queryParams));
        }

        public Task UpdateIntakeAvailability(Guid availabilityId, IList<Guid> intakeIds)
        {
            return Put($"{Constants.Route.Intakes}/update-availability/{availabilityId}", intakeIds);
        }
    }
}
