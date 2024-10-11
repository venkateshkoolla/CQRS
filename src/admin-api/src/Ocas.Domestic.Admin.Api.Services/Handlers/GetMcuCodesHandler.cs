using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Admin.Api.Services.Extensions;
using Ocas.Domestic.Apply.Admin.Enums;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetMcuCodesHandler : IRequestHandler<GetMcuCodes, PagedResult<McuCode>>
    {
        private readonly ILogger _logger;
        private readonly ILookupsCache _lookupsCache;
        private readonly string _locale;

        public GetMcuCodesHandler(ILogger<GetMcuCodesHandler> logger, ILookupsCache lookupsCache, RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<PagedResult<McuCode>> Handle(GetMcuCodes request, CancellationToken cancellationToken)
        {
            request.Params.TrimSearchString();
            (var skipRows, var takeRows) = request.Params.CalculateSkipTakeRows();

            var mcuCodes = await _lookupsCache.GetMcuCodes(_locale);
            mcuCodes = mcuCodes.Where(c => string.IsNullOrEmpty(request.Params.Search)
                        || c.Code == request.Params.Search
                        || c.Title.IndexOf(request.Params.Search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            return new PagedResult<McuCode>
            {
                TotalCount = mcuCodes.Count,
                Items = Sort(mcuCodes, request.Params)
                    .Skip(skipRows)
                    .Take(takeRows)
                    .ToList()
            };
        }

        private static IEnumerable<McuCode> Sort(IEnumerable<McuCode> mcuCodes, GetMcuCodeOptions options)
        {
            switch (options.SortBy)
            {
                case McuCodeSortField.Code:
                    return options.SortDirection == SortDirection.Ascending
                        ? mcuCodes.OrderBy(x => x.Code)
                        : mcuCodes.OrderByDescending(x => x.Code);
                default:
                    return options.SortDirection == SortDirection.Ascending
                        ? mcuCodes.OrderBy(x => x.Title)
                        : mcuCodes.OrderByDescending(x => x.Title);
            }
        }
    }
}
