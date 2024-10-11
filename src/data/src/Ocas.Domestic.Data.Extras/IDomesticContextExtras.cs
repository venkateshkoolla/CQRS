using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Extras
{
    public interface IDomesticContextExtras
    {
        Task<bool> PatchBasisForAdmission(
            Application application,
            Contact applicant,
            string modifiedBy,
            IList<BasisForAdmission> basisForAdmissions,
            IList<Current> currents,
            IList<ApplicationCycle> applicationCycles);

        Task<bool> PatchEducationStatus(
            Contact applicant,
            string modifiedBy,
            IList<BasisForAdmission> basisForAdmissions,
            IList<Current> currents,
            IList<ApplicationCycle> applicationCycles);

        Task<Order> CreateOrder(Guid applicationId, Guid applicantId, string modifiedBy, Guid sourceId, bool isOfflinePayment);

        Task<Order> WriteDetailsToOrder(
            Contact contact,
            Application application,
            Order order,
            string modifiedBy,
            IList<PaymentMethod> paymentMethods,
            bool isOnlinePayment = true,
            Guid? offlinePaymentMethodId = null,
            string offlineMetadata = null,
            decimal offlineAmount = 0);
    }
}
