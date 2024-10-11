using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Data.Utils;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Extras
{
    public class DomesticContextExtras : IDomesticContextExtras
    {
        private readonly IDomesticContext _domesticContext;

        public DomesticContextExtras(IDomesticContext domesticContext)
        {
            _domesticContext = domesticContext;
        }

        // from CBA: https://dev.azure.com/ocas/OCAS%20Portfolio/_git/domesticapi?path=%2Fsrc%2FOCAS.Core.API%2FPlugin.CRM%2FRepository%2FCrmApplicantContactRepository.cs&version=GBmaster&line=1165&lineStyle=plain&lineEnd=1264&lineStartColumn=13&lineEndColumn=78
        public async Task<bool> PatchBasisForAdmission(Application application, Contact applicant, string modifiedBy, IList<BasisForAdmission> basisForAdmissions, IList<Current> currents, IList<ApplicationCycle> applicationCycles)
        {
            application = application ?? throw new ArgumentNullException(nameof(application));
            applicant = applicant ?? throw new ArgumentNullException(nameof(applicant));

            if (applicant.Id != application.ApplicantId)
                throw new ValidationException($"ApplicantId {applicant.Id} does not match Application {application.ApplicantId}");

            var hasChanges = false;
            if (applicant.HighSchoolEnrolled != null && (applicant.HighSchoolGraduated != null || applicant.HighSchoolGraduationDate != null))
            {
                // get all active selections
                var selections = await _domesticContext.GetProgramChoices(new GetProgramChoicesOptions
                {
                    ApplicationId = application.Id,
                    StateCode = State.Active
                });

                var basisForAdmissionId = application.BasisForAdmissionId;
                var currentId = application.CurrentId;
                var updateApplication = false;

                // currently enrolled?
                if (applicant.HighSchoolEnrolled == true && applicant.HighSchoolGraduationDate.HasValue)
                {
                    // determine start of cycle
                    var currentAppCycle = applicationCycles.First(q => q.Id == application.ApplicationCycleId);

                    var programStartDate = currentAppCycle.StartDate; // default to cycle start

                    if (selections.Any())
                    {
                        var programStart = selections.OrderBy(x => x.IntakeStartDate).Select(s => s.IntakeStartDate).FirstOrDefault();

                        if (programStart != null)
                        {
                            var programStartMonth = int.Parse(programStart.Substring(2, 2));
                            var programStartYear = 2000 + int.Parse(programStart.Substring(0, 2));

                            if (!string.IsNullOrWhiteSpace(programStart))
                            {
                                // make first day of month to compare to graduation date which is also set to first day of month when stored
                                programStartDate = new DateTime(programStartYear, programStartMonth, 1);
                            }
                        }
                    }

                    if (applicant.HighSchoolGraduationDate > programStartDate)
                    {
                        // is grad date >= first day of college? ==> BFA = 2, Current = Yes
                        basisForAdmissionId = basisForAdmissions.First(q => q.Code == Constants.BasisForAdmission.WillNotHaveOssd).Id;
                        currentId = currents.First(q => q.Code == Constants.Current.Yes).Id;
                        updateApplication = true;
                    }
                    else if (applicant.HighSchoolGraduationDate <= programStartDate)
                    {
                        // is grad date < first day of college? ==> BFA = 3, Current = Yes
                        basisForAdmissionId = basisForAdmissions.First(q => q.Code == Constants.BasisForAdmission.WillHaveOssd).Id;
                        currentId = currents.First(q => q.Code == Constants.Current.Yes).Id;
                        updateApplication = true;
                    }
                }
                else if (applicant.HighSchoolEnrolled == false && applicant.HighSchoolGraduated.HasValue)
                {
                    if (applicant.HighSchoolGraduated == true)
                    {
                        // has graduated ==> BFA = 3, Current = No
                        basisForAdmissionId = basisForAdmissions.First(q => q.Code == Constants.BasisForAdmission.WillHaveOssd).Id;
                        currentId = currents.First(q => q.Code == Constants.Current.No).Id;
                        updateApplication = true;
                    }
                    else if (applicant.HighSchoolGraduated == false)
                    {
                        // has not graduated ==> BFA = 2, Current = No
                        basisForAdmissionId = basisForAdmissions.First(q => q.Code == Constants.BasisForAdmission.WillNotHaveOssd).Id;
                        currentId = currents.First(q => q.Code == Constants.Current.No).Id;
                        updateApplication = true;
                    }
                }

                // check that BFA values have actually changed
                updateApplication = updateApplication
                                    && (application.BasisForAdmissionId != basisForAdmissionId
                                    || application.CurrentId != currentId);

                if (updateApplication)
                {
                    application.BasisForAdmissionId = basisForAdmissionId;
                    application.CurrentId = currentId;
                    application.ModifiedBy = modifiedBy;

                    hasChanges = true;
                }
            }

            return hasChanges;
        }

        public async Task<bool> PatchEducationStatus(Contact applicant, string modifiedBy, IList<BasisForAdmission> basisForAdmissions, IList<Current> currents, IList<ApplicationCycle> applicationCycles)
        {
            if (applicant == null) throw new ArgumentNullException(nameof(applicant));

            var hasChanges = false;

            if (applicant.HighSchoolEnrolled != null || applicant.HighSchoolGraduated != null || applicant.HighSchoolGraduationDate != null) return hasChanges;

            var applications = await _domesticContext.GetApplications(applicant.Id);
            if (applications.All(a => a.CurrentId.IsEmpty() && a.BasisForAdmissionId.IsEmpty())) return hasChanges;

            // get latest app cycle (ie 2021 over 2020)
            var applicationsCycles = applicationCycles.Where(c => applications.Any(a => a.ApplicationCycleId == c.Id));
            var latestAppCycle = applicationsCycles.Where(c =>
            {
                var app = applications.Single(a => a.ApplicationCycleId == c.Id);
                return !app.CurrentId.IsEmpty() || !app.BasisForAdmissionId.IsEmpty();
            }).OrderByDescending(x => x.StartDate).First();

            var application = applications.First(a => a.ApplicationCycleId == latestAppCycle.Id);
            var current = currents.FirstOrDefault(c => c.Id == application.CurrentId);
            var basisForAdmission = basisForAdmissions.FirstOrDefault(b => b.Id == application.BasisForAdmissionId);

            if (current == null || current.Code == Constants.Current.No)
            {
                if (basisForAdmission == null)
                {
                    applicant.HighSchoolEnrolled = false;
                    applicant.HighSchoolGraduated = null;
                    applicant.HighSchoolGraduationDate = null;
                    applicant.ModifiedBy = modifiedBy;
                    hasChanges = true;
                }
                else if (basisForAdmission.Code == Constants.BasisForAdmission.WillNotHaveOssd)
                {
                    applicant.HighSchoolEnrolled = false;
                    applicant.HighSchoolGraduated = false;
                    applicant.HighSchoolGraduationDate = null;
                    applicant.ModifiedBy = modifiedBy;
                    hasChanges = true;
                }
                else if (basisForAdmission.Code == Constants.BasisForAdmission.WillHaveOssd)
                {
                    applicant.HighSchoolEnrolled = false;
                    applicant.HighSchoolGraduated = true;
                    applicant.HighSchoolGraduationDate = null;
                    applicant.ModifiedBy = modifiedBy;
                    hasChanges = true;
                }
            }
            else if (current.Code == Constants.Current.Yes)
            {
                if (basisForAdmission == null || basisForAdmission.Code == Constants.BasisForAdmission.WillNotHaveOssd)
                {
                    applicant.HighSchoolEnrolled = true;
                    applicant.HighSchoolGraduated = null;
                    applicant.HighSchoolGraduationDate = null;
                    applicant.ModifiedBy = modifiedBy;
                    hasChanges = true;
                }
                else if (basisForAdmission.Code == Constants.BasisForAdmission.WillHaveOssd)
                {
                    var earliestAppCycle = applicationsCycles.OrderBy(c => c.StartDate).First();
                    applicant.HighSchoolEnrolled = true;
                    applicant.HighSchoolGraduated = null;
                    applicant.HighSchoolGraduationDate = earliestAppCycle.StartDate;
                    applicant.ModifiedBy = modifiedBy;
                    hasChanges = true;
                }
            }

            return hasChanges;
        }

        // from A2C: https://dev.azure.com/ocas/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FControllers%2FPaymentController.cs&version=GBmaster&line=1001&lineStyle=plain&lineEnd=1166&lineStartColumn=9&lineEndColumn=10
        public async Task<Order> WriteDetailsToOrder(Contact contact, Application application, Order order, string modifiedBy, IList<PaymentMethod> paymentMethods, bool isOnlinePayment = true, Guid? offlinePaymentMethodId = null, string offlineMetadata = null, decimal offlineAmount = 0)
        {
            var transferBalance = application.Balance != null && !isOnlinePayment ? application.Balance ?? 0M : 0M;
            var accountBalance = contact.Balance ?? 0M;
            var nsfBalance = contact.NsfBalance ?? 0M;
            var returnedPaymentBalance = contact.ReturnedPayment ?? 0M;
            var orderTotal = order.TotalAmount + nsfBalance + returnedPaymentBalance;
            var overpaymentBalance = contact.OverPayment ?? 0M;

            order.OrderPaymentStatus = OrderPaymentStatus.CheckedOut;
            var voucherItem = order.Details.FirstOrDefault(x => x.Type == ShoppingCartItemType.Voucher);
            var hasVoucher = voucherItem != null;
            var voucherAmount = Math.Abs(voucherItem?.Amount ?? 0M);

            if (hasVoucher)
            {
                order.OrderPaymentType = (isOnlinePayment && order.FinalTotal >= 0) ? OrderPaymentType.Online : OrderPaymentType.Offline;
            }
            else
            {
                order.OrderPaymentType = (isOnlinePayment && order.FinalTotal != 0) ? OrderPaymentType.Online : OrderPaymentType.Offline;
            }

            order.PaymentDetails = offlineMetadata;

            order.TransactionAmount = offlineAmount;

            // if this is an online payment, use "N/A" as the payment method
            order.PaymentMethodId = offlinePaymentMethodId ?? paymentMethods.First(pm => pm.Code == Constants.Payment.Na).Id;

            // Adding amount paid from voucher
            if (hasVoucher && voucherAmount > 0)
            {
                order.AmountPaidFromVoucher = voucherAmount;
                order.TransactionAmount = order.TotalAmount;
            }

            if (!isOnlinePayment && ((transferBalance + accountBalance + offlineAmount + overpaymentBalance < orderTotal) || (!order.Details.Any() && (orderTotal == 0))))
            {
                // not enough to pay for order, so log this as a deposit instead of a payment
                // if order.Details count equal to 0, also enable offline payment, and save it as deposit
                order.TransactionType = OrderTransactionType.Deposit;
                order.TransactionAmount = offlineAmount;
                order.PaymentAmount = offlineAmount;
                order.AmountPaidToAccountBalance = offlineAmount;
                order.OrderProcessingStatus = OrderProcessingStatus.Processed;
            }
            else
            { // enough for payment, transaction type is payment instead of deposit
                order.TransactionType = OrderTransactionType.Payment;

                if (transferBalance > 0 && orderTotal > 0)
                {
                    // calculate the amount of transfer balance used to pay for this order (application.balance)
                    if (orderTotal >= transferBalance)
                    {
                        order.AmountPaidFromTransferBalance = transferBalance;
                        orderTotal -= transferBalance;
                        transferBalance = 0M;
                    }
                    else
                    {
                        order.AmountPaidFromTransferBalance = orderTotal;
                        transferBalance -= orderTotal;
                        orderTotal = 0M;
                    }
                }

                if (overpaymentBalance > 0 && orderTotal > 0)
                {
                    // calculate the amount of overpayment balance used to pay for this order (contact.Overpayment)
                    if (orderTotal >= overpaymentBalance)
                    {
                        order.AmountPaidFromOverpaymentBalance = overpaymentBalance;
                        orderTotal -= overpaymentBalance;
                        overpaymentBalance = 0M;
                    }
                    else
                    {
                        order.AmountPaidFromOverpaymentBalance = orderTotal;
                        overpaymentBalance -= orderTotal;
                        orderTotal = 0M;
                    }
                }

                if (accountBalance > 0 && orderTotal > 0)
                {
                    // calculate the amount of account balance used to pay for this order (contact.balance)
                    if (orderTotal >= accountBalance)
                    {
                        order.AmountPaidFromAccountBalance = accountBalance;
                        orderTotal -= accountBalance;
                        accountBalance = 0M;
                    }
                    else
                    {
                        order.AmountPaidFromAccountBalance = orderTotal;
                        accountBalance -= orderTotal;
                        orderTotal = 0M;
                    }
                }

                if (!isOnlinePayment && offlineAmount >= 0M)
                {
                    // offline amount is the amount entered into Amount field in view
                    // Ensure.IsTrue(offlineAmount >= orderTotal, string.Empty);
                    if (offlineAmount < orderTotal)
                    {
                        throw new ValidationException("Offline Amount is less than Order Total");
                    }

                    order.PaymentAmount = offlineAmount;
                    order.TransactionAmount = offlineAmount + (order.AmountPaidFromAccountBalance ?? 0M) + (order.AmountPaidFromOverpaymentBalance ?? 0M);
                    if (nsfBalance != 0) order.AmountPaidAgainstNsfBalance = nsfBalance;
                    if (returnedPaymentBalance != 0) order.AmountPaidAgainstReturnedPaymentBalance = returnedPaymentBalance;
                    if (offlineAmount > orderTotal)
                    {
                        //if there is over payment balance already then PaidToOverpaymentBalance will be offlineAmount - orderTotal
                        if (overpaymentBalance == 0)
                            order.AmountPaidToOverpaymentBalance = offlineAmount + transferBalance + accountBalance - orderTotal;
                        else
                            order.AmountPaidToOverpaymentBalance = offlineAmount - orderTotal;
                    }

                    order.OrderPaymentStatus = OrderPaymentStatus.Paid;
                    order.OrderProcessingStatus = OrderProcessingStatus.Processed;

                    application.CompletedSteps = (int)ApplicationCompletedSteps.Payment;
                    application.ModifiedBy = modifiedBy;
                    await _domesticContext.UpdateApplication(application);
                }

                if (isOnlinePayment && offlineAmount >= 0)
                {
                    if (nsfBalance != 0) order.AmountPaidAgainstNsfBalance = nsfBalance;
                    if (returnedPaymentBalance != 0) order.AmountPaidAgainstReturnedPaymentBalance = returnedPaymentBalance;
                }

                if (isOnlinePayment && order.FinalTotal == 0 && hasVoucher)
                {
                    var paymentMethodId = paymentMethods.First(pm => pm.Code == Constants.Payment.Na).Id;
                    order.OrderPaymentStatus = OrderPaymentStatus.Paid;
                    order.AmountPaidFromVoucher = voucherAmount;
                    order.TransactionAmount = offlineAmount + (order.AmountPaidFromAccountBalance ?? 0M) + (order.AmountPaidFromOverpaymentBalance ?? 0M);
                    order.PaymentAmount = offlineAmount;
                    order.OrderProcessingStatus = OrderProcessingStatus.Processed;
                    order.TransactionType = OrderTransactionType.Payment;
                    order.PaymentMethodId = paymentMethodId;
                    order.PaymentResponseCode = Constants.Payment.PaymentResponseCode;
                    order.OrderConfirmationNumber = Constants.Payment.OrderConfirmationNumber;
                }

                if (isOnlinePayment && order.FinalTotal == 0 && !hasVoucher)
                {
                    order.TransactionAmount = offlineAmount + order.AmountPaidFromAccountBalance + order.AmountPaidFromOverpaymentBalance;
                    order.OrderPaymentStatus = OrderPaymentStatus.Paid;
                    order.OrderProcessingStatus = OrderProcessingStatus.Processed;
                    application.CompletedSteps = (int)ApplicationCompletedSteps.Payment;
                    application.ModifiedBy = modifiedBy;
                    await _domesticContext.UpdateApplication(application);
                    if (nsfBalance != 0) order.AmountPaidAgainstNsfBalance = nsfBalance;
                    if (returnedPaymentBalance != 0) order.AmountPaidAgainstReturnedPaymentBalance = returnedPaymentBalance;
                }
            }

            order.ModifiedBy = modifiedBy;
            order.AmountOverpaymentBalance = order.AmountOverpaymentBalance ?? 0M;
            order.AmountPaidAgainstNsfBalance = order.AmountPaidAgainstNsfBalance ?? 0M;
            order.AmountPaidAgainstReturnedPaymentBalance = order.AmountPaidAgainstReturnedPaymentBalance ?? 0M;
            order.AmountPaidFromAccountBalance = order.AmountPaidFromAccountBalance ?? 0M;
            order.AmountPaidFromOverpaymentBalance = order.AmountPaidFromOverpaymentBalance ?? 0M;
            order.AmountPaidFromTransferBalance = order.AmountPaidFromTransferBalance ?? 0M;
            order.AmountPaidFromVoucher = order.AmountPaidFromVoucher ?? 0M;
            order.AmountPaidToAccountBalance = order.AmountPaidToAccountBalance ?? 0M;
            order.AmountPaidToOverpaymentBalance = order.AmountPaidToOverpaymentBalance ?? 0M;
            order.PaymentAmount = order.PaymentAmount ?? 0M;
            order.TransactionAmount = order.TransactionAmount ?? 0M;
            return await _domesticContext.UpdateOrder(order);
        }

        // From CBA: https://dev.azure.com/ocas/OCAS%20Portfolio/_git/domesticapi?path=%2Fsrc%2FOCAS.Connector.CRM.WCF%2FServices%2FOrder%2FOrderWebService.svc.cs&version=GBmaster&line=283&lineStyle=plain&lineEnd=420&lineStartColumn=9&lineEndColumn=10
        public async Task<Order> CreateOrder(Guid applicationId, Guid applicantId, string modifiedBy, Guid sourceId, bool isOfflinePayment)
        {
            var applicant = await _domesticContext.GetContact(applicantId);
            var applications = new List<Application>();
            var programChoices = new List<ProgramChoice>();

            programChoices.AddRange(
                await _domesticContext.GetProgramChoices(new GetProgramChoicesOptions
                {
                    ApplicationId = applicationId,
                    StateCode = State.Active
                }));

            applications.Add(await _domesticContext.GetApplication(applicationId));

            var overpaymentBalance = applicant.OverPayment ?? 0M;
            var accountBalance = applicant.Balance ?? 0M;
            var nsfBalance = applicant.NsfBalance ?? 0M;
            var returnedPaymentBalance = applicant.ReturnedPayment ?? 0M;

            var accountBalanceConsumed = 0M;
            var balanceAlreadyUsed = false;
            var orders = new List<Order>();
            foreach (var application in applications)
            {
                var cart = await _domesticContext.GetShoppingCart(
                    new GetShoppingCartOptions
                    {
                        ApplicationId = application.Id
                    },
                    Locale.English);
                var cartItems = cart?.Details ?? new List<ShoppingCartDetail>();
                var transferBalance = application.Balance ?? 0M;

                // there are cart items or outstanding balances
                if (isOfflinePayment || cartItems.Any() || (nsfBalance + returnedPaymentBalance) > 0)
                {
                    // check to see if application needs to be included (i.e. exclude if no program choices included)
                    var applicationCount = 0;
                    var programChoiceCount = programChoices.Count();

                    foreach (var cartItem in cartItems)
                    {
                        if (cartItem.ProductId != null && cartItem.Type == ShoppingCartItemType.ApplicationFee)
                        {
                            applicationCount++;
                        }
                    }

                    if (!(applicationCount > 0 && programChoiceCount == 0) || (nsfBalance + returnedPaymentBalance) > 0)
                    {
                        var newOrder = await _domesticContext.CreateOrder(application.Id, applicantId, modifiedBy, sourceId, cart);

                        var orderItems = new List<OrderDetail>();
                        foreach (var cartItem in cartItems)
                        {
                            var item = await _domesticContext.CreateOrderDetail(newOrder, cartItem);
                            orderItems.Add(item);
                        }

                        newOrder.FederalTax = 0M;
                        newOrder.ProvincialTax = 0M;
                        newOrder.ModifiedBy = modifiedBy;
                        var voucherInOrder = orderItems.FirstOrDefault(x => x.Type == ShoppingCartItemType.Voucher);
                        if (voucherInOrder != null)
                        {
                            if (!voucherInOrder.VoucherId.HasValue)
                                throw new Exception($"VoucherId missing off OrderDetail {voucherInOrder.Id}");

                            if (!voucherInOrder.PricePerUnit.HasValue)
                                throw new Exception($"PricePerUnit missing off OrderDetail {voucherInOrder.Id}");

                            newOrder.AmountPaidFromVoucher = Convert.ToDecimal(voucherInOrder.PricePerUnit) * -1.0M;

                            //link voucher with its order
                            var voucher = await _domesticContext.GetVoucher(new GetVoucherOptions
                            {
                                Id = voucherInOrder.VoucherId.Value
                            });

                            voucher.OrderId = voucherInOrder.OrderId;
                            voucher.OrderDetailId = voucherInOrder.Id;

                            await _domesticContext.LinkOrderToVoucher(voucher);
                        }

                        newOrder = await _domesticContext.UpdateOrder(newOrder);

                        if (accountBalance != accountBalanceConsumed)
                        {
                            accountBalanceConsumed += accountBalance;
                        }
                        else
                        {
                            accountBalance -= accountBalanceConsumed;
                        }

                        if (balanceAlreadyUsed)
                        {
                            overpaymentBalance = 0;
                            accountBalance = 0;
                            transferBalance = 0;
                        }
                        else
                        {
                            balanceAlreadyUsed = true;
                        }

                        newOrder.FinalTotal = Math.Max(newOrder.TotalAmount - accountBalance - transferBalance - overpaymentBalance + nsfBalance + returnedPaymentBalance, 0);

                        newOrder = await _domesticContext.UpdateOrder(newOrder);
                        orders.Add(newOrder);
                    }
                }
            }

            return orders.FirstOrDefault();
        }
    }
}
