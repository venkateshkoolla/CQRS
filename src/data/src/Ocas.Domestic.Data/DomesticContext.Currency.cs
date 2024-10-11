﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Currency> GetCurrency(Guid currencyId)
        {
            return CrmExtrasProvider.GetCurrency(currencyId);
        }

        public Task<IList<Currency>> GetCurrencies()
        {
            return CrmExtrasProvider.GetCurrencies();
        }
    }
}
