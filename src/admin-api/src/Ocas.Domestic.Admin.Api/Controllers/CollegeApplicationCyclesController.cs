using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CollegeApplicationCyclesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CollegeApplicationCyclesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IList<CollegeApplicationCycle>>> Get(Guid collegeId)
        {
            return Ok(await _mediator.Send(new GetCollegeApplicationCycles
            {
                CollegeId = collegeId,
                User = User
            }));
        }

        [HttpGet("{collegeApplicationCycleId}/[action]/{code}")]
        public async Task<IActionResult> SpecialCodes(Guid collegeApplicationCycleId, string code)
        {
            return Ok(await _mediator.Send(new GetSpecialCode
            {
                CollegeApplicationCycleId = collegeApplicationCycleId,
                SpecialCode = code,
                User = User
            }));
        }

        [HttpGet("{collegeApplicationCycleId}/[action]")]
        public async Task<IActionResult> SpecialCodes(Guid collegeApplicationCycleId, [FromQuery] GetSpecialCodeOptions @params)
        {
            return Ok(await _mediator.Send(new GetSpecialCodes
            {
                CollegeApplicationCycleId = collegeApplicationCycleId,
                Params = @params,
                User = User
            }));
        }
    }
}
