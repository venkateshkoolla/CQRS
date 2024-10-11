using System;
using FluentValidation;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Models
{
    public class ApplicantBaseValidator : AbstractValidator<ApplicantBase>
    {
        public ApplicantBaseValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(30)
                .Matches(Patterns.Name);

            RuleFor(x => x.MiddleName)
                .MaximumLength(30)
                .Matches(Patterns.Name)
                .Unless(x => string.IsNullOrEmpty(x.MiddleName));

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(30)
                .Matches(Patterns.Name);

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .Must(v => v.IsDate())
                .WithMessage("'{PropertyName}' must be a date.")
                .Must((y, _) => y.BirthDate.ToDateTime().Year >= DateTime.UtcNow.Year - 90 && y.BirthDate.ToDateTime().Year <= DateTime.UtcNow.Year - 16)
                .WithMessage(y => $"Age must be within 16 and 90 years old: {y.BirthDate}");
        }
    }
}