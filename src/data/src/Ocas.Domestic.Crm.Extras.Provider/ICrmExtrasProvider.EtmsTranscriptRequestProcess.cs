﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<EtmsTranscriptRequestProcess> GetEtmsTranscriptRequestProcess(Guid id);
        Task<IList<EtmsTranscriptRequestProcess>> GetEtmsTranscriptRequestProcesses(Guid etmsTranscriptRequestId);
    }
}
