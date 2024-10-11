using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Ocas.Domestic.Apply.Api.Services.Validators;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Services.Validators;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class VerifyApplicantEmailAddressHandler : IRequestHandler<VerifyApplicantEmailAddress, OcasVerificationDetails>
    {
        private readonly IDomesticContext _domesticContext;

        public VerifyApplicantEmailAddressHandler(IDomesticContext domesticContext)
        {
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
        }

        public async Task<OcasVerificationDetails> Handle(VerifyApplicantEmailAddress request, CancellationToken cancellationToken)
        {
            var validator = new EmailValidator();
            var results = await validator.ValidateAsync(request.EmailAddress, cancellationToken);

            if (!results.IsValid)
            {
                return new OcasVerificationDetails
                {
                    IsValid = false,
                    Code = Core.ErrorCodes.General.ValidationError,
                    Message = results.ToString("#--#")
                };
            }

            var isDuplicate = await _domesticContext.IsDuplicateContact(request.ApplicantId, request.EmailAddress);

            return new OcasVerificationDetails
            {
                IsValid = !isDuplicate,
                Code = isDuplicate ? Core.ErrorCodes.General.ConflictVerificationError : string.Empty,
                Message = isDuplicate ? $"Applicant exists with {request.EmailAddress}" : string.Empty
            };
        }

        private class EmailValidator : AbstractValidator<string>
        {
            public EmailValidator()
            {
                CascadeMode = CascadeMode.StopOnFirstFailure;

                RuleFor(x => x)
                    .OcasEmailAddress()
                    .WithName("Email Address");
            }
        }
    }
}
