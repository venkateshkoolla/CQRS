using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetMcuCode : IRequest<McuCode>
    {
        public string McuCode { get; set; }
    }
}
