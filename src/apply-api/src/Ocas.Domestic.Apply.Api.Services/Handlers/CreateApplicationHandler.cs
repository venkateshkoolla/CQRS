using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class CreateApplicationHandler : IRequestHandler<CreateApplication, Application>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly ILookupsCache _lookupsCache;

        public CreateApplicationHandler(ILogger<CreateApplicationHandler> logger, IDomesticContext domesticContext, IApiMapper apiMapper, ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
        }

        public async Task<Application> Handle(CreateApplication request, CancellationToken cancellationToken)
        {
            var appStatuses = await _lookupsCache.GetApplicationStatuses(Constants.Localization.EnglishCanada);
            var appStatus = appStatuses.FirstOrDefault(s => s.Code == Constants.ApplicationStatuses.NewApply) ?? throw new NotFoundException("Application Statuses does not exist.");

            var currentApplications = await _domesticContext.GetApplications(request.ApplicantId);
            if (currentApplications.Any(a => a.ApplicationCycleId == request.ApplicationCycleId))
            {
                return _apiMapper.MapApplication(currentApplications.First(a => a.ApplicationCycleId == request.ApplicationCycleId));
            }

            var applicationBase = new Dto.ApplicationBase
            {
                ModifiedBy = request.User.GetUpnOrEmail(),
                EffectiveDate = DateTime.UtcNow,
                ApplicantId = request.ApplicantId,
                ApplicationCycleId = request.ApplicationCycleId,
                ApplicationStatusId = appStatus.Id
            };

            var application = await _domesticContext.CreateApplication(applicationBase);
            return _apiMapper.MapApplication(application);
        }
    }
}
