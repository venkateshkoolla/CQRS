using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class GetCitiesValidator : AbstractValidator<GetCities>
    {
        public GetCitiesValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.ProvinceId)
                .MustAsync(async (y, _) =>
                {
                    if (y.IsEmpty()) return false;

                    var provinceStates = await lookupsCache.GetProvinceStates(Constants.Localization.EnglishCanada);
                    return provinceStates.Any(x => x.Id == y.Value);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.ProvinceId}")
                .When(x => x.ProvinceId.HasValue);
        }
    }
}
