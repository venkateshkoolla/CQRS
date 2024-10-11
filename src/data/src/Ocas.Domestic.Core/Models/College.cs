using System;

namespace Ocas.Domestic.Models
{
    public class College : IAccount
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string LocalizedName { get; set; }
        public string Name { get; set; }
        public Guid SchoolStatusId { get; set; }
        public bool HasEtms { get; set; }
        public decimal? TranscriptFee { get; set; }
        public bool AllowCba { get; set; }
        public bool AllowCbaBrandConfig { get; set; }
        public bool AllowCbaMultiCollegeApply { get; set; }
        public bool AllowCbaReferralCodeAsSource { get; set; }
        public bool AllowCbaSearch { get; set; }
        public bool AllowCbaCobranding { get; set; }
        public bool ShowInEducation { get; set; }
        public Address MailingAddress { get; set; }
    }
}
