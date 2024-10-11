using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models
{
    public class GetIntakesOptionsValidator : AbstractValidator<GetIntakesOptions>
    {
        public GetIntakesOptionsValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.CampusId)
                .MustAsync(async (y, _) =>
                {
                    var campuses = await lookupsCache.GetCampuses();
                    return campuses.Any(c => c.Id == y.Value);
                })
                .WithMessage("'{PropertyName}' does not exist.")
                .When(x => !x.CampusId.IsEmpty());

            RuleFor(x => x.DeliveryId)
                .MustAsync(async (y, _) =>
                {
                    var studyMethods = await lookupsCache.GetProgramDeliveries(Constants.Localization.FallbackLocalization);
                    return studyMethods.Any(c => c.Id == y.Value);
                })
                .WithMessage("'{PropertyName}' does not exist.")
                .When(x => !x.DeliveryId.IsEmpty());

            RuleFor(x => x.ProgramCode)
                .Length(2, 8)
                .When(x => !string.IsNullOrEmpty(x.ProgramCode));

            RuleFor(x => x.ProgramTitle)
                .MaximumLength(200)
                .When(x => !string.IsNullOrEmpty(x.ProgramTitle));
        }
    }
}
