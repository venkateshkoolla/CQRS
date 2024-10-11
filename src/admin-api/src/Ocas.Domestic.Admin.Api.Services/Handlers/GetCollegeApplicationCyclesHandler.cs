using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Admin.Core.Settings;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetCollegeApplicationCyclesHandler : IRequestHandler<GetCollegeApplicationCycles, IList<CollegeApplicationCycle>>
    {
        private readonly ILogger<GetCollegeApplicationCyclesHandler> _logger;
        private readonly ILookupsCache _lookupsCache;
        private readonly IAppSettings _appSettings;

        public GetCollegeApplicationCyclesHandler(
            ILogger<GetCollegeApplicationCyclesHandler> logger,
            ILookupsCache lookupsCache,
            IAppSettings appSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task<IList<CollegeApplicationCycle>> Handle(GetCollegeApplicationCycles request, CancellationToken cancellationToken)
        {
            var applicationCycleStatuses = await _lookupsCache.GetApplicationCycleStatuses(Constants.Localization.FallbackLocalization);
            var appCycleStatusActiveId = applicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var appCycleStatusDraftId = applicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Draft).Id;

            var overrideIntakeSemesters = _appSettings.GetAppSetting<bool>("ocas:feature:intakeSemesterOverride");
            var collegeApplicationCycles = await _lookupsCache.GetCollegeApplicationCycles();

            return collegeApplicationCycles
                .Where(a => (a.StatusId == appCycleStatusActiveId || a.StatusId == appCycleStatusDraftId) && a.CollegeId == request.CollegeId)
                .Select(c =>
                {
                    var cAppCycle = new CollegeApplicationCycle();
                    cAppCycle = c;
                    cAppCycle.EnableIntakeOverride = overrideIntakeSemesters;
                    return cAppCycle;
                }).ToList();
        }
    }
}
