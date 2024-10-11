using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public ShoppingCart MapShoppingCart(Dto.ShoppingCart shoppingCart, Dto.Contact applicant)
        {
            var model = _mapper.Map<ShoppingCart>(shoppingCart);

            // Get NSF balance
            if (applicant.NsfBalance.HasValue && applicant.NsfBalance.Value != 0)
            {
                var nsf = new ShoppingCartDetail
                {
                    Type = ShoppingCartItemType.NsfFee,
                    Amount = applicant.NsfBalance
                };
                model.Details.Add(nsf);
            }

            // Returned payment
            if (applicant.ReturnedPayment.HasValue && applicant.ReturnedPayment.Value != 0)
            {
                var rpt = new ShoppingCartDetail
                {
                    Type = ShoppingCartItemType.ReturnedPayment,
                    Amount = applicant.ReturnedPayment
                };
                model.Details.Add(rpt);
            }

            // Account Balance
            if (applicant.Balance.HasValue && applicant.Balance.Value != 0)
            {
                var acb = new ShoppingCartDetail
                {
                    Type = ShoppingCartItemType.AccountCredit,
                    Amount = -1.00M * applicant.Balance.Value
                };
                model.Details.Add(acb);
            }

            // Over payment
            if (applicant.OverPayment.HasValue && applicant.OverPayment.Value != 0)
            {
                var acb = new ShoppingCartDetail
                {
                    Type = ShoppingCartItemType.Overpayment,
                    Amount = -1.00M * applicant.OverPayment.Value
                };
                model.Details.Add(acb);
            }

            return model;
        }
    }
}
