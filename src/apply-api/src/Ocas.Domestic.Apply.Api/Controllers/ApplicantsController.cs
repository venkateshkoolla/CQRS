using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Controllers
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

        // Attribute based routing
        [HttpGet("current")]
        public async Task<ActionResult<Applicant>> Get()
        {
            return Ok(await _mediator.Send(
                new GetApplicant
                {
                    User = User
                }));
        }

        [HttpGet("{applicantId}")]
        public async Task<ActionResult<Applicant>> Get(Guid applicantId)
        {
            return Ok(await _mediator.Send(
                new GetApplicant
                {
                    ApplicantId = applicantId,
                    User = User
                }));
        }

        [HttpGet("{applicantId}/[action]/{email}")]
        public async Task<ActionResult<OcasVerificationDetails>> VerifyEmail(Guid applicantId, string email)
        {
            return Ok(await _mediator.Send(
                new VerifyApplicantEmailAddress
                {
                    User = User,
                    ApplicantId = applicantId,
                    EmailAddress = email
                }));
        }

        [HttpGet("{applicantId}/[action]/{oen}")]
        public async Task<ActionResult<OcasVerificationDetails>> VerifyOen(Guid applicantId, string oen)
        {
            return Ok(await _mediator.Send(
                new VerifyApplicantOen
                {
                    User = User,
                    ApplicantId = applicantId,
                    Oen = oen
                }));
        }

        [HttpPost("{applicantId}/[action]")]
        public async Task<ActionResult> VerifyProfile(Guid applicantId)
        {
            return Ok(await _mediator.Send(
                new VerifyProfile
                {
                    ApplicantId = applicantId,
                    User = User
                }));
        }

        [HttpPost("current")]
        public async Task<ActionResult<Applicant>> Post(ApplicantBase applicantBase)
        {
            return Ok(await _mediator.Send(new CreateApplicantBase
            {
                User = User,
                ApplicantBase = applicantBase
            }));
        }

        [HttpPut("{applicantId}")]
        public async Task<ActionResult<Applicant>> Put(Guid applicantId, Applicant applicant)
        {
            if (applicant.Id == Guid.Empty) applicant.Id = applicantId;

            return Ok(await _mediator.Send(
                new UpdateApplicant
                {
                    ApplicantId = applicantId,
                    Applicant = applicant,
                    User = User
                }));
        }

        [HttpPut("{applicantId}/privacy-statement")]
        public async Task<ActionResult> AcceptPrivacyStatement(Guid applicantId, AcceptPrivacyStatementInfo acceptPrivacyStatementInfo)
        {
            if (acceptPrivacyStatementInfo is null) throw new ValidationException("AcceptPrivacyStatementInfo must not be null");

            await _mediator.Send(
                new AcceptPrivacyStatement
                {
                    User = User,
                    ApplicantId = applicantId,
                    PrivacyStatementId = acceptPrivacyStatementInfo.PrivacyStatementId
                });

            return Ok();
        }

        [HttpPut("{applicantId}/commPrefs")]
        public async Task<ActionResult> UpdateCommPreferences(Guid applicantId, CommPreferencesInfo commPrefInfo)
        {
            if (commPrefInfo is null) throw new ValidationException("CommPreferencesInfo must not be null");

            await _mediator.Send(
                new UpdateCommPreferences
                {
                    User = User,
                    ApplicantId = applicantId,
                    AgreedToCasl = commPrefInfo.AgreedToCasl
                });

            return Ok();
        }

        [HttpPut("{applicantId}/eduStatus")]
        public async Task<ActionResult> UpdateEducationStatus(Guid applicantId, EducationStatusInfo eduStatusInfo)
        {
            if (eduStatusInfo is null) throw new ValidationException("EducationStatusInfo must not be null");

            await _mediator.Send(
                new UpdateEducationStatus
                {
                    ApplicantId = applicantId,
                    EnrolledInHighSchool = eduStatusInfo.EnrolledInHighSchool,
                    GraduatedHighSchool = eduStatusInfo.GraduatedHighSchool,
                    GraduationHighSchoolDate = eduStatusInfo.GraduationHighSchoolDate,
                    User = User
                });

            return Ok();
        }

        [HttpPut("{applicantId}/intl-cred")]
        public async Task<ActionResult> UpdateInternationalCreditAssessment(Guid applicantId, IntlCredentialAssessment intlCredentialAssessment)
        {
            await _mediator.Send(
                new UpdateInternationalCreditAssessment
                {
                    User = User,
                    ApplicantId = applicantId,
                    IntlCredentialAssessment = intlCredentialAssessment
                });

            return Ok();
        }
    }
}
