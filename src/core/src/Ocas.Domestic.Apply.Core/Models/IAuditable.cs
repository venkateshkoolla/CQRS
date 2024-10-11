using System;

namespace Ocas.Domestic.Apply.Models
{
    public interface IAuditable
    {
        DateTime ModifiedOn { get; set; }
        string ModifiedBy { get; set; }
    }

    public abstract class Auditable : IAuditable
    {
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
