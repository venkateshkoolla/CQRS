using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<IList<Education>> GetEducations(Guid applicantId)
        {
            return Get<IList<Education>>(QueryHelpers.AddQueryString(Constants.Route.EducationRecords, "applicantId", applicantId.ToString()));
        }

        public Task<Education> PostEducation(EducationBase model)
        {
            return Post<Education>(Constants.Route.EducationRecords, model);
        }

        public Task<Education> UpdateEducation(Education model)
        {
            return Put<Education>($"{Constants.Route.EducationRecords}/{model.Id}", model);
        }

        public Task RemoveEducation(Guid educationId)
        {
            return Delete($"{Constants.Route.EducationRecords}/{educationId}");
        }
    }
}
