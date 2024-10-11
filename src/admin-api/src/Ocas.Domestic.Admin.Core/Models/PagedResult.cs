using System.Collections.Generic;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class PagedResult<T>
    {
        public long TotalCount { get; set; }
        public IList<T> Items { get; set; }
    }
}
