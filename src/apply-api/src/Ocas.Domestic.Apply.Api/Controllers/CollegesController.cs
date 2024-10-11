using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Controllers
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

        // GET: api/v1/colleges
        [HttpGet]
        public async Task<ActionResult<IList<College>>> GetColleges()
        {
            var result = await _mediator.Send(new GetColleges
            {
                User = User
            });

            return Ok(result);
        }

        [HttpGet("{collegeId}/template")]
        public async Task<ActionResult<CollegeTemplate>> GetTemplate(Guid collegeId, CollegeTemplateKey key)
        {
            return Ok(await _mediator.Send(new GetCollegeTemplate
            {
                CollegeId = collegeId,
                Key = key,
                User = User
            }));
        }
    }
}
