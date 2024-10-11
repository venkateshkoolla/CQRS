using System;

namespace Ocas.Domestic.Apply.TestFramework.Models
{
    public class NewTestOfferInfo
    {
        public Guid Id { get; set; }
        public string ApplicationNumber { get; set; }
        public string CampusCode { get; set; }
        public string CollegeCode { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string EntryLevelCode { get; set; }
        public bool IsLateAdmit { get; set; }
        public int SequenceNumber { get; set; }
        public DateTime SoftExpiryDate { get; set; }
        public DateTime HardExpiryDate { get; set; }
        public string OfferStudyMethodCode { get; set; }
        public string OfferStateCode { get; set; }
        public string OfferStatusCode { get; set; }
        public string ProgramCode { get; set; }
        public string StartDate { get; set; }
        public string TermIdentifier { get; set; }
    }
}
