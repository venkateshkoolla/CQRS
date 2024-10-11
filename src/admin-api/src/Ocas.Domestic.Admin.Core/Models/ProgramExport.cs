using System;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class ProgramExport
    {
        public Guid ProgramId { get; set; }
        public string ApplicationCycle { get; set; }
        public string CollegeCode { get; set; }
        public string CampusCode { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramTitle { get; set; }
        public string ProgramDelivery { get; set; }
        public string ProgramType { get; set; }
        public string Promotion { get; set; }
        public decimal? Length { get; set; }
        public string UnitOfMeasure { get; set; }
        public string AdultTraining { get; set; }
        public string ProgramSpecialCode { get; set; }
        public string ProgramSpecialCodeDescription { get; set; }
        public string Credential { get; set; }
        public int? ApsNumber { get; set; }
        public string StudyArea { get; set; }
        public string HighlyCompetitive { get; set; }
        public string ProgramLanguage { get; set; }
        public string ProgramEntryLevel { get; set; }
        public string McuCode { get; set; }
        public string McuDescription { get; set; }
        public string MinistryApproval { get; set; }
        public string Url { get; set; }
        public string ProgramCategory1 { get; set; }
        public string ProgramSubCategory1 { get; set; }
        public string ProgramCategory2 { get; set; }
        public string ProgramSubCategory2 { get; set; }
    }
}
