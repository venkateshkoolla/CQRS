using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetProgramHandler : IRequestHandler<GetProgram, Program>
    {
        private readonly ILogger<GetProgramHandler> _logger;
        private readonly ILookupsCache _lookupsCache;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly string _locale;

        public GetProgramHandler(ILogger<GetProgramHandler> logger, ILookupsCache lookupsCache, IUserAuthorization userAuthorization, IDomesticContext domesticContext, RequestCache requestCache, IApiMapper apiMapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<Program> Handle(GetProgram request, CancellationToken cancellationToken)
        {
            var dtoProgram = await _domesticContext.GetProgram(request.ProgramId)
                ?? throw new NotFoundException($"Program {request.ProgramId} not found.");

            await _userAuthorization.CanAccessCollegeAsync(request.User, dtoProgram.CollegeId);

            IList<Dto.ProgramSpecialCode> specialCodes = new List<Dto.ProgramSpecialCode>();
            if (!dtoProgram.SpecialCodeId.IsEmpty())
            {
                specialCodes = await _domesticContext.GetProgramSpecialCodes(dtoProgram.CollegeApplicationCycleId);
            }

            // Map intakes to the program
            var dtoProgramIntakes = await _domesticContext.GetProgramIntakes(dtoProgram.Id);
            var applicationStatuses = await _lookupsCache.GetApplicationStatuses(Constants.Localization.FallbackLocalization);
            var applicationStatusId = applicationStatuses.First(a => a.Code == Constants.ApplicationStatuses.Active).Id;
            var dtoProgramApplications = await _domesticContext.GetProgramApplications(new Dto.GetProgramApplicationsOptions { ProgramId = request.ProgramId, ApplicationStatusId = applicationStatusId });

            return _apiMapper.MapProgram(dtoProgram, await _lookupsCache.GetMcuCodes(_locale), specialCodes, dtoProgramIntakes, dtoProgramApplications);
        }
    }
}
