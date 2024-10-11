using System;
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
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Coltrane.Bds.Provider;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetCollegeTransmissionHistoriesHandler : IRequestHandler<GetCollegeTransmissionHistories, PagedResult<CollegeTransmissionHistory>>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly IUserAuthorization _userAuthorization;
        private readonly string _locale;
        private readonly IColtraneBdsProvider _coltraneBdsProvider;
        private readonly ILookupsCache _lookupsCache;
        private readonly ITranslationsCache _translationsCache;

        public GetCollegeTransmissionHistoriesHandler(
            ILogger<GetCollegeTransmissionHistoriesHandler> logger,
            IDomesticContext domesticContext,
            IApiMapper apiMapper,
            IUserAuthorization userAuthorization,
            RequestCache requestCache,
            IColtraneBdsProvider coltraneBdsProvider,
            ILookupsCache lookupsCache,
            ITranslationsCache translationsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
            _coltraneBdsProvider = coltraneBdsProvider ?? throw new ArgumentNullException(nameof(coltraneBdsProvider));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _translationsCache = translationsCache ?? throw new ArgumentNullException(nameof(translationsCache));
        }

        public async Task<PagedResult<CollegeTransmissionHistory>> Handle(GetCollegeTransmissionHistories request, CancellationToken cancellationToken)
        {
            if (!_userAuthorization.IsOcasUser(request.User)) throw new ForbiddenException();

            var application = await _domesticContext.GetApplication(request.ApplicationId)
                ?? throw new NotFoundException($"Application {request.ApplicationId} not found");

            request.Options = request.Options ?? new GetCollegeTransmissionHistoryOptions();

            var options = new Dto.GetCollegeTransmissionHistoryOptions
            {
                FromDate = !string.IsNullOrEmpty(request.Options.FromDate) ? request.Options.FromDate.ToDateTime() : (DateTime?)null,
                ToDate = !string.IsNullOrEmpty(request.Options.ToDate) ? request.Options.ToDate.ToDateTime().AddHours(23).AddMinutes(59).AddSeconds(59) : (DateTime?)null,
                TransactionCode = request.Options.Activity != null ? request.Options.Activity.ToString().Substring(0, 2) : string.Empty,
                TransactionType = request.Options.Activity != null ? request.Options.Activity.ToString()[2] : (char?)null
            };
            var dtoCollegeTransmissions = await _coltraneBdsProvider.GetCollegeTransmissions(application.ApplicationNumber, options);

            var groupedTransmissions = dtoCollegeTransmissions.GroupBy(ct => new
            {
                ct.ColtraneXcId,
                ct.BusinessKey,
                ct.CollegeCode,
                ct.TransactionCode,
                ct.TransactionCodeId,
                ct.TransactionType,
                ct.Description,
                ct.Data,
                ct.LastLoadDateTime,
            }).Select(g => g.First());

            (var skipRows, var takeRows) = request.Options.CalculateSkipTakeRows();

            var translationsDictionary = await _translationsCache.GetTranslations(_locale, Constants.Translations.ApplyAdminApi)
                ?? throw new ValidationException("Translations can not be empty or null.");

            var colleges = await _lookupsCache.GetColleges(_locale);
            var result = _apiMapper.MapCollegeTransmissionHistories(
                                                                groupedTransmissions.Skip(skipRows).Take(takeRows).ToList(),
                                                                colleges,
                                                                dtoCollegeTransmissions,
                                                                translationsDictionary);

            return new PagedResult<CollegeTransmissionHistory>
            {
                TotalCount = groupedTransmissions.Count(),
                Items = result
            };
        }
    }
}
