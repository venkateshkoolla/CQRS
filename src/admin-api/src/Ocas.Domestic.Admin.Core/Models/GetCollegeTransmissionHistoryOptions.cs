using Ocas.Domestic.Apply.Admin.Enums;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class GetCollegeTransmissionHistoryOptions : GetPageableOptions
    {
        public GetCollegeTransmissionHistoryOptions()
        {
            Page = 1;
            PageSize = 20;
        }
        public CollegeTransmissionActivity? Activity { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
