using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OsapTokensController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OsapTokensController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<OsapToken>> GetOsapToken(Guid applicantId)
        {
            return Ok(await _mediator.Send(new GetOsapToken
            {
                ApplicantId = applicantId,
                User = User
            }));
        }
    }
}
