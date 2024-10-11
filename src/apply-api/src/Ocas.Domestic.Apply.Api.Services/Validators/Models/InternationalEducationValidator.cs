using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Models
{
    public class InternationalEducationValidator : AbstractValidator<EducationBase>
    {
        public InternationalEducationValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.CredentialId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var credentials = await lookupsCache.GetCredentials(Constants.Localization.EnglishCanada);

                    return credentials.Any(z => z.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.CredentialId}")
                .WhenAsync(async (y, _) =>
                {
                    var instituteTypes = await lookupsCache.GetInstituteTypes(Constants.Localization.EnglishCanada);
                    var instituteType = instituteTypes.FirstOrDefault(z => z.Id == y.InstituteTypeId);

                    if (instituteType is null) return false;

                    return instituteType.Code != Constants.InstituteTypes.HighSchool;
                });

            RuleFor(x => x.CountryId)
                .MustAsync(async (y, _) =>
                {
                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);
                    var country = countries.FirstOrDefault(z => z.Id == y);

                    return country?.Code != Constants.Countries.Canada;
                })
                .WithMessage(y => $"'{{PropertyName}}'is not international: {y.CountryId}");

            RuleFor(x => x.InstituteName)
                .NotEmpty()
                .MaximumLength(60)
                .Matches(Patterns.Iso8859);

            RuleFor(x => x.LevelAchievedId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var achievementLevels = await lookupsCache.GetAchievementLevels(Constants.Localization.EnglishCanada);

                    return achievementLevels.Any(z => z.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.LevelAchievedId}");

            RuleFor(x => x.Major)
                .NotEmpty()
                .Matches(Patterns.Iso8859)
                .WhenAsync(async (y, _) =>
                {
                    var instituteTypes = await lookupsCache.GetInstituteTypes(Constants.Localization.EnglishCanada);
                    var instituteType = instituteTypes.FirstOrDefault(z => z.Id == y.InstituteTypeId);

                    if (instituteType is null) return false;

                    return instituteType.Code != Constants.InstituteTypes.HighSchool;
                });
        }
    }
}
