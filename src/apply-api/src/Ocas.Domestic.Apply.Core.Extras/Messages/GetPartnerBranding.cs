using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class GetPartnerBranding : IRequest<PartnerBranding>
    {
        public string Code { get; set; }
    }
}
