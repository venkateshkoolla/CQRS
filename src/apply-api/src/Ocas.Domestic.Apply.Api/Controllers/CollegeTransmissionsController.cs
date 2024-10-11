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
    public class CollegeTransmissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CollegeTransmissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IList<CollegeTransmission>>> GetCollegeTransmissions(Guid applicationId)
        {
            return Ok(await _mediator.Send(new GetCollegeTransmissions
            {
                ApplicationId = applicationId,
                User = User
            }));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<DateTime?>> NextSendDate()
        {
            return Ok(await _mediator.Send(new GetNextSendDate()));
        }
    }
}
