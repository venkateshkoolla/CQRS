using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class DeleteProgramHandler : IRequestHandler<DeleteProgram>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IApiMapper _apiMapper;
        private readonly ILookupsCache _lookupsCache;

        public DeleteProgramHandler(ILogger<DeleteProgramHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization, IApiMapper apiMapper, ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
        }

        public async Task<Unit> Handle(DeleteProgram request, CancellationToken cancellationToken)
        {
            var dtoProgram = await _domesticContext.GetProgram(request.ProgramId) ??
                          throw new NotFoundException($"ProgramId does not exist: {request.ProgramId}");

            await _userAuthorization.CanAccessCollegeAsync(request.User, dtoProgram.CollegeId);

            // Lookups
            var modifiedBy = request.User.GetUpnOrEmail();
            var intakeAvailabilities = await _lookupsCache.GetIntakeAvailabilities(Constants.Localization.EnglishCanada);
            var closedId = intakeAvailabilities.First(x => x.Code == Constants.ProgramIntakeAvailabilities.Closed).Id;
            var collegeApplicationCycles = await _lookupsCache.GetCollegeApplicationCycles();
            var collegeApplicationCycleStatusId = collegeApplicationCycles.First(x => x.Id == dtoProgram.CollegeApplicationCycleId).StatusId;
            var applicationCycleCycleStatuses = await _lookupsCache.GetApplicationCycleStatuses(Constants.Localization.EnglishCanada);
            var draftApplicationCycleStatusId = applicationCycleCycleStatuses.First(x => x.Code == Constants.ApplicationCycleStatuses.Draft).Id;
            var activeApplicationCycleStatusId = applicationCycleCycleStatuses.First(x => x.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var mcuCodes = await _lookupsCache.GetMcuCodes(Constants.Localization.EnglishCanada);
            var specialCodes = await _domesticContext.GetProgramSpecialCodes(dtoProgram.CollegeApplicationCycleId);

            // get program intakes and map program to get CanDelete flag
            var applicationStatuses = await _lookupsCache.GetApplicationStatuses(Constants.Localization.FallbackLocalization);
            var applicationStatusId = applicationStatuses.First(a => a.Code == Constants.ApplicationStatuses.Active).Id;
            var dtoProgramIntakes = await _domesticContext.GetProgramIntakes(dtoProgram.Id);
            var dtoProgramApplications = await _domesticContext.GetProgramApplications(new Dto.GetProgramApplicationsOptions { ProgramId = dtoProgram.Id, ApplicationStatusId = applicationStatusId });

            var program = _apiMapper.MapProgram(dtoProgram, mcuCodes, specialCodes, dtoProgramIntakes, dtoProgramApplications);

            if (collegeApplicationCycleStatusId != draftApplicationCycleStatusId
                && (!_userAuthorization.IsOcasUser(request.User) && collegeApplicationCycleStatusId == activeApplicationCycleStatusId)
                && program.Intakes?.All(i => i.CanDelete) == false)
            {
                throw new ValidationException("Cannot delete the program.");
            }

            // close and delete intakes before deleting program
            await _domesticContext.BeginTransaction();
            try
            {
                foreach (var intake in dtoProgramIntakes)
                {
                    // Update the intake availability to closed before deleting
                    intake.AvailabilityId = closedId;
                    intake.ModifiedBy = modifiedBy;

                    await _domesticContext.UpdateProgramIntake(intake);
                    await _domesticContext.DeleteProgramIntake(intake);
                }

                await _domesticContext.DeleteProgram(dtoProgram);

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
