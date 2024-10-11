using System;

namespace Ocas.Domestic.Models
{
    public interface IAuditable
    {
        DateTime? CreatedOn { get; set; }
        DateTime? ModifiedOn { get; set; }
        string ModifiedBy { get; set; }
    }

    public abstract class Auditable : IAuditable
    {
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
