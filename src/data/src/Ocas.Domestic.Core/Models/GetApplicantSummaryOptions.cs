using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class GetApplicantSummaryOptions
    {
        public Guid ApplicantId { get; set; }
        public Guid? ApplicationId { get; set; }
        public Guid? ApplicationStatusId { get; set; }
        public Locale Locale { get; set; }
        public bool IncludeShoppingCartDetails { get; set; } = true;
        public bool IncludeFinancialTransactions { get; set; } = true;
        public bool IncludeTranscriptRequests { get; set; } = true;
    }
}
