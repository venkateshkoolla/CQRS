﻿using System;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<OrderDetail> GetOrderDetail(Guid orderDetailId);
    }
}