using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Services.Mappers
{
    public partial class ApiMapperBase
    {
        public Order MapOrder(Dto.Order dbDto, Dto.FinancialTransaction financialTransaction = null)
        {
            var model = _mapper.Map<Order>(dbDto);

            if (financialTransaction is null)
                return model;

            if (financialTransaction.ReturnedPaymentPaid.HasValue && financialTransaction.ReturnedPaymentPaid.Value != 0)
            {
                var detail = new OrderDetail
                {
                    Type = ShoppingCartItemType.ReturnedPayment,
                    Amount = financialTransaction.ReturnedPaymentPaid
                };

                model.Details.Add(detail);
            }

            if (financialTransaction.ReturnedPaymentChargePaid.HasValue && financialTransaction.ReturnedPaymentChargePaid.Value != 0)
            {
                var detail = new OrderDetail
                {
                    Type = ShoppingCartItemType.NsfFee,
                    Amount = financialTransaction.ReturnedPaymentChargePaid
                };

                model.Details.Add(detail);
            }

            return model;
        }
    }
}
