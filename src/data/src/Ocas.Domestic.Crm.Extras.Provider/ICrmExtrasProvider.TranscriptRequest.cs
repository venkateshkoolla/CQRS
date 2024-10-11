﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<TranscriptRequest> GetTranscriptRequest(Guid id);
        Task<IList<TranscriptRequest>> GetTranscriptRequests(GetTranscriptRequestOptions options);
    }
}
