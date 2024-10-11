using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Client
{
    public partial class AdminApiClient
    {
        public Task<PagedResult<ApplicantBrief>> GetApplicantBriefs(GetApplicantBriefOptions options)
        {
            var queryParams = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(options.AccountNumber)) queryParams.Add("accountnumber", options.AccountNumber);
            if (options.ApplicationCycleId.HasValue) queryParams.Add("applicationcycleid", options.ApplicationCycleId.ToString());
            if (!string.IsNullOrEmpty(options.ApplicationNumber)) queryParams.Add("applicationnumber", options.ApplicationNumber);
            if (options.ApplicationStatusId.HasValue) queryParams.Add("applicationstatusid", options.ApplicationStatusId.ToString());
            if (!string.IsNullOrEmpty(options.BirthDate)) queryParams.Add("birthdate", options.BirthDate);
            if (!string.IsNullOrEmpty(options.Email)) queryParams.Add("email", options.Email);
            if (!string.IsNullOrEmpty(options.FirstName)) queryParams.Add("firstname", options.FirstName);
            if (!string.IsNullOrEmpty(options.LastName)) queryParams.Add("lastname", options.LastName);
            if (!string.IsNullOrEmpty(options.MiddleName)) queryParams.Add("middlename", options.MiddleName);
            if (!string.IsNullOrEmpty(options.Mident)) queryParams.Add("mident", options.Mident);
            if (!string.IsNullOrEmpty(options.PhoneNumber)) queryParams.Add("phonenumber", options.PhoneNumber);
            if (!string.IsNullOrEmpty(options.OntarioEducationNumber)) queryParams.Add("ontarioeducationnumber", options.OntarioEducationNumber);
            if (options.Page != null) queryParams.Add("page", options.Page.ToString());
            if (options.PageSize != null) queryParams.Add("pagesize", options.PageSize.ToString());
            if (!string.IsNullOrEmpty(options.PreviousLastName)) queryParams.Add("previouslastname", options.PreviousLastName);
            queryParams.Add("sortby", options.SortBy.ToString());
            if (options.SortDirection.HasValue) queryParams.Add("sortdirection", options.SortDirection.ToString());
            return Get<PagedResult<ApplicantBrief>>(QueryHelpers.AddQueryString(Constants.Route.Applicants, queryParams));
        }

        public Task<ApplicantSummary> GetApplicantSummary(Guid applicantId)
        {
            return Get<ApplicantSummary>($"{Constants.Route.Applicants}/{applicantId}");
        }

        public Task<ApplicantUpdateInfo> UpdateApplicantInfo(Guid applicantId, ApplicantUpdateInfo applicant)
        {
            return Put<ApplicantUpdateInfo>($"{Constants.Route.Applicants}/{applicantId}/", applicant);
        }

        public Task<UpsertAcademicRecordResult> UpsertAcademicRecord(Guid applicantId, AcademicRecordBase academicRecord)
        {
            return Post<UpsertAcademicRecordResult>($"{Constants.Route.Applicants}/{applicantId}/{Constants.Actions.AcademicRecord}", academicRecord);
        }

        public Task<PagedResult<ApplicantHistory>> GetApplicantHistories(Guid applicantId, Guid? applicationId, GetApplicantHistoryOptions options)
        {
            var queryParams = new Dictionary<string, string>();

            if (applicationId.HasValue) queryParams.Add("applicationId", applicationId.ToString());
            if (options.Page.HasValue) queryParams.Add("page", options.Page.ToString());
            if (options.PageSize.HasValue) queryParams.Add("pageSize", options.PageSize.ToString());
            if (!string.IsNullOrEmpty(options.FromDate)) queryParams.Add("fromDate", options.FromDate);
            if (!string.IsNullOrEmpty(options.ToDate)) queryParams.Add("toDate", options.ToDate);

            return Get<PagedResult<ApplicantHistory>>(QueryHelpers.AddQueryString($"{Constants.Route.Applicants}/{applicantId}/{Constants.Actions.ApplicantHistories}", queryParams));
        }
    }
}
