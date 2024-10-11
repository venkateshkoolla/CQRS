using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetMcuCodes : IRequest<PagedResult<McuCode>>
    {
        public GetMcuCodeOptions Params { get; set; }
    }
}
