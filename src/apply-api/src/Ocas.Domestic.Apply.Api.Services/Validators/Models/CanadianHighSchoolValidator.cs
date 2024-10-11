using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Enums;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Models
{
    public class CanadianHighSchoolValidator : AbstractValidator<EducationBase>
    {
        public CanadianHighSchoolValidator(ILookupsCache lookupsCache, IDomesticContext domesticContext, OperationType operationType = OperationType.Create)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.CityId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var cities = await lookupsCache.GetCities(Constants.Localization.EnglishCanada);

                    return cities.Any(z => z.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.CityId}")
                .WhenAsync(async (y, _) =>
                {
                    var provinces = await lookupsCache.GetProvinceStates(Constants.Localization.EnglishCanada);
                    var province = provinces.FirstOrDefault(z => z.Id == y.ProvinceId);

                    return province?.Code != Constants.Provinces.Ontario;
                });

            RuleFor(x => x.ProvinceId)
                .MustAsync(async (x, y, _) =>
                {
                    var cities = await lookupsCache.GetCities(Constants.Localization.EnglishCanada);
                    var city = cities.First(c => c.Id == x.CityId);

                    return y == city.ProvinceId;
                })
                .WithMessage(y => $"'{{PropertyName}}' province does not match City.ProvinceId: {y.ProvinceId}")
                .WhenAsync(async (y, _) =>
                {
                    var cities = await lookupsCache.GetCities(Constants.Localization.EnglishCanada);

                    return !y.CityId.IsEmpty()
                           && !y.ProvinceId.IsEmpty()
                           && cities.Any(z => z.Id == y.CityId);
                });

            RuleFor(x => x.CountryId)
                .MustAsync(async (y, _) =>
                {
                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);
                    var country = countries.FirstOrDefault(z => z.Id == y);

                    return country?.Code == Constants.Countries.Canada;
                })
                .WithMessage(y => $"'{{PropertyName}}' is not Canada: {y.CountryId}");

            RuleFor(x => x.Graduated)
                .NotEmpty()
                .When(y => y.CurrentlyAttending == false);

            RuleFor(x => x.InstituteId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var highSchools = await lookupsCache.GetHighSchools(Constants.Localization.EnglishCanada);
                    var hasActiveHighSchool = highSchools.Any(x => x.Id == y && x.ShowInEducation);

                    switch (operationType)
                    {
                        case OperationType.Update:
                            if (hasActiveHighSchool) return true;

                            var highSchool = await domesticContext.GetHighSchool(y.Value, Domestic.Enums.Locale.English);
                            return highSchool != null;
                        default:
                            return hasActiveHighSchool;
                    }
                })
                .WithMessage(y => $"'{{PropertyName}}' is not an Ontario high school: {y.InstituteId}")
                .WhenAsync(async (y, _) =>
                {
                    var provinces = await lookupsCache.GetProvinceStates(Constants.Localization.EnglishCanada);
                    var province = provinces.FirstOrDefault(z => z.Id == y.ProvinceId);

                    return province?.Code == Constants.Provinces.Ontario;
                });

            RuleFor(x => x.InstituteName)
                .NotEmpty()
                .MaximumLength(100)
                .Matches(Patterns.Iso8859)
                .When(y => y.InstituteId.IsEmpty());

            RuleFor(x => x.InstituteTypeId)
                .MustAsync(async (y, _) =>
                {
                    var instituteTypes = await lookupsCache.GetInstituteTypes(Constants.Localization.EnglishCanada);
                    var instituteType = instituteTypes.FirstOrDefault(x => x.Id == y);

                    return instituteType?.Code == Constants.InstituteTypes.HighSchool;
                })
                .WithMessage(y => $"'{{PropertyName}}' is not university: {y.InstituteTypeId}");

            RuleFor(x => x.LastGradeCompletedId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var grades = await lookupsCache.GetGrades(Constants.Localization.EnglishCanada);

                    return grades.Any(z => z.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.LastGradeCompletedId}")
                .When(y => y.CurrentlyAttending == false);

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

            RuleFor(x => x.StudentNumber)
                .NotEmpty()
                .MaximumLength(12)
                .Matches(Patterns.StudentNumber);

            RuleFor(x => x.OntarioEducationNumber)
                .OenValid()
                .When(x => x.OntarioEducationNumber != Constants.Education.DefaultOntarioEducationNumber)
                .WhenAsync(async (y, _) =>
                {
                    var provinces = await lookupsCache.GetProvinceStates(Constants.Localization.EnglishCanada);
                    var province = provinces.FirstOrDefault(z => z.Id == y.ProvinceId);

                    return province?.Code == Constants.Provinces.Ontario;
                });
        }
    }
}
