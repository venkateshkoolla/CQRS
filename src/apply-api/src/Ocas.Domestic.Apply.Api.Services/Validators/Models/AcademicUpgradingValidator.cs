using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Enums;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Models
{
    public class AcademicUpgradingValidator : AbstractValidator<EducationBase>
    {
        public AcademicUpgradingValidator(ILookupsCache lookupsCache, IDomesticContext domesticContext, OperationType operationType = OperationType.Create)
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
                .Matches(Patterns.StudentNumber);

            RuleFor(x => x.ProvinceId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var provinces = await lookupsCache.GetProvinceStates(Constants.Localization.EnglishCanada);
                    var province = provinces.FirstOrDefault(z => z.Id == y);

                    return province?.Code == Constants.Provinces.Ontario;
                })
                .WithMessage(y => $"'{{PropertyName}}' is not Ontario: {y.ProvinceId}");

            RuleFor(x => x.InstituteId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var colleges = await lookupsCache.GetColleges(Constants.Localization.EnglishCanada);
                    var hasActiveCollege = colleges.Any(x => x.Id == y);

                    switch (operationType)
                    {
                        case OperationType.Update:
                            if (hasActiveCollege) return true;

                            var college = await domesticContext.GetCollege(y.Value);
                            return college != null;
                        default:
                            return hasActiveCollege;
                    }
                })
                .WithMessage(y => $"'{{PropertyName}}' is not an Ontario college: {y.InstituteId}");

            RuleFor(x => x.InstituteTypeId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var instituteTypes = await lookupsCache.GetInstituteTypes(Constants.Localization.EnglishCanada);
                    var instituteType = instituteTypes.FirstOrDefault(x => x.Id == y);

                    return instituteType?.Code == Constants.InstituteTypes.College;
                })
                .WithMessage(y => $"'{{PropertyName}}' is not college: {y.InstituteTypeId}");
        }
    }
}
