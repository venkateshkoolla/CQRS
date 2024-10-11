using System;

namespace Ocas.Domestic.Models
{
    public class GetOfferOptions
    {
        public Guid? ApplicationId { get; set; }
        public Guid? ApplicantId { get; set; }
        public Guid? ApplicationStatusId { get; set; }
    }
}
