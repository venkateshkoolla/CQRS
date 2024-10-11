using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Core.Extensions;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class GetIntakesReportValidator : AbstractValidator<GetIntakesReport>
    {
        private readonly ILookupsCache _lookupsCache;

        public GetIntakesReportValidator(ILookupsCache lookupsCache)
        {
            _lookupsCache = lookupsCache;
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new ICollegeUserValidator(lookupsCache));

            RuleFor(x => x.ApplicationCycleId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var applicationCycles = await lookupsCache.GetApplicationCycles();
                    return applicationCycles.Any(a => a.Id == y);
                })
                .WithMessage("'{PropertyName}' does not exist.");

            RuleFor(x => x.Params.StartDate)
                .MustAsync((x, w, _) => IsValidStartDate(x, w))
                .WithMessage("'Start Date' must be within the application cycle")
                .When(y => !string.IsNullOrEmpty(y.Params.StartDate));

            RuleFor(x => x.Params.CampusId)
                .MustAsync(async (x, y, _) =>
                {
                    var campuses = await lookupsCache.GetCampuses();
                    return campuses.Any(c => c.CollegeId == x.CollegeId && c.Id == y);
                })
                .WithMessage("'Campus Id' is not in requested college")
                .When(x => !x.Params.CampusId.IsEmpty());

            RuleFor(x => x.Params)
                .SetValidator(new GetIntakesOptionsValidator(lookupsCache));
        }

        private async Task<bool> IsValidStartDate(GetIntakesReport intakesReport, string startDate)
        {
            var applicationCycles = await _lookupsCache.GetApplicationCycles();
            var crmAppCycle = applicationCycles.First(x => x.Id == intakesReport.ApplicationCycleId);
            if (crmAppCycle == null) return false;

            var appCycleYear = int.Parse(crmAppCycle.Year);

            var validStartDates = new List<string>();
            for (var i = 8; i <= 19; i++)
            {
                var year = i > 12 ? appCycleYear + 1 : appCycleYear;
                var month = i > 12 ? i - 12 : i;
                var paddedMonth = month.ToString("#00");
                validStartDates.Add(year.ToString().Substring(2) + paddedMonth);
            }

            return validStartDates.Contains(startDate);
        }
    }
}
