using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PartnerBrandingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PartnerBrandingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{code}")]
        [AllowAnonymous]
        public async Task<ActionResult<PartnerBranding>> GetPartnerBranding(string code)
        {
            return Ok(await _mediator.Send(new GetPartnerBranding
            {
                Code = code
            }));
        }
    }
}
