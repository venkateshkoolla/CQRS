using System;
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
    public class CampusesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CampusesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Campus>>> Get(Guid collegeId)
        {
            return Ok(await _mediator.Send(
                new GetCampuses
                {
                    CollegeId = collegeId,
                    User = User
                }));
        }
    }
}