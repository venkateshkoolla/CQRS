using System;
using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Services.Validators;
using CommonPattern = Ocas.Domestic.Apply.Services.Validators;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models
{
    public class GetApplicantBriefOptionsValidator : AbstractValidator<GetApplicantBriefOptions>
    {
        public GetApplicantBriefOptionsValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.AccountNumber)
                .Length(12)
                .Matches(Patterns.AccountNumber)
                .Unless(x => string.IsNullOrEmpty(x.AccountNumber));

            RuleFor(x => x.ApplicationCycleId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var applicationCycles = await lookupsCache.GetApplicationCycles();
                    return applicationCycles.Any(a => a.Id == y && a.Status == Constants.ApplicationCycleStatuses.Active);
                })
                .WithMessage("'{PropertyName}' does not exist.")
                .Unless(x => x.ApplicationCycleId == null);

            RuleFor(x => x.ApplicationNumber)
                .Length(8, 9)
                .Matches(Patterns.ApplicationNumber)
                .Unless(x => string.IsNullOrEmpty(x.ApplicationNumber));

            RuleFor(x => x.ApplicationStatusId)
                 .NullableGuidNotEmpty()
                 .MustAsync(async (y, _) =>
                 {
                     var applicationStatuses = await lookupsCache.GetApplicationStatuses(Constants.Localization.EnglishCanada);
                     return applicationStatuses.Any(a => a.Id == y);
                 })
                .WithMessage("'{PropertyName}' does not exist.")
                .Unless(x => x.ApplicationStatusId == null);

            RuleFor(x => x.BirthDate)
                .Must(y => y.IsDate())
                .WithMessage(y => $"'{{PropertyName}}' is not valid: {y.BirthDate}")
                .Must(y => y.ToDateTime() < DateTime.UtcNow)
                .WithMessage(y => $"'{{PropertyName}}' cannot be in future: {y.BirthDate}")
                .Unless(x => string.IsNullOrEmpty(x.BirthDate));

            RuleFor(x => x.Email)
                .OcasEmailAddress()
                .Unless(x => string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.FirstName)
                .MaximumLength(30)
                .Matches(Patterns.Name)
                .Unless(x => string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(30)
                .Matches(Patterns.Name)
                .Unless(x => string.IsNullOrEmpty(x.LastName));

            RuleFor(x => x.MiddleName)
                .MaximumLength(30)
                .Matches(Patterns.Name)
                .Unless(x => string.IsNullOrEmpty(x.MiddleName));

            RuleFor(x => x.Mident)
                .Length(6)
                .MustAsync(async (y, _) =>
                {
                    var highSchools = await lookupsCache.GetHighSchools(Constants.Localization.EnglishCanada);
                    return highSchools.Any(h => h.Mident == y);
                })
                .WithMessage("'{PropertyName}' does not exist.")
                .Unless(x => string.IsNullOrEmpty(x.Mident));

            RuleFor(x => x.OntarioEducationNumber)
                .Length(9)
                .Matches(Patterns.OntarioEducationNumber)
                .Unless(x => string.IsNullOrEmpty(x.OntarioEducationNumber));

            RuleFor(x => x.PreviousLastName)
                .MaximumLength(30)
                .Matches(Patterns.Name)
                .Unless(x => string.IsNullOrEmpty(x.PreviousLastName));

            RuleFor(x => x.PhoneNumber)
                .Matches(CommonPattern.Patterns.InternationalPhoneNumberLengthRegex)
                .Unless(x => string.IsNullOrEmpty(x.PhoneNumber));
        }
    }
}
