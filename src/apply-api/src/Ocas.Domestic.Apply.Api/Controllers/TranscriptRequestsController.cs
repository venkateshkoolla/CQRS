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
    public class TranscriptRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TranscriptRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IList<TranscriptRequest>>> GetTranscriptRequests([FromQuery] Guid? applicationId, [FromQuery] Guid? applicantId)
        {
            return Ok(await _mediator.Send(
                new GetTranscriptRequests
                {
                    ApplicantId = applicantId,
                    ApplicationId = applicationId,
                    User = User
                }));
        }

        [HttpPost]
        public async Task<ActionResult<IList<TranscriptRequest>>> CreateTranscriptRequests([FromBody]IList<TranscriptRequestBase> transcriptRequests)
        {
            return Ok(await _mediator.Send(
                new CreateTranscriptRequests
                {
                    TranscriptRequests = transcriptRequests,
                    User = User
                }));
        }

        [HttpDelete("{transcriptRequestId}")]
        public async Task<ActionResult> DeleteTranscriptRequest(Guid transcriptRequestId)
        {
            return Ok(await _mediator.Send(
                new DeleteTranscriptRequest
                {
                    TranscriptRequestId = transcriptRequestId,
                    User = User
                }));
        }

        [HttpPost("{transcriptRequestId}/reissue")]
        public async Task<ActionResult<IList<TranscriptRequest>>> ReissueTranscriptRequest(Guid transcriptRequestId)
        {
            return Ok(await _mediator.Send(
                new ReissueTranscriptRequest
                {
                    TranscriptRequestId = transcriptRequestId,
                    User = User
                }));
        }
    }
}
