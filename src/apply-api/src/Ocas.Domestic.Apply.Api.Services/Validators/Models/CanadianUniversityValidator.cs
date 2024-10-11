using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Enums;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Models
{
    public class CanadianUniversityValidator : AbstractValidator<EducationBase>
    {
        public CanadianUniversityValidator(ILookupsCache lookupsCache, IDomesticContext domesticContext, OperationType operationType = OperationType.Create)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.CountryId)
                .MustAsync(async (y, _) =>
                {
                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);
                    var country = countries.FirstOrDefault(z => z.Id == y);

                    return country?.Code == Constants.Countries.Canada;
                })
                .WithMessage(y => $"'{{PropertyName}}' is not Canada: {y.CountryId}");

            RuleFor(x => x.StudentNumber)
                .NotEmpty()
                .MaximumLength(20)
                .Matches(Patterns.StudentNumber)
                .WhenAsync(async (y, _) =>
                {
                    if (y.AcademicUpgrade == true) return true;

                    var provinceStates = await lookupsCache.GetProvinceStates(Constants.Localization.EnglishCanada);
                    var ontario = provinceStates.First(z => z.Code == Constants.Provinces.Ontario);

                    return y.ProvinceId == ontario.Id;
                });

            RuleFor(x => x.InstituteName)
                .NotEmpty()
                .MaximumLength(60)
                .Matches(Patterns.Iso8859)
                .When(y => y.InstituteId.IsEmpty());

            RuleFor(x => x.ProvinceId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);
                    var canada = countries.First(x => x.Code == Constants.Countries.Canada);

                    var provinceStates = await lookupsCache.GetProvinceStates(Constants.Localization.EnglishCanada);
                    var provinces = provinceStates.Where(x => x.CountryId == canada.Id);

                    return provinces.Any(x => x.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' is not in Canada: {y.ProvinceId}");

            RuleFor(x => x.Major)
                .NotEmpty()
                .MaximumLength(60)
                .Matches(Patterns.Iso8859);

            RuleFor(x => x.CredentialId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var credentials = await lookupsCache.GetCredentials(Constants.Localization.EnglishCanada);

                    return credentials.Any(z => z.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.CredentialId}");

            RuleFor(x => x.OtherCredential)
                .NotEmpty()
                .WhenAsync(async (y, _) =>
                {
                    var credentials = await lookupsCache.GetCredentials(Constants.Localization.EnglishCanada);
                    return y.CredentialId == credentials.FirstOrDefault(c => c.Code == Constants.Credentials.Other)?.Id;
                });

            RuleFor(x => x.InstituteId)
                .MustAsync(async (y, _) =>
                {
                    var universities = await lookupsCache.GetUniversities();
                    var hasActiveUniversity = universities.Any(x => x.Id == y && x.ShowInEducation);

                    switch (operationType)
                    {
                        case OperationType.Update:
                            if (hasActiveUniversity) return true;

                            var university = await domesticContext.GetUniversity(y.Value);
                            return university != null;
                        default:
                            return hasActiveUniversity;
                    }
                })
                .WithMessage(y => $"'{{PropertyName}}' is not an Ontario university: {y.InstituteId}")
                .When(y => !y.InstituteId.IsEmpty());

            RuleFor(x => x.InstituteTypeId)
                .MustAsync(async (y, _) =>
                {
                    var instituteTypes = await lookupsCache.GetInstituteTypes(Constants.Localization.EnglishCanada);
                    var instituteType = instituteTypes.FirstOrDefault(x => x.Id == y);

                    return instituteType?.Code == Constants.InstituteTypes.University;
                })
                .WithMessage(y => $"'{{PropertyName}}' is not university: {y.InstituteTypeId}");

            RuleFor(x => x.LevelOfStudiesId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var studyLevels = await lookupsCache.GetStudyLevels(Constants.Localization.EnglishCanada);

                    return studyLevels.Any(z => z.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.LevelOfStudiesId}");
        }
    }
}
