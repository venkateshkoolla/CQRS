using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Admin.Enums;
using Ocas.Domestic.Apply.Admin.Api.Services.Extensions;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Enums;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetIntakeApplicantsHandler : IRequestHandler<GetIntakeApplicants, PagedResult<IntakeApplicant>>
    {
        private readonly ILogger<GetIntakeApplicantsHandler> _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;
        private readonly ILookupsCache _lookupsCache;
        private readonly IApiMapper _apiMapper;

        public GetIntakeApplicantsHandler(ILogger<GetIntakeApplicantsHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization, ILookupsCache lookupsCache, IApiMapper apiMapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
        }

        public async Task<PagedResult<IntakeApplicant>> Handle(GetIntakeApplicants request, CancellationToken cancellationToken)
        {
            var dtoIntake = await _domesticContext.GetProgramIntake(request.IntakeId)
                ?? throw new NotFoundException($"Intake {request.IntakeId} not found.");
            await _userAuthorization.CanAccessCollegeAsync(request.User, dtoIntake.CollegeId);

            var applicationStatuses = await _lookupsCache.GetApplicationStatuses(Constants.Localization.FallbackLocalization);
            var applicationStatusId = applicationStatuses.FirstOrDefault(a => a.Code == Constants.ApplicationStatuses.Active)?.Id
                ?? throw new NotFoundException("Application status of active not found.");

            var options = new Dto.GetProgramApplicationsOptions { IntakeId = request.IntakeId, ApplicationStatusId = applicationStatusId };
            var intakeApplications = _apiMapper.MapIntakeApplicants(await _domesticContext.GetProgramApplications(options));

            (var skipRows, var takeRows) = request.Params.CalculateSkipTakeRows();
            return new PagedResult<IntakeApplicant>
            {
                TotalCount = intakeApplications.Count,
                Items = Sort(intakeApplications, request.Params)
                    .Skip(skipRows)
                    .Take(takeRows)
                    .ToList()
            };
        }

        private IEnumerable<IntakeApplicant> Sort(IEnumerable<IntakeApplicant> paidApplications, GetIntakeApplicantOptions options)
        {
            switch (options.SortBy)
            {
                case IntakeApplicantSortField.FirstName:
                    return options.SortDirection == SortDirection.Ascending
                        ? paidApplications.OrderBy(x => x.FirstName)
                        : paidApplications.OrderByDescending(x => x.FirstName);
                case IntakeApplicantSortField.LastName:
                    return options.SortDirection == SortDirection.Ascending
                        ? paidApplications.OrderBy(x => x.LastName)
                        : paidApplications.OrderByDescending(x => x.LastName);
                case IntakeApplicantSortField.Number:
                    return options.SortDirection == SortDirection.Ascending
                        ? paidApplications.OrderBy(x => x.Number)
                        : paidApplications.OrderByDescending(x => x.Number);
                default:
                    return paidApplications.OrderBy(x => x.Number);
            }
        }
    }
}
