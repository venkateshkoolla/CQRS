using System;
using Ocas.Domestic.Apply.Models.Lookups;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class SubCategory : LookupItem
    {
        public Guid CategoryId { get; set; }
    }
}
