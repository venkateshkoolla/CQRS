using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public void PatchVoucher(Voucher model, ocaslr_voucher entity)
        {
            entity.ocaslr_Order = model.OrderId.ToEntityReference(SalesOrder.EntityLogicalName);
            entity.ocaslr_OrderItem = model.OrderDetailId.HasValue ? model.OrderDetailId.Value.ToString() : string.Empty;
            entity.ocaslr_Applicant = model.ApplicantId.ToEntityReference(Crm.Entities.Contact.EntityLogicalName);
            entity.ocaslr_Application = model.ApplicationId.ToEntityReference(ocaslr_application.EntityLogicalName);
            entity.ocaslr_ShoppingCart = model.ShoppingCartId.ToEntityReference(Opportunity.EntityLogicalName);
            entity.ocaslr_ShoppingCartItem = model.ShoppingCartDetailId.HasValue ? model.ShoppingCartDetailId.Value.ToString() : string.Empty;
            entity.ocaslr_Source = model.SourceId.ToEntityReference(ocaslr_source.EntityLogicalName);

            if (entity.ocaslr_StateEnum != (ocaslr_voucher_ocaslr_State?)model.VoucherState)
                entity.ocaslr_StateEnum = (ocaslr_voucher_ocaslr_State?)model.VoucherState;
        }
    }
}
