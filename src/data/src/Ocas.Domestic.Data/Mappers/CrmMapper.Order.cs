using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public void PatchOrder(Order model, SalesOrder entity)
        {
            entity.ocaslr_PaidfromVoucher = model.AmountPaidFromVoucher.ToMoney();
            entity.ocaslr_federaltax = model.FederalTax.ToMoney();
            entity.ocaslr_provincialtax = model.ProvincialTax.ToMoney();
            entity.ocaslr_modifiedbyuser = model.ModifiedBy;
            entity.ocaslr_finaltotal = model.FinalTotal.ToMoney();
            entity.ocaslr_paymenttypeEnum = (SalesOrder_ocaslr_paymenttype?)model.OrderPaymentType;
            entity.ocaslr_orderpaymentstatusEnum = (SalesOrder_ocaslr_orderpaymentstatus?)model.OrderPaymentStatus;
            entity.ocaslr_paymentamount = model.PaymentAmount.ToMoney();
            entity.ocaslr_transactionamount = model.TransactionAmount.ToMoney();
            entity.ocaslr_paymentmethodid = model.PaymentMethodId.ToEntityReference(ocaslr_paymentmethod.EntityLogicalName);
            entity.ocaslr_paymentresponsecode = model.PaymentResponseCode;
            entity.ocaslr_orderconfirmationnumber = model.OrderConfirmationNumber;
            entity.ocaslr_paidagainstnsfbalance = model.AmountPaidAgainstNsfBalance.ToMoney();
            entity.ocaslr_paidagainstreturnedpaymentbalance = model.AmountPaidAgainstReturnedPaymentBalance.ToMoney();
            entity.ocaslr_paidtoaccountbalance = model.AmountPaidToAccountBalance.ToMoney();
            entity.ocaslr_paidtooverpaymentbalance = model.AmountPaidToOverpaymentBalance.ToMoney();
            entity.ocaslr_paidfromoverpaymentbalance = model.AmountPaidFromOverpaymentBalance.ToMoney();
            entity.ocaslr_paidfromaccountbalance = model.AmountPaidFromAccountBalance.ToMoney();
            entity.ocaslr_paidfromtransferbalance = model.AmountPaidFromTransferBalance.ToMoney();
            entity.ocaslr_paymentdetails = model.PaymentDetails;
            entity.Ocaslr_note = model.Note;

            if (entity.ocaslr_orderprocessingstatusEnum != (SalesOrder_ocaslr_orderprocessingstatus?)model.OrderProcessingStatus)
                entity.ocaslr_orderprocessingstatusEnum = (SalesOrder_ocaslr_orderprocessingstatus?)model.OrderProcessingStatus;

            if (entity.ocaslr_transactiontypeEnum != (SalesOrder_ocaslr_transactiontype?)model.TransactionType)
                entity.ocaslr_transactiontypeEnum = (SalesOrder_ocaslr_transactiontype?)model.TransactionType;
        }
    }
}