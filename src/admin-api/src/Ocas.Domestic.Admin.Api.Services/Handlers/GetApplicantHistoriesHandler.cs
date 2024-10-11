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
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetApplicantHistoriesHandler : IRequestHandler<GetApplicantHistories, PagedResult<ApplicantHistory>>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly IUserAuthorization _userAuthorization;
        private readonly string _locale;
        private readonly ILookupsCache _lookupsCache;

        public GetApplicantHistoriesHandler(ILogger<GetApplicantHistoriesHandler> logger, IDomesticContext domesticContext, IApiMapper apiMapper, IUserAuthorization userAuthorization, RequestCache requestCache, ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
        }

        public async Task<PagedResult<ApplicantHistory>> Handle(GetApplicantHistories request, CancellationToken cancellationToken)
        {
            if (!_userAuthorization.IsOcasUser(request.User)) throw new ForbiddenException();

            DateTime? fromDate = null;
            DateTime? toDate = null;

            request.Options = request.Options ?? new GetApplicantHistoryOptions();

            if (!string.IsNullOrEmpty(request.Options.FromDate))
                fromDate = request.Options.FromDate.ToDateTime();

            if (!string.IsNullOrEmpty(request.Options.ToDate))
                toDate = request.Options.ToDate.ToDateTime().AddDays(1);

            var dtoCustomAuditsByApplicant = await _domesticContext.GetCustomAudits(
                new Dto.GetCustomAuditOptions
                {
                    ApplicantId = request.ApplicantId,
                    ApplicationId = null,
                    FromDate = fromDate,
                    ToDate = toDate
                }, _locale.ToLocaleEnum());

            IList<Dto.CustomAudit> dtoCustomAuditsByApplication = new List<Dto.CustomAudit>();
            if (request.ApplicationId.HasValue)
            {
                var auditsByApplication = await _domesticContext.GetCustomAudits(
                     new Dto.GetCustomAuditOptions
                     {
                         ApplicantId = null,
                         ApplicationId = request.ApplicationId,
                         FromDate = fromDate,
                         ToDate = toDate
                     }, _locale.ToLocaleEnum());

                dtoCustomAuditsByApplication = dtoCustomAuditsByApplication.Union(auditsByApplication).ToList();
            }
            else
            {
                var applications = await _domesticContext.GetApplications(request.ApplicantId);
                foreach (var application in applications)
                {
                    var auditsByApplication = await _domesticContext.GetCustomAudits(
                     new Dto.GetCustomAuditOptions
                     {
                         ApplicantId = null,
                         ApplicationId = application.Id,
                         FromDate = fromDate,
                         ToDate = toDate
                     }, _locale.ToLocaleEnum());

                    dtoCustomAuditsByApplication = dtoCustomAuditsByApplication.Union(auditsByApplication).ToList();
                }
            }

            (var skipRows, var takeRows) = request.Options.CalculateSkipTakeRows();

            var filteredAudits = dtoCustomAuditsByApplicant.Union(dtoCustomAuditsByApplication)
                                                           .GroupBy(x => x.Id)
                                                           .Select(grp => grp.First())
                                                           .Where(x => x.CustomEntityLabelEn?.ToUpperInvariant().Trim() != "ORDER" && x.OrderId == null)
                                                           .OrderByDescending(x => x.CreatedOn);
            return new PagedResult<ApplicantHistory>
            {
                TotalCount = filteredAudits.Count(),
                Items = _apiMapper.MapApplicantHistories(filteredAudits.Skip(skipRows).Take(takeRows).ToList())
            };
        }
    }
}
