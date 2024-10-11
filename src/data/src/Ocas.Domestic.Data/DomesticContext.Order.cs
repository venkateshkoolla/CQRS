using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Data.Mappers;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Task = System.Threading.Tasks.Task;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Order> GetOrder(Guid id)
        {
            return CrmExtrasProvider.GetOrder(id);
        }

        public Task<Order> GetOrder(string orderNumber)
        {
            return CrmExtrasProvider.GetOrder(orderNumber);
        }

        public Task<IList<Order>> GetOrders(GetOrderOptions options)
        {
            return CrmExtrasProvider.GetOrders(options);
        }

        public Task DeleteOrderDetail(Guid orderDetailId)
        {
            return CrmProvider.DeleteEntity(SalesOrderDetail.EntityLogicalName, orderDetailId);
        }

        public async Task<Order> UpdateOrder(Order model)
        {
            var entity = CrmProvider.Orders.First(x => x.Id == model.Id);

            CrmMapper.PatchOrder(model, entity);
            await CrmProvider.UpdateEntity(entity);

            return await GetOrder(model.Id);
        }

        // From A2C: https://dev.azure.com/ocas/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Services%2FOrder%2FOrderService.cs&version=GBmaster&line=136&lineStyle=plain&lineEnd=163&lineStartColumn=9&lineEndColumn=10
        public async Task<Order> CreateOrder(Guid applicationId, Guid applicantId, string modifiedBy, Guid sourceId, ShoppingCart shoppingCart)
        {
            var entity = new SalesOrder
            {
                ocaslr_applicationid = applicationId.ToEntityReference(ocaslr_application.EntityLogicalName),
                CustomerId = applicantId.ToEntityReference(Crm.Entities.Contact.EntityLogicalName),
                TransactionCurrencyId = shoppingCart.TransactionCurrencyId.ToEntityReference(TransactionCurrency.EntityLogicalName),
                PriceLevelId = shoppingCart.PriceLevelId.ToEntityReference(PriceLevel.EntityLogicalName),
                ocaslr_modifiedbyuser = modifiedBy,
                ocaslr_orderpaymentstatus = ((int)SalesOrder_ocaslr_orderpaymentstatus.Pending).ToOptionSet<SalesOrder_ocaslr_orderpaymentstatus>(),
                ocaslr_PaymentSource = sourceId.ToEntityReference(ocaslr_source.EntityLogicalName)
            };

            var id = await CrmProvider.CreateEntity(entity);

            return await GetOrder(id);
        }

        // From A2C: https://dev.azure.com/ocas/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Providers%2FOrderItem%2FOrderItemProvider.cs&version=GBmaster&line=98&lineStyle=plain&lineEnd=137&lineStartColumn=9&lineEndColumn=10
        public async Task<OrderDetail> CreateOrderDetail(Order order, ShoppingCartDetail shoppingCartItem)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            if (shoppingCartItem is null)
                throw new ArgumentNullException(nameof(shoppingCartItem));

            var productDescription = shoppingCartItem.ProductId.HasValue ? null : shoppingCartItem.Description;
            if (shoppingCartItem.Type == ShoppingCartItemType.Voucher)
                productDescription = Constants.Product.Voucher;

            //create new order item and associate it with order and program choice
            var entity = new SalesOrderDetail
            {
                SalesOrderId = order.Id.ToEntityReference(SalesOrder.EntityLogicalName),
                ProductId = shoppingCartItem.ProductId.ToEntityReference(Crm.Entities.Product.EntityLogicalName),
                ProductDescription = productDescription,
                Quantity = shoppingCartItem.Quantity,
                UoMId = shoppingCartItem.UomId.ToEntityReference(UoM.EntityLogicalName),
                ocaslr_applicationid = shoppingCartItem.ApplicationId.ToEntityReference(ocaslr_application.EntityLogicalName),
                ocaslr_programchoiceid = shoppingCartItem.ProgramChoiceId.ToEntityReference(ocaslr_programchoice.EntityLogicalName),
                ocaslr_transcriptrequestid = shoppingCartItem.TranscriptRequestId.ToEntityReference(ocaslr_transcriptrequest.EntityLogicalName),
                ocaslr_Voucher = shoppingCartItem.VoucherId.ToEntityReference(ocaslr_voucher.EntityLogicalName)
            };

            if (!string.IsNullOrEmpty(entity.ProductDescription) && entity.ProductDescription == Constants.Product.Voucher)
            {
                entity.IsProductOverridden = true;
            }

            if (shoppingCartItem.VoucherId != null)
            {
                entity.ManualDiscountAmount = shoppingCartItem.ManualDiscountAmount.ToMoney();
                entity.PricePerUnit = shoppingCartItem.PricePerUnit.ToMoney();
            }

            var id = await CrmProvider.CreateEntity(entity);

            var newOrder = await GetOrder(order.Id);

            return newOrder.Details.First(x => x.Id == id);
        }
    }
}
