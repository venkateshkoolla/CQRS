using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class UpdateIntakeAvailabilityHandler : IRequestHandler<UpdateIntakeAvailability>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly ILookupsCache _lookupsCache;
        private readonly IUserAuthorization _userAuthorization;

        public UpdateIntakeAvailabilityHandler(ILogger<UpdateIntakeAvailabilityHandler> logger, IDomesticContext domesticContext, ILookupsCache lookupsCache, IUserAuthorization userAuthorization)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
        }

        public async Task<Unit> Handle(UpdateIntakeAvailability request, CancellationToken cancellationToken)
        {
            var intakes = await _domesticContext.GetProgramIntakes(new Dto.GetProgramIntakeOptions
            {
                Ids = request.IntakeIds,
                StateCode = null,
                StatusCode = null
            });

            var intakeIds = intakes.Select(x => x.Id).Except(request.IntakeIds).ToList();
            if (intakeIds.Any())
            {
                // if one of the intake ids does not exist then return 404
                throw new NotFoundException($"IntakeId does not exist: {string.Join(", ", intakeIds.Select(x => x.ToString()))}");
            }

            var collegeApplicationCycles = await _lookupsCache.GetCollegeApplicationCycles();
            var collegeApplicationCycleIds = intakes.Select(x => x.CollegeApplicationCycleId).Distinct();
            var collegeIds = collegeApplicationCycles
                .Where(x => collegeApplicationCycleIds.Any(y => y == x.Id))
                .Select(x => x.CollegeId)
                .Distinct()
                .ToList();

            // get the list of college ids to whom the intakes belong
            foreach (var collegeId in collegeIds)
            {
                // verify that the user has access to these colleges
                await _userAuthorization.CanAccessCollegeAsync(request.User, collegeId);
            }

            await _domesticContext.BeginTransaction();
            try
            {
                foreach (var crmIntake in intakes)
                {
                    crmIntake.AvailabilityId = request.AvailabilityId;
                    crmIntake.ModifiedBy = request.User.GetUpnOrEmail();

                    await _domesticContext.UpdateProgramIntake(crmIntake);
                }

                await _domesticContext.CommitTransaction();
            }
            catch (Exception e)
            {
                await _domesticContext.RollbackTransaction(e);

                throw;
            }

            return Unit.Value;
        }
    }
}
