using System.Threading.Tasks;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Services.Clients
{
    public interface IMonerisClient
    {
        Task<ChargeCardResult> ChargeCard(
            string paymentToken,
            decimal amount,
            string customerId,
            string orderNumber,
            string email,
            string cvd,
            string expDate,
            string cardHolder);
    }

    public class ChargeCardResult
    {
        public Dto.ReceiptBase ReceiptBase { get; set; }
        public bool ChargeSuccess { get; set; }
    }
}
