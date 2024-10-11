using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class ProgramCodeAvailableValidator : AbstractValidator<ProgramCodeAvailable>
    {
        public ProgramCodeAvailableValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.Code)
                .NotEmpty()
                .Length(2, 8);

            RuleFor(x => x.CollegeApplicationCycleId)
               .NotEmpty()
               .MustAsync(async (y, _) =>
               {
                   var appCycles = await lookupsCache.GetCollegeApplicationCycles();
                   return appCycles.Any(c => c.Id == y);
               })
               .WithMessage("'{PropertyName}' does not exist.");

            RuleFor(x => x.CampusId)
               .NotEmpty()
               .MustAsync(async (y, _) =>
               {
                   var campuses = await lookupsCache.GetCampuses();
                   return campuses.Any(c => c.Id == y);
               })
               .WithMessage("'{PropertyName}' does not exist.")
               .MustAsync(async (x, y, _) =>
               {
                   var collegeAppCycles = await lookupsCache.GetCollegeApplicationCycles();
                   var collegeAppCycle = collegeAppCycles.FirstOrDefault(a => a.Id == x.CollegeApplicationCycleId);
                   if (collegeAppCycle is null) return false;

                   var campuses = await lookupsCache.GetCampuses();
                   return campuses.Any(c => c.Id == y && c.CollegeId == collegeAppCycle.CollegeId);
               })
               .WithMessage("'{PropertyName}' does not exist in college's application cycle.");

            RuleFor(x => x.DeliveryId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var studyMethods = await lookupsCache.GetStudyMethods(Constants.Localization.FallbackLocalization);
                    return studyMethods.Any(c => c.Id == y);
                })
                .WithMessage("'{PropertyName}' does not exist.");
        }
    }
}
