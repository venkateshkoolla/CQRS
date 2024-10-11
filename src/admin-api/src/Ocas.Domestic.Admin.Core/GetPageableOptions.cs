using Ocas.Domestic.Apply.Admin.Enums;

namespace Ocas.Domestic.Apply.Admin
{
    public abstract class GetPageableOptions
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public SortDirection? SortDirection { get; set; }
    }
}
