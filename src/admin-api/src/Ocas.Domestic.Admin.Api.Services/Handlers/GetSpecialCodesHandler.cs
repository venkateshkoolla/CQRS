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
using Ocas.Domestic.Apply.Admin.Enums;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetSpecialCodesHandler : IRequestHandler<GetSpecialCodes, PagedResult<SpecialCode>>
    {
        private readonly ILogger<GetSpecialCodesHandler> _logger;
        private readonly ILookupsCache _lookupsCache;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IDomesticContext _domesticContext;
        private readonly string _locale;
        private readonly IApiMapper _apiMapper;

        public GetSpecialCodesHandler(
            ILogger<GetSpecialCodesHandler> logger,
            ILookupsCache lookupsCache,
            IUserAuthorization userAuthorization,
            IDomesticContext domesticContext,
            RequestCache requestCache,
            IApiMapper apiMapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
        }

        public async Task<PagedResult<SpecialCode>> Handle(GetSpecialCodes request, CancellationToken cancellationToken)
        {
            var collegeAppCycles = await _lookupsCache.GetCollegeApplicationCycles();
            var collegeAppCycle = collegeAppCycles.FirstOrDefault(c => c.Id == request.CollegeApplicationCycleId) ??
                throw new NotFoundException($"'College Application Cycle Id' not found: {request.CollegeApplicationCycleId}");

            await _userAuthorization.CanAccessCollegeAsync(request.User, collegeAppCycle.CollegeId);

            var specialCodes = _apiMapper.MapSpecialCodes(await _domesticContext.GetProgramSpecialCodes(request.CollegeApplicationCycleId));

            request.Params.TrimSearchString();
            (var skipRows, var takeRows) = request.Params.CalculateSkipTakeRows();
            specialCodes = specialCodes.Where(c => string.IsNullOrEmpty(request.Params.Search)
                        || c.Code == request.Params.Search
                        || c.Description.IndexOf(request.Params.Search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            return new PagedResult<SpecialCode>
            {
                TotalCount = specialCodes.Count,
                Items = Sort(specialCodes, request.Params)
                    .Skip(skipRows)
                    .Take(takeRows)
                    .ToList()
            };
        }

        private IEnumerable<SpecialCode> Sort(IEnumerable<SpecialCode> specialCodes, GetSpecialCodeOptions options)
        {
            switch (options.SortBy)
            {
                case SpecialCodeSortField.Code:
                    return options.SortDirection == SortDirection.Ascending
                        ? specialCodes.OrderBy(x => x.Code)
                        : specialCodes.OrderByDescending(x => x.Code);
                default:
                    return options.SortDirection == SortDirection.Ascending
                        ? specialCodes.OrderBy(x => x.Description)
                        : specialCodes.OrderByDescending(x => x.Description);
            }
        }
    }
}
