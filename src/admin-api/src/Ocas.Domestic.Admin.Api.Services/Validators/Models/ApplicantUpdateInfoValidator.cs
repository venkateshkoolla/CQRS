using System;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models
{
    public class ApplicantUpdateInfoValidator : AbstractValidator<ApplicantUpdateInfo>
    {
        public ApplicantUpdateInfoValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(30)
                .Matches(Patterns.Name);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(30)
                .Matches(Patterns.Name);

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .Must(y => y.IsDate())
                .WithMessage(y => $"'{{PropertyName}}' is not valid: {y.BirthDate}")
                .Must(y => y.ToDateTime() < DateTime.UtcNow)
                .WithMessage(y => $"'{{PropertyName}}' can not be in future: {y.BirthDate}")
                .Must((y, _) => y.BirthDate.ToDateTime().Year > DateTime.UtcNow.Year - 90 && y.BirthDate.ToDateTime().Year < DateTime.UtcNow.Year - 16)
                .WithMessage(y => $"Age must be within 16 and 90: {y.BirthDate}");
        }
    }
}