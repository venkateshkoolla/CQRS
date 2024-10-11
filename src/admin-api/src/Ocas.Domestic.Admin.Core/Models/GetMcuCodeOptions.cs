using Ocas.Domestic.Apply.Admin.Enums;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class GetMcuCodeOptions : GetSearchableOptions
    {
        public GetMcuCodeOptions()
        {
            Page = 1;
            PageSize = 100;
            SortDirection = Enums.SortDirection.Ascending;
            SortBy = McuCodeSortField.Title;
        }

        public McuCodeSortField SortBy { get; set; }
    }
}
