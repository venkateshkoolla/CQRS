using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EducationRecordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EducationRecordsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Education>>> Get([FromQuery] Guid applicantId)
        {
            if (applicantId.IsEmpty()) throw new ValidationException("Applicant Id must not be empty");

            return Ok(await _mediator.Send(
                new GetEducations
                {
                    User = User,
                    ApplicantId = applicantId
                }));
        }

        [HttpPost]
        public async Task<ActionResult<Education>> CreateEducation(EducationBase education)
        {
            if (education is null) throw new ValidationException("Education must not be null");

            return Ok(await _mediator.Send(
                new CreateEducation
                {
                    User = User,
                    ApplicantId = education.ApplicantId,
                    Education = education
                }));
        }

        [HttpPut("{educationId}")]
        public async Task<ActionResult<Education>> UpdateEducation(Guid educationId, Education education)
        {
            if (education is null) throw new ValidationException("Education must not be null");

            return Ok(await _mediator.Send(
                new UpdateEducation
                {
                    User = User,
                    ApplicantId = education.ApplicantId,
                    EducationId = educationId,
                    Education = education
                }));
        }

        [HttpDelete("{educationId}")]
        public async Task<ActionResult> RemoveEducation(Guid educationId)
        {
            return Ok(await _mediator.Send(new RemoveEducation
            {
                User = User,
                EducationId = educationId
            }));
        }
    }
}
