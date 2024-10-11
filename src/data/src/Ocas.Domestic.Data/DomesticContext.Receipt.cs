using System;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Receipt> GetReceipt(Guid id)
        {
            return CrmExtrasProvider.GetReceipt(id);
        }
    }
}
