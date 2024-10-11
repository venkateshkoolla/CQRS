using System.Collections.Generic;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Services.Mappers
{
    public partial class ApiMapperBase
    {
        public IList<ShoppingCartDetail> MapShoppingCartDetail(IList<Dto.ShoppingCartDetail> shoppingCartDetails, Dto.Contact applicant, Dto.Application application)
        {
            var details = _mapper.Map<IList<ShoppingCartDetail>>(shoppingCartDetails);

            // Get NSF balance
            if (applicant.NsfBalance.HasValue && applicant.NsfBalance.Value != 0)
            {
                var nsf = new ShoppingCartDetail
                {
                    Type = ShoppingCartItemType.NsfFee,
                    Amount = applicant.NsfBalance
                };
                details.Add(nsf);
            }

            // Returned payment
            if (applicant.ReturnedPayment.HasValue && applicant.ReturnedPayment.Value != 0)
            {
                var rpt = new ShoppingCartDetail
                {
                    Type = ShoppingCartItemType.ReturnedPayment,
                    Amount = applicant.ReturnedPayment
                };
                details.Add(rpt);
            }

            // Account Balance
            if (applicant.Balance.HasValue && applicant.Balance.Value != 0)
            {
                var acb = new ShoppingCartDetail
                {
                    Type = ShoppingCartItemType.AccountCredit,
                    Amount = -1.00M * applicant.Balance.Value
                };
                details.Add(acb);
            }

            // Over payment
            if (applicant.OverPayment.HasValue && applicant.OverPayment.Value != 0)
            {
                var acb = new ShoppingCartDetail
                {
                    Type = ShoppingCartItemType.Overpayment,
                    Amount = -1.00M * applicant.OverPayment.Value
                };
                details.Add(acb);
            }

            // Transfer balance
            if (application.Balance.HasValue && application.Balance.Value != 0)
            {
                var tb = new ShoppingCartDetail
                {
                    Type = ShoppingCartItemType.TransferBalance,
                    Amount = -1.00M * application.Balance.Value
                };
                details.Add(tb);
            }

            return details;
        }
    }
}
