using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class ProgramCodeAvailableHandler : IRequestHandler<ProgramCodeAvailable, bool>
    {
        private readonly ILogger<ProgramCodeAvailableHandler> _logger;
        private readonly ILookupsCache _lookupsCache;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IDomesticContext _domesticContext;

        public ProgramCodeAvailableHandler(ILogger<ProgramCodeAvailableHandler> logger, ILookupsCache lookupsCache, IUserAuthorization userAuthorization, IDomesticContext domesticContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
        }

        public async Task<bool> Handle(ProgramCodeAvailable request, CancellationToken cancellationToken)
        {
            var collegeAppCycles = await _lookupsCache.GetCollegeApplicationCycles();
            var collegeAppCycle = collegeAppCycles.First(c => c.Id == request.CollegeApplicationCycleId);

            await _userAuthorization.CanAccessCollegeAsync(request.User, collegeAppCycle.CollegeId);

            var options = new Dto.GetProgramsOptions
            {
                ApplicationCycleId = request.CollegeApplicationCycleId,
                CollegeId = collegeAppCycle.CollegeId,
                CampusId = request.CampusId,
                DeliveryId = request.DeliveryId,
                Code = request.Code
            };

            var dtoPrograms = await _domesticContext.GetPrograms(options);

            return !dtoPrograms.Any();
        }
    }
}
