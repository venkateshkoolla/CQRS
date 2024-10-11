using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Ocas.Domestic.Apply.Api.Services.Validators;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class VerifyApplicantOenHandler : IRequestHandler<VerifyApplicantOen, OcasVerificationDetails>
    {
        private readonly IDomesticContext _domesticContext;

        public VerifyApplicantOenHandler(IDomesticContext domesticContext)
        {
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
        }

        public async Task<OcasVerificationDetails> Handle(VerifyApplicantOen request, CancellationToken cancellationToken)
        {
            var validator = new OenValidator();
            var results = await validator.ValidateAsync(request.Oen, cancellationToken);

            if (!results.IsValid)
            {
                return new OcasVerificationDetails
                {
                    IsValid = false,
                    Code = ErrorCodes.General.ValidationError,
                    Message = results.ToString("#--#")
                };
            }

            var isDuplicate = await _domesticContext.IsDuplicateOen(request.ApplicantId, request.Oen);
            return new OcasVerificationDetails
            {
                IsValid = !isDuplicate,
                Code = isDuplicate ? ErrorCodes.General.ConflictVerificationError : string.Empty,
                Message = isDuplicate ? $"Applicant exists with {request.Oen}" : string.Empty
            };
        }

        private class OenValidator : AbstractValidator<string>
        {
            public OenValidator()
            {
                CascadeMode = CascadeMode.StopOnFirstFailure;

                RuleFor(x => x)
                    .OenValid()
                    .WithName("Oen")
                    .When(x => x != Constants.Education.DefaultOntarioEducationNumber);
            }
        }
    }
}
