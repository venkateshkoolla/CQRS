using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public ocaslr_paymenttransactiondetail MapReceipt(ReceiptBase receiptBase)
        {
            var entity = new ocaslr_paymenttransactiondetail
            {
                ocaslr_name = receiptBase.Name,
                ocaslr_banktransactionid = receiptBase.BankTransactionId,
                ocaslr_chargetotal = receiptBase.ChargeTotal.ToMoney(),
                ocaslr_isocode = receiptBase.IsoCode,
                ocaslr_paymentmethodid = receiptBase.PaymentMethodId.ToEntityReference(ocaslr_paymentmethod.EntityLogicalName),
                ocaslr_responseorderid = receiptBase.ResponseOrderId,
                ocaslr_responsecode = receiptBase.ResponseCode,
                ocaslr_bankapprovalcode = receiptBase.BankApprovalCode,
                ocaslr_cardholder = receiptBase.Cardholder,
                ocaslr_datetimestamp = receiptBase.TimeStamp,
                ocaslr_message = receiptBase.Message,
                ocaslr_paymentresultid = receiptBase.PaymentResultId.ToEntityReference(ocaslr_paymentresult.EntityLogicalName),
                ocaslr_transactionname = receiptBase.TransactionName,
                ocaslr_verificationstatusEnum = ocaslr_paymenttransactiondetail_ocaslr_verificationstatus.Verified,
                ocaslr_avsresponsecode = receiptBase.AvsResponseCode?.Substring(0, 1),
                ocaslr_cvdresponsecode = receiptBase.CvdResponseCode?.Substring(0, 1),
                ocaslr_eci = receiptBase.Eci,
                ocaslr_invoice = receiptBase.Invoice,
                ocaslr_issconf = receiptBase.IssConf,
                ocaslr_issname = receiptBase.IssName,
                ocaslr_transactionkey = receiptBase.TransactionKey,
                ocaslr_gatewaytransacationidentifier = receiptBase.GatewayTransactionId,
                ocaslr_first4andlast4 = receiptBase.MaskedPan
            };

            return entity;
        }
    }
}