using System;
using Ocas.Domestic.Admin.Enums;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class GetIntakesOptions
    {
        public GetIntakesOptions()
        {
            SortDirection = Enums.SortDirection.Ascending;
            SortBy = IntakeSortField.Title;
        }

        public string ProgramCode { get; set; }
        public string ProgramTitle { get; set; }
        public Guid? DeliveryId { get; set; }
        public Guid? CampusId { get; set; }
        public string StartDate { get; set; }
        public IntakeSortField? SortBy { get; set; }
        public Enums.SortDirection? SortDirection { get; set; }
        public string Props { get; set; }
    }
}
