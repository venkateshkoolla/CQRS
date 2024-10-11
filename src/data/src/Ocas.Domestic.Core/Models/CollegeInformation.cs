using System;

namespace Ocas.Domestic.Models
{
    public class CollegeInformation
    {
        public Guid Id { get; set; }
        public Guid CollegeId { get; set; }
        public string CollegeWebsiteURL { get; set; }
        public string LocalizedWelcomeText { get; set; }
        public string RevokedOfferPhoneAreaCode { get; set; }
        public string RevokedOfferPhoneNumber { get; set; }
        public string RevokedInformationUrl { get; set; }
        public string SuspendedInformationUrl { get; set; }
        public string AuditReportEmail { get; set; }
        public string FullLogoPath { get; set; }
    }
}
