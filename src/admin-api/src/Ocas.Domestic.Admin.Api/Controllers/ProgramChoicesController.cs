using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Services.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProgramChoicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProgramChoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IList<ProgramChoice>>> Get(Guid? applicationId, Guid? applicantId, bool isRemoved = false)
        {
            return Ok(await _mediator.Send(
                new GetProgramChoices
                {
                    ApplicationId = applicationId,
                    ApplicantId = applicantId,
                    IsRemoved = isRemoved,
                    User = User
                }));
        }

        [HttpPut("{programChoiceId}")]
        public async Task<ActionResult<ProgramChoice>> Update(Guid programChoiceId, ProgramChoiceUpdateInfo programChoiceUpdateInfo)
        {
            return Ok(await _mediator.Send(
                new UpdateProgramChoice
                {
                    ProgramChoiceId = programChoiceId,
                    EffectiveDate = programChoiceUpdateInfo.EffectiveDate,
                    User = User
                }));
        }
    }
}
