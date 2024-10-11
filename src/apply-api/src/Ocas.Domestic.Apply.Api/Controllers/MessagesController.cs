using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IList<ApplicantMessage>>> Get(Guid applicantId, DateTimeOffset? after)
        {
            return Ok(await _mediator.Send(new GetApplicantMessages
            {
                After = after?.UtcDateTime,
                ApplicantId = applicantId,
                User = User
            }));
        }

        [HttpPost("{id}/[action]")]
        public async Task<ActionResult> Read(Guid id)
        {
            return Ok(await _mediator.Send(new UpdateApplicantMessage
            {
                Id = id,
                User = User
            }));
        }
    }
}
