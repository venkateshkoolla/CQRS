﻿using System.Collections.Generic;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class ApplicationSummary
    {
        public Application Application { get; set; }
        public IList<Offer> Offers { get; set; }
        public IList<ProgramChoice> ProgramChoices { get; set; }
        public IList<TranscriptRequest> TranscriptRequests { get; set; }
        public IList<FinancialTransaction> FinancialTransactions { get; set; }
        public IList<ShoppingCartDetail> ShoppingCartDetails { get; set; }
    }
}
