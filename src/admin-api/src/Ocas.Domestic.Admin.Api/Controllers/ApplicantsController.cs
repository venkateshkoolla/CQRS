using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ApplicantsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ApplicantBrief>>> Get([FromQuery] GetApplicantBriefOptions @params)
        {
            return Ok(await _mediator.Send(
                new GetApplicantBriefs
                {
                    Params = @params,
                    User = User
                }));
        }

        [HttpGet("{applicantId}")]
        public async Task<ActionResult<ApplicantSummary>> Get(Guid applicantId)
        {
            return Ok(await _mediator.Send(
                new GetApplicantSummary
                {
                    ApplicantId = applicantId,
                    User = User
                }));
        }

        [HttpPut("{applicantId}")]
        public async Task<ActionResult<ApplicantUpdateInfo>> Put(Guid applicantId, ApplicantUpdateInfo applicant)
        {
            return Ok(await _mediator.Send(
                new UpdateApplicantInfo
                {
                    ApplicantId = applicantId,
                    ApplicantUpdateInfo = applicant,
                    User = User
                }));
        }

        [HttpPost("{applicantId}/[action]")]
        public async Task<ActionResult<UpsertAcademicRecordResult>> AcademicRecord(Guid applicantId, AcademicRecordBase academicRecord)
        {
            if (academicRecord == null) throw new ValidationException("'Academic Record' must not be empty.");

            //Set applicant id on academic record
            academicRecord.ApplicantId = applicantId;

            return Ok(await _mediator.Send(new UpsertAcademicRecord
            {
                AcademicRecord = academicRecord,
                ApplicantId = applicantId,
                User = User
            }));
        }

        [HttpGet("{applicantId}/[action]")]
        public async Task<ActionResult<PagedResult<ApplicantHistory>>> ApplicantHistories(Guid applicantId, [FromQuery] Guid? applicationId, [FromQuery] GetApplicantHistoryOptions options)
        {
            return Ok(await _mediator.Send(
                new GetApplicantHistories
                {
                    User = User,
                    ApplicantId = applicantId,
                    ApplicationId = applicationId,
                    Options = options
                }));
        }
    }
}