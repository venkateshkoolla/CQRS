using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Services.Messages
{
    public class PayOrder : IRequest<FinancialTransaction>, IIdentityUser
    {
        public IPrincipal User { get; set; }
        public Guid OrderId { get; set; }
        public PayOrderInfo PayOrderInfo { get; set; }
        public bool IsOfflinePayment { get; set; }
        public OfflinePaymentInfo OfflinePaymentInfo { get; set; }
    }
}