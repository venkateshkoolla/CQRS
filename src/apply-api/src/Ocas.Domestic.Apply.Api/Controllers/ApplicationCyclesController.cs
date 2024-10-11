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
    public class ApplicationCyclesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationCyclesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IList<ApplicationCycle>>> Get()
        {
            var result = await _mediator.Send(new GetApplicationCycles
            {
                User = User
            });

            return Ok(result);
        }
    }
}