using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Services.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "BO,PortalAdmin")]
        [HttpPut("{applicationId}/[action]")]
        public async Task<ActionResult<Application>> Number(Guid applicationId, ApplicationNumberUpdateInfo applicationInfo)
        {
            return Ok(await _mediator.Send(
                new UpdateApplicationNumber
                {
                    ApplicationId = applicationId,
                    Number = applicationInfo.Number,
                    User = User
                }));
        }

        [Authorize(Roles = "BO,PortalAdmin")]
        [HttpPut("{applicationId}/[action]")]
        public async Task<ActionResult<Application>> EffectiveDate(Guid applicationId, ApplicationEffectiveDateUpdateInfo applicationInfo)
        {
            return Ok(await _mediator.Send(
                new UpdateApplicationEffectiveDate
                {
                    ApplicationId = applicationId,
                    EffectiveDate = applicationInfo.EffectiveDate,
                    User = User
                }));
        }

        [HttpGet("{applicationId}/[action]")]
        public async Task<ActionResult<IList<OfferHistory>>> OfferHistories(Guid applicationId)
        {
            return Ok(await _mediator.Send(
                new GetOfferHistories
                {
                    User = User,
                    ApplicationId = applicationId
                }));
        }

        [Authorize(Roles = "BO,PortalAdmin")]
        [HttpPost("{applicationId}/[action]")]
        public async Task<ActionResult<IList<ApplicationSummary>>> Pay(Guid applicationId, [FromBody]OfflinePaymentInfo offlinePaymentInfo)
        {
            var order = await _mediator.Send(new CreateOrder
            {
                ApplicationId = applicationId,
                IsOfflinePayment = true,
                User = User
            });

            await _mediator.Send(new PayOrder
            {
                OrderId = order.Id,
                User = User,
                OfflinePaymentInfo = offlinePaymentInfo,
                IsOfflinePayment = true
            });

            return Ok(await _mediator.Send(new GetApplicationSummaries
            {
                ApplicationId = applicationId,
                User = User
            }));
        }

        [Authorize(Roles = "BO,PortalAdmin")]
        [HttpPost("{applicationId}/[action]")]
        public async Task<ActionResult<ApplicationSummary>> ProgramChoice(Guid applicationId, [FromBody]CreateProgramChoiceRequest programChoice)
        {
            return Ok(await _mediator.Send(new CreateProgramChoice
            {
                ApplicationId = applicationId,
                ProgramChoice = programChoice,
                User = User
            }));
        }

        [HttpGet("{applicationId}/[action]")]
        public async Task<ActionResult<IList<CollegeTransmissionHistory>>> TransmissionHistories(Guid applicationId, [FromQuery] GetCollegeTransmissionHistoryOptions options)
        {
            return Ok(await _mediator.Send(
                new GetCollegeTransmissionHistories
                {
                    User = User,
                    ApplicationId = applicationId,
                    Options = options
                }));
        }
    }
}
