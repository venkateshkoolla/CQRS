using Ocas.Domestic.Apply.Admin.Enums;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class GetSpecialCodeOptions : GetSearchableOptions
    {
        public GetSpecialCodeOptions()
        {
            Page = 1;
            PageSize = 100;
            SortDirection = Enums.SortDirection.Ascending;
            SortBy = SpecialCodeSortField.Code;
        }

        public SpecialCodeSortField SortBy { get; set; }
    }
}
