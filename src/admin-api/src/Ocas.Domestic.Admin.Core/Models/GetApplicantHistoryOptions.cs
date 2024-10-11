using System;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class GetApplicantHistoryOptions : GetPageableOptions
    {
        public GetApplicantHistoryOptions()
        {
            Page = 1;
            PageSize = 20;
        }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
