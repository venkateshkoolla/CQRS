using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OntarioStudentCourseCreditsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OntarioStudentCourseCreditsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<OntarioStudentCourseCredit>> Post(OntarioStudentCourseCreditBase model)
        {
            if (model is null) throw new ValidationException("OntarioStudentCourseCredit model must not be null");

            return Ok(await _mediator.Send(
                new CreateOntarioStudentCourseCredit
                {
                    User = User,
                    ApplicantId = model.ApplicantId,
                    OntarioStudentCourseCredit = model
                }));
        }

        [HttpPut("{ontarioStudentCourseCreditId}")]
        public async Task<ActionResult<OntarioStudentCourseCredit>> Put(Guid ontarioStudentCourseCreditId, OntarioStudentCourseCredit model)
        {
            if (model is null) throw new ValidationException("Ontario Student Course Credit model must not be null");

            return Ok(await _mediator.Send(
                new UpdateOntarioStudentCourseCredit
                {
                    User = User,
                    ApplicantId = model.ApplicantId,
                    OntarioStudentCourseCreditId = ontarioStudentCourseCreditId,
                    OntarioStudentCourseCredit = model
                }));
        }

        [HttpDelete("{ontarioStudentCourseCreditId}")]
        public async Task<ActionResult> DeleteOntarioStudentCourseCredit(Guid ontarioStudentCourseCreditId)
        {
            return Ok(await _mediator.Send(new DeleteOntarioStudentCourseCredit
            {
                User = User,
                OntarioStudentCourseCreditId = ontarioStudentCourseCreditId
            }));
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<OntarioHighSchoolCourseCode>> Get(string code)
        {
            return Ok(await _mediator.Send(new GetOntarioHighSchoolCourseCode
            {
                Code = code
            }));
        }
    }
}