using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xrm.Client;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Data.Mappers;
using Ocas.Domestic.Data.Utils;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Task = System.Threading.Tasks.Task;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Voucher> GetVoucher(GetVoucherOptions options)
        {
            return CrmExtrasProvider.GetVoucher(options);
        }

        public async Task<Voucher> UpdateVoucher(Voucher voucher)
        {
            var crmEntity = CrmProvider.Vouchers.First(x => x.Id == voucher.Id);

            CrmMapper.PatchVoucher(voucher, crmEntity);
            await CrmProvider.UpdateEntity(crmEntity);

            return await GetVoucher(new GetVoucherOptions { Id = crmEntity.Id });
        }

        public async Task<Voucher> LinkOrderToVoucher(Voucher voucher)
        {
            var crmEntity = new ocaslr_voucher
            {
                Id = voucher.Id,
                ocaslr_Order = new CrmEntityReference(SalesOrder.EntityLogicalName, voucher.OrderId.Value),
                ocaslr_OrderItem = voucher.OrderDetailId.ToString()
            };
            await CrmProvider.UpdateEntity(crmEntity);

            return await GetVoucher(new GetVoucherOptions { Id = crmEntity.Id });
        }

        // From A2C: https://dev.azure.com/ocas/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Services%2FVoucher%2FVoucherService.cs&version=GBmaster&line=168&lineStyle=plain&lineEnd=208&lineStartColumn=1&lineEndColumn=1
        public async Task AddVoucherToShoppingCart(Voucher model, Guid applicationId, Guid applicantId, Guid shoppingCartId, Guid sourceId, string modifiedBy)
        {
            if (!model.Value.HasValue)
                throw new Exception($"Value missing off Voucher: {model.Id}");

            var shoppingCartItem = new OpportunityProduct
            {
                IsProductOverridden = true,
                ProductDescription = Constants.Product.Voucher,
                ocaslr_applicationid = applicationId.ToEntityReference(ocaslr_application.EntityLogicalName),
                ocaslr_Voucher = model.Id.ToEntityReference(ocaslr_voucher.EntityLogicalName),
                PricePerUnit = (model.Value.Value * -1.0M).ToMoney(),
                Quantity = 1,
                OpportunityId = shoppingCartId.ToEntityReference(Opportunity.EntityLogicalName),
                OCASLR_ModifiedByUser = modifiedBy
            };

            //Add new shopping cart item
            var id = await CrmProvider.CreateEntity(shoppingCartItem);

            if (id.IsEmpty())
            {
                throw new Exception($"Failed to create OpportunityProduct for Voucher {model.Id} and ShoppingCart {shoppingCartId}");
            }

            model.ApplicantId = applicantId;
            model.ApplicationId = applicationId;
            model.ShoppingCartDetailId = id;
            model.ShoppingCartId = shoppingCartId;
            model.SourceId = sourceId;
            model.VoucherState = VoucherState.Claimed;

            var result = await UpdateVoucher(model);

            if (result is null)
            {
                throw new Exception($"Failed to update Voucher {model.Id} and ShoppingCart {shoppingCartId}");
            }
        }

        // From A2C: https://dev.azure.com/ocas/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Services%2FVoucher%2FVoucherService.cs&version=GBmaster&line=212&lineStyle=plain&lineEnd=301&lineStartColumn=9&lineEndColumn=10
        public async Task RemoveVoucherFromShoppingCart(Voucher model, string modifiedBy)
        {
            if (model.ShoppingCartId is null)
                throw new Exception($"ShoppingCartId missing off Voucher: {model.Code}");

            if (model.ShoppingCartDetailId is null)
                throw new Exception($"ShoppingCartDetailId missing off Voucher: {model.Code}");

            var applicantId = model.ApplicantId;
            var shoppingCartDetailId = model.ShoppingCartDetailId.Value;

            if (model.VoucherState != VoucherState.Claimed)
                throw new Exception($"Voucher is not in claimed state: {model.Code}");

            model.VoucherState = VoucherState.Issued;
            model.ApplicantId = null;
            model.ApplicationId = null;
            model.ShoppingCartId = null;
            model.ShoppingCartDetailId = null;
            model.OrderId = null;
            model.OrderDetailId = null;
            model.SourceId = null;

            await UpdateVoucher(model);

            await DeleteShoppingCartDetail(shoppingCartDetailId);

            if (applicantId.HasValue)
            {
                var contact = await GetContact(applicantId.Value);

                if (contact != null)
                {
                    var orders = await GetOrders(new GetOrderOptions
                    {
                        CustomerId = applicantId
                    });

                    var unpaidOrdersWithThisVoucher = orders
                        .Where(x => x.Details.Any(
                                        y => y.Type == ShoppingCartItemType.Voucher && y.VoucherId == model.Id)
                                    && x.OrderPaymentStatus.HasValue
                                    && (x.OrderPaymentStatus == OrderPaymentStatus.CheckedOut
                                        || x.OrderPaymentStatus == OrderPaymentStatus.Pending)).ToList();

                    foreach (var order in unpaidOrdersWithThisVoucher)
                    {
                        foreach (var voucherOrderDetail in order.Details.Where(x =>
                            x.Type == ShoppingCartItemType.Voucher && x.VoucherId == model.Id))
                        {
                            // Recalculate order
                            await RemoveVoucherFromOrder(order, voucherOrderDetail, contact, modifiedBy);
                        }
                    }
                }
            }
        }

        // From A2C: https://dev.azure.com/ocas/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Services%2FOrder%2FOrderService.cs&version=GBmaster&line=286&lineStyle=plain&lineEnd=323&lineStartColumn=9&lineEndColumn=10
        public async Task RemoveVoucherFromOrder(Order order, OrderDetail orderDetail, Models.Contact contact, string modifiedBy)
        {
            if (order.ApplicationId is null)
                throw new Exception($"ApplicationId is missing off SalesOrder entity: {order.Id}");

            if (order.OrderPaymentStatus != OrderPaymentStatus.Pending && order.OrderPaymentStatus != OrderPaymentStatus.CheckedOut)
                throw new Exception($"Order is not in Pending or CheckedOut state: {order.Id}");

            if (orderDetail.Type != ShoppingCartItemType.Voucher)
                throw new Exception($"OrderDetail is not a Voucher: {orderDetail.Id}");

            if (orderDetail.Amount is null)
                throw new Exception($"BaseAmount is missing off SalesOrderDetail entity: {orderDetail.Id}");

            var crmEntity = CrmProvider.Contacts.First(x => x.Id == contact.Id);
            if (crmEntity is null)
                throw new Exception($"Contact does not exist: {contact.Id}");

            await DeleteOrderDetail(orderDetail.Id);

            // let CRM recalculate TotalAmount now that the voucher has been removed
            order = await GetOrder(order.Id);

            var overpaymentBalance = contact.OverPayment ?? 0M;
            var accountBalance = contact.Balance ?? 0M;
            var nsfBalance = contact.NsfBalance ?? 0M;
            var returnedPaymentBalance = contact.ReturnedPayment ?? 0M;

            var application = await GetApplication(order.ApplicationId.Value);
            var transferBalance = application?.Balance ?? 0M;

            order.AmountPaidFromVoucher = null;
            order.ModifiedBy = modifiedBy;
            order.FinalTotal = Math.Max(order.TotalAmount - accountBalance - transferBalance - overpaymentBalance + nsfBalance + returnedPaymentBalance, 0);

            await UpdateOrder(order);
        }
    }
}
