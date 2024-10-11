using Ocas.Domestic.Admin.Enums;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class GetIntakeApplicantOptions: GetPageableOptions
    {
        public GetIntakeApplicantOptions()
        {
            Page = 1;
            PageSize = 100;
            SortDirection = Enums.SortDirection.Ascending;
            SortBy = IntakeApplicantSortField.Number;
        }

        public IntakeApplicantSortField? SortBy { get; set; }
    }
}
