using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Enums;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetProgramBriefsHandler : IRequestHandler<GetProgramBriefs, IList<ProgramBrief>>
    {
        private readonly ILogger<GetProgramBriefsHandler> _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly ILookupsCache _lookupsCache;
        private readonly string _locale;
        private readonly IApiMapper _apiMapper;

        public GetProgramBriefsHandler(
            ILogger<GetProgramBriefsHandler> logger,
            IDomesticContext domesticContext,
            ILookupsCache lookupsCache,
            RequestCache requestCache,
            IApiMapper apiMapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
        }

        public async Task<IList<ProgramBrief>> Handle(GetProgramBriefs request, CancellationToken cancellationToken)
        {
            var options = new Dto.GetProgramsOptions
            {
                ApplicationCycleId = request.ApplicationCycleId,
                CollegeId = request.CollegeId,
                CampusId = request.Params.CampusId,
                DeliveryId = request.Params.DeliveryId,
                Code = request.Params.Code,
                Title = request.Params.Title
            };
            var dtoPrograms = await _domesticContext.GetPrograms(options);

            var deliveries = await _lookupsCache.GetProgramDeliveries(_locale);
            var colleges = await _lookupsCache.GetColleges(_locale);
            var campuses = await _lookupsCache.GetCampuses();

            var programBriefs = _apiMapper.MapProgramBriefs(dtoPrograms, deliveries, colleges, campuses);

            return Sort(programBriefs, request.Params);
        }

        private IList<ProgramBrief> Sort(IList<ProgramBrief> programBriefs, GetProgramBriefOptions options)
        {
            switch (options.SortBy)
            {
                case ProgramSortField.Code:
                    return options.SortDirection == SortDirection.Ascending
                        ? programBriefs.OrderBy(x => x.Code).ToList()
                        : programBriefs.OrderByDescending(x => x.Code).ToList();
                case ProgramSortField.Title:
                    return options.SortDirection == SortDirection.Ascending
                        ? programBriefs.OrderBy(x => x.Title).ToList()
                        : programBriefs.OrderByDescending(x => x.Title).ToList();
                case ProgramSortField.Delivery:
                    return options.SortDirection == SortDirection.Ascending
                        ? programBriefs.OrderBy(x => x.Delivery).ToList()
                        : programBriefs.OrderByDescending(x => x.Delivery).ToList();
                case ProgramSortField.College:
                    return options.SortDirection == SortDirection.Ascending
                        ? programBriefs.OrderBy(x => x.College).ToList()
                        : programBriefs.OrderByDescending(x => x.College).ToList();
                case ProgramSortField.Campus:
                    return options.SortDirection == SortDirection.Ascending
                        ? programBriefs.OrderBy(x => x.Campus).ToList()
                        : programBriefs.OrderByDescending(x => x.Campus).ToList();
                default:
                    return programBriefs.OrderBy(x => x.Title).ToList();
            }
        }
    }
}
