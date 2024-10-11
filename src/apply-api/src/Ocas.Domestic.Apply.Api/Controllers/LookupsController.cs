﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LookupsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LookupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v1/lookups?filter=one,two,three...
        [HttpGet]
        public async Task<ActionResult<AllLookups>> GetLookups(string filter = null)
        {
            var result = await _mediator.Send(new GetLookups
            {
                Filter = filter,
                User = User
            });

            return Ok(result);
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult> Purge(string filter = null)
        {
            await _mediator.Send(new PurgeLookups
            {
                Filter = filter,
                User = User
            });

            return Ok();
        }
    }
}
