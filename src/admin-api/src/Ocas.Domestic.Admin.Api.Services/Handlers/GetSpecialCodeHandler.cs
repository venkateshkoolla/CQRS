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
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetSpecialCodeHandler : IRequestHandler<GetSpecialCode, SpecialCode>
    {
        private readonly ILogger<GetSpecialCodeHandler> _logger;
        private readonly ILookupsCache _lookupsCache;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly string _locale;

        public GetSpecialCodeHandler(ILogger<GetSpecialCodeHandler> logger, ILookupsCache lookupsCache, IUserAuthorization userAuthorization, IDomesticContext domesticContext, RequestCache requestCache, IApiMapper apiMapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<SpecialCode> Handle(GetSpecialCode request, CancellationToken cancellationToken)
        {
            var collegeAppCycles = await _lookupsCache.GetCollegeApplicationCycles();
            var collegeAppCycle = collegeAppCycles.FirstOrDefault(c => c.Id == request.CollegeApplicationCycleId) ??
                throw new NotFoundException($"'College Application Cycle Id' not found: {request.CollegeApplicationCycleId}");

            await _userAuthorization.CanAccessCollegeAsync(request.User, collegeAppCycle.CollegeId);

            var dtoCodes = await _domesticContext.GetProgramSpecialCodes(request.CollegeApplicationCycleId);
            var dtoCode = dtoCodes.SingleOrDefault(s => s.Code.Equals(request.SpecialCode, StringComparison.OrdinalIgnoreCase)) ??
                throw new NotFoundException($"'Special Code' not found: {request.SpecialCode}");

            return _apiMapper.MapSpecialCode(dtoCode);
        }
    }
}
