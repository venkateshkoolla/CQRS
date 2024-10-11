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
    public class CitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v1/cities
        [HttpGet]
        public async Task<ActionResult<IList<City>>> GetCities(Guid? provinceId)
        {
            var result = await _mediator.Send(new GetCities
            {
                User = User,
                ProvinceId = provinceId
            });

            return Ok(result);
        }
    }
}
