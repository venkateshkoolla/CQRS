using System;

namespace Ocas.Domestic.Models
{
    public class GetApplicantMessageOptions
    {
        public Guid ApplicantId { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
