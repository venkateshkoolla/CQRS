using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProgramsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProgramsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{programId}")]
        public async Task<ActionResult<Models.Program>> Get(Guid programId)
        {
            return Ok(await _mediator.Send(new GetProgram
            {
                ProgramId = programId,
                User = User
            }));
        }

        [HttpGet]
        public async Task<ActionResult<IList<ProgramBrief>>> Get(Guid applicationCycleMasterId, Guid collegeId, [FromQuery] GetProgramBriefOptions @params)
        {
            return Ok(await _mediator.Send(new GetProgramBriefs
            {
                ApplicationCycleId = applicationCycleMasterId,
                CollegeId = collegeId,
                User = User,
                Params = @params
            }));
        }

        [HttpGet("[action]/{code}")]
        public async Task<ActionResult<bool>> CodeAvailable(string code, Guid applicationCycleId, Guid campusId, Guid deliveryId)
        {
            return Ok(await _mediator.Send(new ProgramCodeAvailable
            {
                Code = code,
                CollegeApplicationCycleId = applicationCycleId,
                CampusId = campusId,
                DeliveryId = deliveryId,
                User = User
            }));
        }

        [HttpGet("export")]
        public async Task<FileContentResult> Export(Guid applicationCycleMasterId, Guid collegeId, [FromQuery] GetProgramOptions @params)
        {
            var reportDocument = await _mediator.Send(new GetProgramsReport
            {
                ApplicationCycleId = applicationCycleMasterId,
                CollegeId = collegeId,
                Params = @params,
                User = User
            });

            return File(reportDocument.Data, reportDocument.MimeType, reportDocument.Name);
        }

        [HttpPost]
        public async Task<ActionResult<Models.Program>> Post([FromBody] ProgramBase model)
        {
            return Ok(await _mediator.Send(new CreateProgram
            {
                Program = model,
                User = User
            }));
        }

        [HttpPut("{programId}")]
        public async Task<ActionResult<Models.Program>> Put(Guid programId, [FromBody] Models.Program model)
        {
            return Ok(await _mediator.Send(new UpdateProgram
            {
                ProgramId = programId,
                Program = model,
                User = User
            }));
        }

        [HttpDelete("{programId}")]
        public async Task<IActionResult> Delete(Guid programId)
        {
            await _mediator.Send(new DeleteProgram
            {
                ProgramId = programId,
                User = User
            });

            return Ok();
        }
    }
}
