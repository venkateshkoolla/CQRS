using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Extensions;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.AppSettings.Extras;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetApplicationSummariesHandler : IRequestHandler<GetApplicationSummaries, IList<ApplicationSummary>>
    {
        private readonly ILogger<GetApplicationSummariesHandler> _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly IUserAuthorization _userAuthorization;
        private readonly ILookupsCache _lookupsCache;
        private readonly IAppSettingsExtras _appSettingsExtras;
        private readonly string _locale;

        public GetApplicationSummariesHandler(ILogger<GetApplicationSummariesHandler> logger, IDomesticContext domesticContext, IApiMapper apiMapper, IUserAuthorization userAuthorization, ILookupsCache lookupsCache, IAppSettingsExtras appSettingsExtras, RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _appSettingsExtras = appSettingsExtras ?? throw new ArgumentNullException(nameof(appSettingsExtras));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<IList<ApplicationSummary>> Handle(GetApplicationSummaries request, CancellationToken cancellationToken)
        {
            var dtoApplication = await _domesticContext.GetApplication(request.ApplicationId) ??
                throw new NotFoundException($"Application {request.ApplicationId} not found.");
            await _userAuthorization.CanAccessApplicantAsync(request.User, dtoApplication.ApplicantId);

            var dtoApplicantSummary = await _domesticContext.GetApplicantSummary(new Dto.GetApplicantSummaryOptions
            {
                ApplicantId = dtoApplication.ApplicantId,
                Locale = _locale.ToLocaleEnum()
            });

            var applicationSummaries = new List<ApplicationSummary>();
            foreach (var dtoApplicationSummary in dtoApplicantSummary.ApplicationSummaries)
            {
                applicationSummaries.Add(_apiMapper.MapApplicationSummary(
                        dtoApplicationSummary,
                        dtoApplicantSummary.Contact,
                        await GetProgramIntakes(dtoApplicationSummary.ProgramChoices),
                        await _lookupsCache.GetApplicationStatuses(_locale),
                        await _lookupsCache.GetInstituteTypes(_locale),
                        await _lookupsCache.GetTranscriptTransmissions(_locale),
                        await _lookupsCache.GetOfferStates(_locale),
                        await _lookupsCache.GetProgramIntakeAvailabilities(_locale),
                        _appSettingsExtras));
            }

            return applicationSummaries;
        }

        private async Task<IList<Dto.ProgramIntake>> GetProgramIntakes(IList<Dto.ProgramChoice> programChoices)
        {
            var filterChoices = programChoices.Where(c => c.SequenceNumber <= Constants.ProgramChoices.MaxTotalChoices).ToList();
            IList<Dto.ProgramIntake> intakes = null;
            if (filterChoices.Any())
            {
                var intakeIds = filterChoices.Select(c => c.ProgramIntakeId).Distinct().ToList();
                intakes = await _domesticContext.GetProgramIntakes(new Dto.GetProgramIntakeOptions { Ids = intakeIds });
            }

            return intakes;
        }
    }
}
