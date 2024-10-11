using System.Linq;
using FluentValidation;
using Newtonsoft.Json;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Services.Validators;
using CommonPattern = Ocas.Domestic.Apply.Services.Validators;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Models
{
    public class ApplicantValidator : AbstractValidator<Applicant>
    {
        public ApplicantValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.AboriginalStatuses)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var aboriginalStatuses = await lookupsCache.GetAboriginalStatuses(Constants.Localization.EnglishCanada);
                    var portalAboriginalStatuses = aboriginalStatuses.Where(z => z.ShowInPortal).ToList();

                    return y.All(z => portalAboriginalStatuses.Any(t => t.Id == z));
                })
                .WithMessage(y => $"'{{PropertyName}}' at least 1 guid does not exist: {JsonConvert.SerializeObject(y.AboriginalStatuses)}")
                .Must((y, _) => y.AboriginalStatuses.Distinct().Count() == y.AboriginalStatuses.Count())
                .WithMessage(y => $"'{{PropertyName}}' cannot contain duplicates: {JsonConvert.SerializeObject(y.AboriginalStatuses)}")
                .When(y => y.IsAboriginalPerson == true);

            RuleFor(x => x.CountryOfCitizenshipId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);

                    return countries.Any(x => x.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.CountryOfCitizenshipId}");

            RuleFor(x => x.Email)
                .NotEmpty()
                .OcasEmailAddress();

            RuleFor(x => x.FirstGenerationId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var firstGenerationApplicants = await lookupsCache.GetFirstGenerationApplicants(Constants.Localization.EnglishCanada);

                    return firstGenerationApplicants.Any(x => x.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.FirstGenerationId}");

            RuleFor(x => x.FirstLanguageId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var firstLanguages = await lookupsCache.GetFirstLanguages(Constants.Localization.EnglishCanada);

                    return firstLanguages.Any(x => x.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.FirstLanguageId}");

            RuleFor(x => x.GenderId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var genders = await lookupsCache.GetGenders(Constants.Localization.EnglishCanada);

                    return genders.Any(x => x.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.GenderId}");

            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.IsAboriginalPerson)
                .NotEmpty();

            RuleFor(x => x.HomePhone)
                .NotEmpty()
                .When(y => string.IsNullOrEmpty(y.MobilePhone));

            RuleFor(x => x.MailingAddress)
                .NotEmpty()
                .SetValidator(new ApplicantAddressValidator(lookupsCache));

            RuleFor(x => x.MobilePhone)
                .NotEmpty()
                .When(y => string.IsNullOrEmpty(y.HomePhone));

            RuleFor(x => x.OtherAboriginalStatus)
                .NotEmpty()
                .MaximumLength(30)
                .Matches(Patterns.Iso8859)
                .When(x => x.IsAboriginalPerson == true)
                .WhenAsync(async (y, _) =>
                {
                    if (y.AboriginalStatuses is null)
                        return false;

                    var aboriginalStatuses = await lookupsCache.GetAboriginalStatuses(Constants.Localization.EnglishCanada);
                    var selectedAboriginalStatuses = aboriginalStatuses.Where(z => y.AboriginalStatuses.Contains(z.Id)).ToList();

                    return selectedAboriginalStatuses.Any(z => z.Code == Constants.AboriginalStatuses.Other);
                });

            RuleFor(x => x.PreferredLanguageId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var preferredLanguages = await lookupsCache.GetPreferredLanguages(Constants.Localization.EnglishCanada);
                    return preferredLanguages.Any(x => x.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.PreferredLanguageId}");

            RuleFor(x => x.PreferredName)
                .MaximumLength(15)
                .Matches(Patterns.Name)
                .Unless(x => string.IsNullOrEmpty(x.PreferredName));

            RuleFor(x => x.PreviousLastName)
                .MaximumLength(30)
                .Matches(Patterns.Name)
                .Unless(x => string.IsNullOrEmpty(x.PreviousLastName));

            RuleFor(x => x.SponsorAgencyId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var sponsorAgencies = await lookupsCache.GetSponsorAgencies(Constants.Localization.EnglishCanada);
                    return sponsorAgencies.Any(x => x.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.SponsorAgencyId}");

            RuleFor(x => x.StatusInCanadaId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var canadianStatuses = await lookupsCache.GetCanadianStatuses(Constants.Localization.EnglishCanada);
                    return canadianStatuses.Any(x => x.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.StatusInCanadaId}")
                .WhenAsync(async (y, _) =>
                {
                    if (y.CountryOfCitizenshipId.IsEmpty()) return false;

                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);
                    return countries.Any(x => x.Id == y.CountryOfCitizenshipId)
                           && countries.Single(x => x.Id == y.CountryOfCitizenshipId).Code != Constants.Countries.Canada;
                });

            RuleFor(x => x.StatusOfVisaId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var visaStatuses = await lookupsCache.GetVisaStatuses(Constants.Localization.EnglishCanada);
                    return visaStatuses.Any(x => x.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.StatusOfVisaId}")
                .WhenAsync(async (y, _) =>
                {
                    if (y.CountryOfCitizenshipId.IsEmpty()) return false;

                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);
                    var country = countries.FirstOrDefault(x => x.Id == y.CountryOfCitizenshipId);

                    if (country?.Code == Constants.Countries.Canada) return false;

                    var canadianStatuses = await lookupsCache.GetCanadianStatuses(Constants.Localization.EnglishCanada);

                    var statusInCanada = canadianStatuses.FirstOrDefault(x => x.Id == y.StatusInCanadaId);

                    return statusInCanada?.Code == Constants.CanadianStatuses.StudyPermit;
                });

            RuleFor(x => x.TitleId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var titles = await lookupsCache.GetTitles(Constants.Localization.EnglishCanada);
                    return titles.Any(x => x.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.TitleId}")
                .When(x => !x.TitleId.IsEmpty());

            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(100);

            // This rule will execute in parallel with the x.MailingAddress rule above, so we must null-check here as well
            RuleFor(x => x)
                .Must((y, _) => y.MailingAddress != null)
                .WithMessage(_ => "'Mailing Address' is null")
                .MustAsync(async (y, _) =>
                {
                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);

                    return countries.Any(x => x.Id == y.MailingAddress.CountryId);
                })
                .WithMessage(y => $"'Country Id' does not exist: {y.MailingAddress.CountryId}")
                .MustAsync(async (y, _) =>
                {
                    if (string.IsNullOrEmpty(y.HomePhone)) return true;

                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);

                    var country = countries.Single(x => x.Id == y.MailingAddress.CountryId);

                    switch (country.Code)
                    {
                        case Constants.Countries.Canada:
                        case Constants.Countries.UnitedStates:
                            return Patterns.NorthAmericanPhoneNumberLengthRegex.IsMatch(y.HomePhone);
                        default:
                            return CommonPattern.Patterns.InternationalPhoneNumberLengthRegex.IsMatch(y.HomePhone);
                    }
                })
                .WithMessage(y => $"'Home Phone' is invalid: {y.HomePhone}")
                .MustAsync(async (y, _) =>
                {
                    if (string.IsNullOrEmpty(y.MobilePhone)) return true;

                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);

                    var country = countries.Single(x => x.Id == y.MailingAddress.CountryId);

                    switch (country.Code)
                    {
                        case Constants.Countries.Canada:
                        case Constants.Countries.UnitedStates:
                            return Patterns.NorthAmericanPhoneNumberLengthRegex.IsMatch(y.MobilePhone);
                        default:
                            return CommonPattern.Patterns.InternationalPhoneNumberLengthRegex.IsMatch(y.MobilePhone);
                    }
                })
                .WithMessage(y => $"'Mobile Phone' is invalid: {y.MobilePhone}")
                .Must(y => string.IsNullOrEmpty(y.DateOfArrival) || y.DateOfArrival.IsDate())
                .WithMessage("'Date Of Arrival' must be a valid date 'yyyy-MM-dd'")
                .Must(y => !y.CountryOfBirthId.IsEmpty())
                .WithMessage("'Country Of Birth Id' must not be empty.")
                .MustAsync(async (y, _) =>
                {
                    var countries = await lookupsCache.GetCountries(Constants.Localization.EnglishCanada);
                    return countries.Any(x => x.Id == y.CountryOfBirthId);
                })
                .WithMessage(y => $"'Country Of Birth Id' does not exist: {y.CountryOfBirthId}");

            RuleFor(x => x)
                .SetValidator(new ApplicantBaseValidator());
        }
    }
}