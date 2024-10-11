using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CollegesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CollegesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IList<College>>> Get()
        {
            return Ok(await _mediator.Send(new GetColleges { User = User }));
        }
    }
}