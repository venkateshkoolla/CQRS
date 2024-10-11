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
    public class FinancialTransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FinancialTransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IList<FinancialTransaction>>> Get(Guid applicationId)
        {
            return Ok(await _mediator.Send(
                new GetFinancialTransactions
                {
                    User = User,
                    ApplicationId = applicationId
                }));
        }
    }
}
