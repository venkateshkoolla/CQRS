using System;
using Ocas.Domestic.Apply.Admin.Enums;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class GetProgramBriefOptions
    {

        public GetProgramBriefOptions()
        {
            SortDirection = Enums.SortDirection.Ascending;
            SortBy = ProgramSortField.Title;
        }

        public Guid? CampusId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public Guid? DeliveryId { get; set; }
        public SortDirection? SortDirection { get; set; }
        public ProgramSortField? SortBy { get; set; }
    }
}
