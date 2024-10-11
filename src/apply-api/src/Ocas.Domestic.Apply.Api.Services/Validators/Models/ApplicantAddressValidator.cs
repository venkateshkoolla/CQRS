using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Models
{
    public class ApplicantAddressValidator : AbstractValidator<ApplicantAddress>
    {
        public ApplicantAddressValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Street)
                .NotEmpty()
                .MaximumLength(50)
                .Matches(Patterns.Iso8859);

            RuleFor(x => x.City)
                .NotEmpty()
                .MaximumLength(30)
                .Matches(Patterns.Iso8859);

            RuleFor(x => x.CountryId)
                .NullableGuidNotEmpty();

            RuleFor(x => x.PostalCode)
                .NotEmpty()
                .MaximumLength(10)
                .Matches(Patterns.Iso8859);

            RuleFor(x => x)
                .MustAsync(async (y, _) =>
                {
                    if (y.CountryId.IsEmpty()) return false;

                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);
                    return countries.Any(x => x.Id == y.CountryId);
                })
                .WithMessage(y => $"'Country Id' does not exist: {y.CountryId}")
                .MustAsync(async (y, _) =>
                {
                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);

                    var country = countries.Single(x => x.Id == y.CountryId);

                    // Skip this validator for non-Canada and non-US
                    if (country.Code != Constants.Countries.Canada && country.Code != Constants.Countries.UnitedStates)
                    {
                        return true;
                    }

                    return !y.ProvinceStateId.IsEmpty();
                })
                .WithMessage(y => $"'Province State Id' is missing: {y.ProvinceStateId}")
                .MustAsync(async (y, _) =>
                {
                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);

                    var country = countries.Single(x => x.Id == y.CountryId);

                    // Skip this validator for non-Canada and non-US
                    if (country.Code != Constants.Countries.Canada && country.Code != Constants.Countries.UnitedStates)
                    {
                        return true;
                    }

                    if (!y.ProvinceStateId.IsEmpty())
                    {
                        var provinceStates = await lookupsCache.GetProvinceStates(Constants.Localization.EnglishCanada);
                        return provinceStates.Any(x => x.Id == y.ProvinceStateId);
                    }

                    return false;
                })
                .WithMessage(y => $"'Province State Id' does not exist: {y.ProvinceStateId}")
                .MustAsync(async (y, _) =>
                {
                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);

                    var country = countries.Single(x => x.Id == y.CountryId);

                    // Skip this validator for non-Canada and non-US
                    if (country.Code != Constants.Countries.Canada && country.Code != Constants.Countries.UnitedStates)
                    {
                        return true;
                    }

                    var provinceStates = await lookupsCache.GetProvinceStates(Constants.Localization.EnglishCanada);
                    var provinceState = provinceStates.Single(x => x.Id == y.ProvinceStateId.Value);

                    return y.CountryId == provinceState.CountryId;
                })
                .WithMessage(y => $"ProvinceState.CountryId does not match Applicant.MailingAddress.CountryId: {y.CountryId}")
                .MustAsync(async (y, _) =>
                {
                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);

                    var country = countries.Single(x => x.Id == y.CountryId);

                    switch (country.Code)
                    {
                        case Constants.Countries.Canada:
                            return !string.IsNullOrEmpty(y.PostalCode) && Patterns.PostalCode.IsMatch(y.PostalCode);
                        case Constants.Countries.UnitedStates:
                            return !string.IsNullOrEmpty(y.PostalCode) && Patterns.ZipCode.IsMatch(y.PostalCode);
                        default:
                            return true;
                    }
                })
                .WithMessage(y => $"'Postal Code' is invalid: {y.PostalCode}");
        }
    }
}
