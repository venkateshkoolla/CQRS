using System;
using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Api.Services.Extensions;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Enums;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Models
{
    public class EducationBaseValidator : AbstractValidator<EducationBase>
    {
        public EducationBaseValidator(ILookupsCache lookupsCache, IDomesticContext domesticContext, OperationType operationType = OperationType.Create)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ApplicantId)
                .NotEmpty();

            RuleFor(x => x.AcademicUpgrade)
                .NotNull();

            RuleFor(x => x.CurrentlyAttending)
                .NotNull();

            RuleFor(x => x.AttendedFrom)
                .NotEmpty()
                .Length(7)
                .Must(y => y.IsDate(Constants.DateFormat.YearMonthDashed))
                .WithMessage(x => $"'{{PropertyName}}' is not a date: {x.AttendedFrom}")
                .Must(y => y.ToDateTime(Constants.DateFormat.YearMonthDashed) <= DateTime.UtcNow.ToString(Constants.DateFormat.YearMonthDashed).ToDateTime(Constants.DateFormat.YearMonthDashed))
                .WithMessage(x => $"'{{PropertyName}}' must be in the past: {x.AttendedFrom}");

            RuleFor(x => x.AttendedTo)
                .NotEmpty()
                .Length(7)
                .Must(y => y.IsDate(Constants.DateFormat.YearMonthDashed))
                .WithMessage(x => $"'{{PropertyName}}' is not a date: {x.AttendedTo}")
                .Must(y => y.ToDateTime(Constants.DateFormat.YearMonthDashed) <= DateTime.UtcNow.ToString(Constants.DateFormat.YearMonthDashed).ToDateTime(Constants.DateFormat.YearMonthDashed))
                .WithMessage(x => $"'{{PropertyName}}' must be in the past: {x.AttendedTo}")
                .When(x => x.CurrentlyAttending == false);

            RuleFor(x => x)
                .Must(x => x.AttendedTo.ToDateTime(Constants.DateFormat.YearMonthDashed) >= x.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed))
                .WithMessage(x => $"'Attended To' must be greater than 'Attended From': {x.AttendedTo} >= {x.AttendedFrom}")
                .When(x => x.CurrentlyAttending == false
                           && x.AttendedTo.IsDate(Constants.DateFormat.YearMonthDashed)
                           && x.AttendedFrom.IsDate(Constants.DateFormat.YearMonthDashed));

            RuleFor(x => x.CountryId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);

                    return countries.Any(z => z.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.CountryId}");

            RuleFor(x => x.FirstNameOnRecord)
                .MaximumLength(30)
                .Matches(Patterns.Name)
                .When(x => !string.IsNullOrEmpty(x.FirstNameOnRecord));

            RuleFor(x => x.InstituteTypeId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var instituteTypes = await lookupsCache.GetInstituteTypes(Constants.Localization.EnglishCanada);

                    return instituteTypes.Any(z => z.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.InstituteTypeId}");

            RuleFor(x => x.LastNameOnRecord)
                .MaximumLength(30)
                .Matches(Patterns.Name)
                .When(x => !string.IsNullOrEmpty(x.LastNameOnRecord));

            RuleFor(x => x.Major)
                .MaximumLength(60)
                .Matches(Patterns.Iso8859)
                .When(x => !string.IsNullOrEmpty(x.Major));

            RuleFor(x => x.OtherCredential)
                .MaximumLength(60)
                .Matches(Patterns.Iso8859)
                .When(x => !string.IsNullOrEmpty(x.OtherCredential));

            RuleFor(x => x)
                .MustAsync(async (y, _) => y.GetEducationType(await lookupsCache.GetCountries(Constants.Localization.FallbackLocalization), await lookupsCache.GetInstituteTypes(Constants.Localization.FallbackLocalization)) != null)
                .WithMessage(_ => "Could not determine education type");

            RuleFor(x => x)
                .SetValidator(new AcademicUpgradingValidator(lookupsCache, domesticContext, operationType))
                .WhenAsync(async (x, _) => x.GetEducationType(await lookupsCache.GetCountries(Constants.Localization.FallbackLocalization), await lookupsCache.GetInstituteTypes(Constants.Localization.FallbackLocalization)) == EducationType.AcademicUpgrading);

            RuleFor(x => x)
                .SetValidator(new CanadianUniversityValidator(lookupsCache, domesticContext, operationType))
                .WhenAsync(async (x, _) => x.GetEducationType(await lookupsCache.GetCountries(Constants.Localization.FallbackLocalization), await lookupsCache.GetInstituteTypes(Constants.Localization.FallbackLocalization)) == EducationType.CanadianUniversity);

            RuleFor(x => x)
                .SetValidator(new CanadianCollegeValidator(lookupsCache, domesticContext, operationType))
                .WhenAsync(async (x, _) => x.GetEducationType(await lookupsCache.GetCountries(Constants.Localization.FallbackLocalization), await lookupsCache.GetInstituteTypes(Constants.Localization.FallbackLocalization)) == EducationType.CanadianCollege);

            RuleFor(x => x)
                .SetValidator(new InternationalEducationValidator(lookupsCache))
                .WhenAsync(async (x, _) => x.GetEducationType(await lookupsCache.GetCountries(Constants.Localization.FallbackLocalization), await lookupsCache.GetInstituteTypes(Constants.Localization.FallbackLocalization)) == EducationType.International);

            RuleFor(x => x)
                .SetValidator(new CanadianHighSchoolValidator(lookupsCache, domesticContext, operationType))
                .WhenAsync(async (x, _) => x.GetEducationType(await lookupsCache.GetCountries(Constants.Localization.FallbackLocalization), await lookupsCache.GetInstituteTypes(Constants.Localization.FallbackLocalization)) == EducationType.CanadianHighSchool);
        }
    }
}
