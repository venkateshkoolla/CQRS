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
    public class PrivacyStatementsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PrivacyStatementsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("latest")]
        public async Task<ActionResult<PrivacyStatement>> GetLatestPrivacyStatement()
        {
            return Ok(await _mediator.Send(
                new GetLatestPrivacyStatement
                {
                    User = User
                }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PrivacyStatement>> GetPrivacyStatement(Guid id)
        {
            return Ok(await _mediator.Send(
                new GetPrivacyStatement
                {
                    Id = id,
                    User = User
                }));
        }
    }
}
