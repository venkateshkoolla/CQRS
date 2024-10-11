using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moneris;
using Ocas.Domestic.Apply.Settings;

namespace Ocas.Domestic.Apply.Services.Clients
{
    public class MonerisClient : IMonerisClient
    {
        private readonly ILogger _logger;
        private readonly ILookupsCacheBase _lookupsCache;
        private readonly IAppSettingsBase _appSettings;

        public MonerisClient(ILogger<MonerisClient> logger, ILookupsCacheBase lookupsCache, IAppSettingsBase appSettings)
        {
            _logger = logger;
            _lookupsCache = lookupsCache;
            _appSettings = appSettings;
        }

        public async Task<ChargeCardResult> ChargeCard(string paymentToken, decimal amount, string customerId, string orderNumber, string email, string cvd, string expDate, string cardHolder)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var host = _appSettings.GetAppSetting(Constants.AppSettings.MonerisHost);
            var storeId = _appSettings.GetAppSetting(Constants.AppSettings.MonerisStoreId);
            var apiToken = _appSettings.GetAppSetting(Constants.AppSettings.MonerisApiKey);

            var purchase = new ResPurchaseCC(
                paymentToken,
                orderNumber,
                customerId,
                amount.ToString("C").Replace("$", string.Empty),
                Constants.Moneris.CryptType);
            purchase.SetExpDate(expDate);

            var cvdInfo = new CvdInfo();
            cvdInfo.SetCvdIndicator("1");
            cvdInfo.SetCvdValue(cvd);
            purchase.SetCvdInfo(cvdInfo);

            var custInfo = new CustInfo();
            custInfo.SetEmail(email);
            custInfo.SetBilling(
                               string.Empty, //first name
                               string.Empty, //last name
                               string.Empty, //company name
                               string.Empty, //address
                               string.Empty, //city
                               string.Empty, //province
                               string.Empty, //postal code
                               string.Empty, //country
                               string.Empty, //phone
                               string.Empty, //fax
                               string.Empty, //federal tax
                               string.Empty, //prov tax
                               string.Empty, //luxury tax
                               string.Empty); //shipping cost

            custInfo.SetShipping(
                               string.Empty, //first name
                               string.Empty, //last name
                               string.Empty, //company name
                               string.Empty, //address
                               string.Empty, //city
                               string.Empty, //province
                               string.Empty, //postal code
                               string.Empty, //country
                               string.Empty, //phone
                               string.Empty, //fax
                               string.Empty, //federal tax
                               string.Empty, //prov tax
                               string.Empty, //luxury tax
                               string.Empty); //shipping cost

            purchase.SetCustInfo(custInfo);

            var mpgReq = new HttpsPostRequest(host, storeId, apiToken, purchase);
            mpgReq.SetProcCountryCode(Constants.Moneris.Country);
            mpgReq.Send();

            var receipt = mpgReq.GetReceipt();

            /* Per Moneris documentation:
             * Financial Transaction Responses (i.e. ResPurchase)
             * < 50    Transaction approved
             * >= 50   Transaction declined
             * NULL    Transaction was not sent for authorization
             */
            var hasResponseCode = int.TryParse(receipt.GetResponseCode(), out var responseCode);
            if (!hasResponseCode)
            {
                _logger.LogWarning($"Transaction was not sent to financial institution for authorization (invalid card, csc, expiry, or duplicate order #): {receipt.GetMessage()}.");
                return new ChargeCardResult
                {
                    ChargeSuccess = false,
                    ReceiptBase = new Domestic.Models.ReceiptBase
                    {
                        ResponseCode = receipt.GetResponseCode(),
                        BankApprovalCode = receipt.GetAuthCode(),
                        BankTransactionId = receipt.GetReferenceNum(),
                        Message = receipt.GetMessage(),
                        AvsResponseCode = receipt.GetAvsResponseCode(),
                        CvdResponseCode = receipt.GetCvvResponseCode()
                    }
                };
            }

            if (responseCode >= 50)
            {
                _logger.LogWarning($"Card declined. Moneris responded with code: {receipt.GetResponseCode()} and message: {receipt.GetMessage()}");
                return new ChargeCardResult
                {
                    ChargeSuccess = false,
                    ReceiptBase = new Domestic.Models.ReceiptBase
                    {
                        ResponseCode = receipt.GetResponseCode(),
                        BankApprovalCode = receipt.GetAuthCode(),
                        BankTransactionId = receipt.GetReferenceNum(),
                        Message = receipt.GetMessage(),
                        AvsResponseCode = receipt.GetAvsResponseCode(),
                        CvdResponseCode = receipt.GetCvvResponseCode()
                    }
                };
            }

            var paymentMethods = await _lookupsCache.GetPaymentMethods(Constants.Localization.EnglishCanada);
            var paymentResults = await _lookupsCache.GetPaymentResults();
            var paymentMethodCode = receipt.GetCardType();
            var paymentMethodId = paymentMethods.FirstOrDefault(x => x.Code == paymentMethodCode)?.Id;
            var paymentResultId = paymentResults.First(x => x.Code == Constants.PaymentResults.Approved).Id;
            var isoParsed = int.TryParse(receipt.GetISO(), out var isoCode);
            var eciParsed = int.TryParse(receipt.GetMpiEci(), out var eciCode);

            var currencies = await _lookupsCache.GetCurrencies();
            var currencyId = currencies.First(x => x.Code == Constants.Currency.Cad).Id;

            return new ChargeCardResult
            {
                ChargeSuccess = true,
                ReceiptBase = new Domestic.Models.ReceiptBase
                {
                    Name = orderNumber,
                    ChargeTotal = amount,
                    IsoCode = isoParsed ? isoCode : (int?)null,
                    PaymentMethodId = paymentMethodId,
                    PaymentResultId = paymentResultId,
                    ResponseCode = receipt.GetResponseCode(),
                    ResponseOrderId = orderNumber,
                    TransactionCurrencyId = currencyId,
                    TransactionName = "purchase",
                    BankApprovalCode = receipt.GetAuthCode(),
                    BankTransactionId = receipt.GetReferenceNum(),
                    Cardholder = cardHolder,
                    TimeStamp = DateTime.UtcNow,
                    Message = receipt.GetMessage(),
                    AvsResponseCode = receipt.GetAvsResponseCode(),
                    CvdResponseCode = receipt.GetCvvResponseCode(),
                    Eci = eciParsed ? eciCode : (int?)null,
                    GatewayTransactionId = receipt.GetTxnNumber(),
                    MaskedPan = receipt.GetMaskedPan()
                }
            };
        }
    }
}
