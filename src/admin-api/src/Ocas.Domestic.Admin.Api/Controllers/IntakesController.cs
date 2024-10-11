using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class IntakesController : Controller
    {
        private readonly IMediator _mediator;

        public IntakesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IList<IntakeBrief>>> Get(Guid applicationCycleMasterId, Guid? collegeId, [FromQuery] GetIntakesOptions options)
        {
            return Ok(await _mediator.Send(new GetIntakes
            {
                ApplicationCycleId = applicationCycleMasterId,
                CollegeId = collegeId,
                Options = options,
                User = User
            }));
        }

        [HttpGet("{intakeId}/applicants")]
        public async Task<ActionResult<PagedResult<IntakeApplicant>>> Get(Guid intakeId, [FromQuery] GetIntakeApplicantOptions @params)
        {
            return Ok(await _mediator.Send(new GetIntakeApplicants
            {
                IntakeId = intakeId,
                Params = @params,
                User = User
            }));
        }

        [HttpPut("[action]/{availabilityId}")]
        public async Task<IActionResult> UpdateAvailability(Guid availabilityId, [FromBody] IList<Guid> intakeIds)
        {
            await _mediator.Send(new UpdateIntakeAvailability
            {
                AvailabilityId = availabilityId,
                IntakeIds = intakeIds,
                User = User
            });

            return Ok();
        }

        [HttpGet("export")]
        public async Task<FileContentResult> Export(Guid applicationCycleMasterId, Guid collegeId, [FromQuery] GetIntakesOptions @params)
        {
            var reportDocument = await _mediator.Send(new GetIntakesReport
            {
                ApplicationCycleId = applicationCycleMasterId,
                CollegeId = collegeId,
                Params = @params,
                User = User
            });
            return File(reportDocument.Data, reportDocument.MimeType, reportDocument.Name);
        }
    }
}
