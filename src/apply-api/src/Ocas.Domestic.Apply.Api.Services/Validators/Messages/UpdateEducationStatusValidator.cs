using System;
using FluentValidation;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class UpdateEducationStatusValidator : AbstractValidator<UpdateEducationStatus>
    {
        public UpdateEducationStatusValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IApplicantUserValidator());

            RuleFor(x => x.EnrolledInHighSchool)
                .NotNull();

            When(x => x.EnrolledInHighSchool.HasValue, () =>
            {
                RuleFor(x => x.GraduatedHighSchool)
                    .NotNull()
                    .When(x => x.EnrolledInHighSchool == false);

                RuleFor(x => x.GraduationHighSchoolDate)
                    .Must(x => x.IsDate(Constants.DateFormat.YearMonthDashed))
                    .WithMessage("'{PropertyName}' must be in correct format of yyyy-MM.")
                    .Must(x =>
                    {
                        if (!x.IsDate(Constants.DateFormat.YearMonthDashed)) return false;

                        var gradDate = x.ToDateTime(Constants.DateFormat.YearMonthDashed);
                        var today = DateTime.UtcNow.ToDateInEstAsUtc();

                        return gradDate.Year > today.Year || (gradDate.Year == today.Year && gradDate.Month >= today.Month);
                    })
                    .WithMessage("'{PropertyName}' must be in the future.")
                    .When(x => x.EnrolledInHighSchool == true);
            });
        }
    }
}
