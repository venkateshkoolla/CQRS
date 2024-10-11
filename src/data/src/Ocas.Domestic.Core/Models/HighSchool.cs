using System;

namespace Ocas.Domestic.Models
{
    public class HighSchool : IAccount
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string LocalizedName { get; set; }
        public string Name { get; set; }
        public Guid ParentId { get; set; }
        public decimal? TranscriptFee { get; set; }
        public bool HasEtms { get; set; }
        public Guid SchoolTypeId { get; set; }
        public string SchoolType { get; set; }
        public string SchoolBoardName { get; set; }
        public long SchoolId { get; set; }
        public Guid SchoolStatusId { get; set; }
        public string SchoolStatus { get; set; }
        public string AddressTypeCode { get; set; }
        public string Mident { get; set; }
        public bool ShowInEducation { get; set; }
        public Address MailingAddress { get; set; }
        public string BoardMident { get; set; }
    }
}
