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
    public class UniversitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UniversitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v1/universities
        [HttpGet]
        public async Task<ActionResult<IList<University>>> GetUniversities()
        {
            return Ok(await _mediator.Send(new GetUniversities
            {
                User = User
            }));
        }
    }
}
