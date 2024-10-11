using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<ShsmCompletion> GetShsmCompletion(Guid shsmCompletionId)
        {
            return CrmExtrasProvider.GetShsmCompletion(shsmCompletionId);
        }

        public Task<IList<ShsmCompletion>> GetShsmCompletions()
        {
            return CrmExtrasProvider.GetShsmCompletions();
        }
    }
}