using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Services.Messages;

namespace Ocas.Domestic.Apply.Api.Controllers
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

        [HttpGet]
        public async Task<ActionResult<IList<Application>>> Get(Guid applicantId)
        {
            return Ok(await _mediator.Send(new GetApplications
            {
                ApplicantId = applicantId,
                User = User
            }));
        }

        [HttpPost]
        public async Task<ActionResult<Application>> Post(ApplicationBase application)
        {
            if (application is null) throw new ValidationException("Application must not be null");

            return Ok(await _mediator.Send(
                new CreateApplication
                {
                    User = User,
                    ApplicantId = application.ApplicantId,
                    ApplicationCycleId = application.ApplicationCycleId
                }));
        }

        [HttpPut("{applicationId}/[action]")]
        public async Task<ActionResult<IList<ProgramChoice>>> ProgramChoices(Guid applicationId, [FromBody]List<ProgramChoiceBase> programChoices)
        {
            //Set application id on choices
            programChoices.ForEach(c => c.ApplicationId = applicationId);

            return Ok(await _mediator.Send(
                new UpdateProgramChoices
                {
                    ApplicationId = applicationId,
                    ProgramChoices = programChoices,
                    User = User
                }));
        }

        [HttpPost("{applicationId}/[action]")]
        public async Task<ActionResult> CompletePrograms(Guid applicationId)
        {
            return Ok(await _mediator.Send(
                new CompletePrograms
                {
                    ApplicationId = applicationId,
                    User = User
                }));
        }

        [HttpPost("{applicationId}/[action]")]
        public async Task<ActionResult> CompleteTranscripts(Guid applicationId)
        {
            return Ok(await _mediator.Send(
                new CompleteTranscripts
                {
                    ApplicationId = applicationId,
                    User = User
                }));
        }

        [HttpGet("{applicationId}/[action]")]
        public async Task<ActionResult<IList<ShoppingCartDetail>>> ShoppingCart(Guid applicationId)
        {
            return Ok(await _mediator.Send(new GetShoppingCart
            {
                ApplicationId = applicationId,
                User = User
            }));
        }

        [HttpPost("{applicationId}/[action]")]
        public async Task<ActionResult> Voucher(Guid applicationId, string code)
        {
            return Ok(await _mediator.Send(new ClaimVoucher
            {
                ApplicationId = applicationId,
                Code = code,
                User = User
            }));
        }

        [HttpDelete("{applicationId}/voucher")]
        public async Task<ActionResult> RemoveVoucher(Guid applicationId, string code)
        {
            return Ok(await _mediator.Send(new RemoveVoucher
            {
                ApplicationId = applicationId,
                Code = code,
                User = User
            }));
        }
    }
}
