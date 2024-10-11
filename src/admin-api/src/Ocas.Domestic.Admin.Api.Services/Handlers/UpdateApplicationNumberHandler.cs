using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class UpdateApplicationNumberHandler : IRequestHandler<UpdateApplicationNumber, Application>
    {
        private readonly ILogger<UpdateApplicationNumberHandler> _logger;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly ILookupsCache _lookupsCache;

        public UpdateApplicationNumberHandler(ILogger<UpdateApplicationNumberHandler> logger, IUserAuthorization userAuthorization, IDomesticContext domesticContext, IApiMapper apiMapper, ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
        }

        public async Task<Application> Handle(UpdateApplicationNumber request, CancellationToken cancellationToken)
        {
            var userType = _userAuthorization.GetUserType(request.User);
            if (userType != UserType.OcasUser) throw new ForbiddenException();

            var formattedNumber = request.Number.Length == 8 ? request.Number.Insert(0, "0") : request.Number;

            if (await _domesticContext.IsDuplicateApplication(request.ApplicationId, formattedNumber))
                throw new ConflictException($"Application with {request.Number} already exists.");

            var application = await _domesticContext.GetApplication(request.ApplicationId)
                ?? throw new NotFoundException($"Application not found: {request.ApplicationId}");

            var applicationCycles = await _lookupsCache.GetApplicationCycles();
            var applicationCycle = applicationCycles.First(x => x.Id == application.ApplicationCycleId);

            if (!request.Number.StartsWith(applicationCycle.Year.Substring(2, 2)))
                throw new ValidationException("Application Number should be between YY0000000 and YY9999999. (YY – Application Cycle Start Year)");

            application.ApplicationNumber = formattedNumber;
            application.ModifiedBy = request.User.GetUpnOrEmail();

            var updatedApplication = await _domesticContext.UpdateApplication(application);

            return _apiMapper.MapApplication(updatedApplication);
        }
    }
}
