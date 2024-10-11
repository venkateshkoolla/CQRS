using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Client
{
    public partial class AdminApiClient
    {
        public Task<Program> GetProgram(Guid programId)
        {
            return Get<Program>($"{Constants.Route.Programs}/{programId}");
        }

        public Task<IList<ProgramBrief>> GetProgramBriefs(Guid applicationCycleId, Guid collegeId, GetProgramBriefOptions options)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "applicationCycleMasterId", applicationCycleId.ToString() },
                { "collegeId", collegeId.ToString() }
            };

            if (!string.IsNullOrEmpty(options.Code)) queryParams.Add("code", options.Code);
            if (!string.IsNullOrEmpty(options.Title)) queryParams.Add("title", options.Title);
            if (options.DeliveryId.HasValue) queryParams.Add("deliveryId", options.DeliveryId.ToString());
            if (options.CampusId.HasValue) queryParams.Add("campusId", options.CampusId.ToString());
            if (options.SortDirection != null) queryParams.Add("sortDirection", options.SortDirection.ToString());
            if (options.SortBy != null) queryParams.Add("sortBy", options.SortBy.ToString());

            return Get<IList<ProgramBrief>>(QueryHelpers.AddQueryString(Constants.Route.Programs, queryParams));
        }

        public Task<bool> ProgramCodeAvailable(string code, Guid collegeApplicationCycleId, Guid campusId, Guid deliveryId)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "applicationCycleId", collegeApplicationCycleId.ToString() },
                { "campusId", campusId.ToString() },
                { "deliveryId", deliveryId.ToString() }
            };

            return Get<bool>(QueryHelpers.AddQueryString($"{Constants.Route.Programs}/code-available/{code}", queryParams));
        }

        public Task<BinaryDocument> ProgramExport(Guid applicationCycleId, Guid collegeId, GetProgramOptions options)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "applicationCycleMasterId", applicationCycleId.ToString() },
                { "collegeId", collegeId.ToString() }
            };

            if (options != null)
            {
                if (!string.IsNullOrEmpty(options.Code)) queryParams.Add("code", options.Code);
                if (!string.IsNullOrEmpty(options.Title)) queryParams.Add("title", options.Title);
                if (options.DeliveryId.HasValue) queryParams.Add("deliveryId", options.DeliveryId.ToString());
                if (options.CampusId.HasValue) queryParams.Add("campusId", options.CampusId.ToString());
                if (options.SortBy != null) queryParams.Add("sortby", options.SortBy.ToString());
                if (options.SortDirection != null) queryParams.Add("sortdirection", options.SortDirection.ToString());
            }

            return Get<BinaryDocument>(QueryHelpers.AddQueryString($"{Constants.Route.Programs}/export", queryParams));
        }

        public Task DeleteProgram(Guid id)
        {
            return Delete<Program>($"{Constants.Route.Programs}/{id}");
        }

        public Task<Program> CreateProgram(ProgramBase p)
        {
            return Post<Program>(Constants.Route.Programs, p);
        }

        public Task<Program> UpdateProgram(Guid id, Program p)
        {
            return Put<Program>($"{Constants.Route.Programs}/{id}", p);
        }
    }
}
