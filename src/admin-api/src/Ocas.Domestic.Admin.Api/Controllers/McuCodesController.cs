using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class McuCodesController : Controller
    {
        private readonly IMediator _mediator;

        public McuCodesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<McuCode>> Get(string code)
        {
            return Ok(await _mediator.Send(new GetMcuCode
            {
                McuCode = code
            }));
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<McuCode>>> Get([FromQuery] GetMcuCodeOptions @params)
        {
            return Ok(await _mediator.Send(new GetMcuCodes
            {
                Params = @params
            }));
        }
    }
}
