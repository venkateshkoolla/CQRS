using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Admin.Api.Services;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Api.Services.Utils;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Services.Handlers
{
    public class CreateProgramHandler : IRequestHandler<CreateProgram, Program>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly ILookupsCache _lookupsCache;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IDtoMapper _dtoMapper;
        private readonly IApiMapper _apiMapper;
        private readonly string _locale;

        public CreateProgramHandler(ILogger<CreateProgramHandler> logger, IDomesticContext domesticContext, ILookupsCache lookupsCache, IDtoMapper dtoMapper, IUserAuthorization userAuthorization, IApiMapper apiMapper, RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _dtoMapper = dtoMapper ?? throw new ArgumentNullException(nameof(dtoMapper));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<Program> Handle(CreateProgram request, CancellationToken cancellationToken)
        {
            // Authorize against the college ID
            await _userAuthorization.CanAccessCollegeAsync(request.User, request.Program.CollegeId);

            var modifiedBy = request.User.GetUpnOrEmail();

            var collegeApplicationCycles = await _lookupsCache.GetCollegeApplicationCycles();
            var applicationCycleId = collegeApplicationCycles.First(x => x.Id == request.Program.ApplicationCycleId).MasterId;
            var mcuCodes = await _lookupsCache.GetMcuCodes(_locale);
            var specialCodes = await _domesticContext.GetProgramSpecialCodes(request.Program.ApplicationCycleId);

            Dto.Program program;
            await _domesticContext.BeginTransaction();
            try
            {
                /////////////
                // Program //
                /////////////

                var duplicatePrograms = await _domesticContext.GetPrograms(new Dto.GetProgramsOptions
                {
                    ApplicationCycleId = applicationCycleId,
                    CampusId = request.Program.CampusId,
                    CollegeId = request.Program.CollegeId,
                    DeliveryId = request.Program.DeliveryId,
                    Code = request.Program.Code
                });
                program = duplicatePrograms.FirstOrDefault();

                if (program == null)
                {
                    var programToBeCreated = new Dto.Program();
                    _dtoMapper.PatchProgramBase(
                        programToBeCreated,
                        request.Program,
                        modifiedBy,
                        await _lookupsCache.GetMcuCodes(_locale),
                        specialCodes,
                        await _lookupsCache.GetPromotions(_locale),
                        await _lookupsCache.GetAdultTrainings(_locale));

                    program = await _domesticContext.CreateProgram(programToBeCreated);
                }
                else
                {
                    if (program.StateCode == State.Inactive)
                        await _domesticContext.ActivateProgram(program.Id);

                    _dtoMapper.PatchProgramBase(
                        program,
                        request.Program,
                        modifiedBy,
                        await _lookupsCache.GetMcuCodes(_locale),
                        specialCodes,
                        await _lookupsCache.GetPromotions(_locale),
                        await _lookupsCache.GetAdultTrainings(_locale));

                    program = await _domesticContext.UpdateProgram(program);
                }

                //////////////////////////
                // Program Entry Levels //
                //////////////////////////

                var existingEntryLevels = await _domesticContext.GetProgramEntryLevels(new Dto.GetProgramEntryLevelOptions
                {
                    ProgramId = program.Id
                });

                await ProgramMerger.MergeProgramEntryLevels(_domesticContext, existingEntryLevels, request.Program.EntryLevelIds, program.Id);

                //////////////////////////////
                // Intakes and Entry Levels //
                //////////////////////////////

                var existingIntakes = await _domesticContext.GetProgramIntakes(program.Id);

                await ProgramMerger.MergeIntakes(_domesticContext, _dtoMapper, existingIntakes, request.Program.Intakes, program, modifiedBy);

                await _domesticContext.CommitTransaction();
            }
            catch (Exception e)
            {
                await _domesticContext.RollbackTransaction(e);

                throw;
            }

            program = await _domesticContext.GetProgram(program.Id); // get program to populate entry level ids

            var applicationStatuses = await _lookupsCache.GetApplicationStatuses(Constants.Localization.FallbackLocalization);
            var applicationStatusId = applicationStatuses.First(a => a.Code == Constants.ApplicationStatuses.Active).Id;

            var dtoProgramIntakes = await _domesticContext.GetProgramIntakes(program.Id);
            var dtoProgramApplications = await _domesticContext.GetProgramApplications(new Dto.GetProgramApplicationsOptions { ProgramId = program.Id, ApplicationStatusId = applicationStatusId });

            return _apiMapper.MapProgram(program, mcuCodes, specialCodes, dtoProgramIntakes, dtoProgramApplications);
        }
    }
}
