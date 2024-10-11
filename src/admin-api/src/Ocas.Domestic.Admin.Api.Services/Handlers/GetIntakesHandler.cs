using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Admin.Enums;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Enums;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetIntakesHandler : IRequestHandler<GetIntakes, IList<IntakeBrief>>
    {
        private readonly ILogger<GetIntakesHandler> _logger;
        private readonly IUserAuthorization _userAuthorization;
        private readonly ILookupsCache _lookupsCache;
        private readonly IDomesticContext _domesticContext;
        private readonly string _locale;
        private readonly IApiMapper _apiMapper;

        public GetIntakesHandler(ILogger<GetIntakesHandler> logger, IUserAuthorization userAuthorization, ILookupsCache lookupsCache, IDomesticContext domesticContext, RequestCache requestCache, IApiMapper apiMapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
        }

        public async Task<IList<IntakeBrief>> Handle(GetIntakes request, CancellationToken cancellationToken)
        {
            if (request.CollegeId.HasValue)
                await _userAuthorization.CanAccessCollegeAsync(request.User, request.CollegeId.Value);

            var options = new Domestic.Models.GetProgramIntakeOptions
            {
                ApplicationCycleId = request.ApplicationCycleId,
                CollegeId = request.CollegeId,
                CampusId = request.Options?.CampusId,
                ProgramCode = request.Options?.ProgramCode,
                ProgramTitle = request.Options?.ProgramTitle,
                ProgramDeliveryId = request.Options?.DeliveryId,
                FromDate = request.Options?.StartDate
            };

            var results = await _domesticContext.GetProgramIntakes(options);

            var studyMethods = await _lookupsCache.GetStudyMethods(_locale);
            var colleges = await _lookupsCache.GetColleges(_locale);
            var campuses = await _lookupsCache.GetCampuses();
            var intakeStatuses = await _lookupsCache.GetIntakeStatuses(_locale);
            var intakeAvailabilities = await _lookupsCache.GetIntakeAvailabilities(_locale);

            return Sort(_apiMapper.MapProgramIntakeBriefs(results, studyMethods, colleges, campuses, intakeStatuses, intakeAvailabilities, request.Options.Props), request.Options).ToList();
        }

        private static IEnumerable<IntakeBrief> Sort(IList<IntakeBrief> intakes, GetIntakesOptions options)
        {
            switch (options.SortBy)
            {
                case IntakeSortField.Code:
                    return options.SortDirection == SortDirection.Ascending
                        ? intakes.OrderBy(x => x.ProgramCode)
                        : intakes.OrderByDescending(x => x.ProgramCode);
                case IntakeSortField.Title:
                    return options.SortDirection == SortDirection.Ascending
                        ? intakes.OrderBy(x => x.ProgramTitle)
                        : intakes.OrderByDescending(x => x.ProgramTitle);
                case IntakeSortField.Campus:
                    return options.SortDirection == SortDirection.Ascending
                        ? intakes.OrderBy(x => x.CampusName)
                        : intakes.OrderByDescending(x => x.CampusName);
                case IntakeSortField.Delivery:
                    return options.SortDirection == SortDirection.Ascending
                        ? intakes.OrderBy(x => x.Delivery)
                        : intakes.OrderByDescending(x => x.Delivery);
                case IntakeSortField.Availability:
                    return options.SortDirection == SortDirection.Ascending
                        ? intakes.OrderBy(x => x.IntakeAvailability)
                        : intakes.OrderByDescending(x => x.IntakeAvailability);
                case IntakeSortField.StartDate:
                    return options.SortDirection == SortDirection.Ascending
                        ? intakes.OrderBy(x => x.StartDate)
                        : intakes.OrderByDescending(x => x.StartDate);
                case IntakeSortField.Status:
                    return options.SortDirection == SortDirection.Ascending
                        ? intakes.OrderBy(x => x.IntakeStatus)
                        : intakes.OrderByDescending(x => x.IntakeStatus);
                default:
                    return intakes.OrderBy(x => x.ProgramTitle).ToList();
            }
        }
    }
}
