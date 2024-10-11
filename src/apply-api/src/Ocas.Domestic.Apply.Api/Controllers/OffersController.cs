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
    public class OffersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OffersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Offer>>> Get(Guid applicantId)
        {
            return Ok(await _mediator.Send(
                new GetOffers
                {
                    User = User,
                    ApplicantId = applicantId
                }));
        }

        [HttpPost("{offerId}/[action]")]
        public async Task<ActionResult<IList<Offer>>> Accept(Guid offerId)
        {
            return Ok(await _mediator.Send(
                new AcceptOffer
                {
                    User = User,
                    OfferId = offerId
                }));
        }

        [HttpPost("{offerId}/[action]")]
        public async Task<ActionResult<IList<Offer>>> Decline(Guid offerId)
        {
            return Ok(await _mediator.Send(
                new DeclineOffer
                {
                    User = User,
                    OfferId = offerId
                }));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<IList<Offer>>> DeclineAll(DeclineAllOffersInfo declineAllOffersInfo)
        {
            return Ok(await _mediator.Send(
                new DeclineAllOffers
                {
                    User = User,
                    ApplicationId = declineAllOffersInfo.ApplicationId,
                    IncludeAccepted = declineAllOffersInfo.IncludeAccepted
                }));
        }
    }
}
