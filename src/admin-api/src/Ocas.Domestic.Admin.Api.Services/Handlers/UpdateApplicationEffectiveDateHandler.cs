using System;
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
    public class UpdateApplicationEffectiveDateHandler : IRequestHandler<UpdateApplicationEffectiveDate, Application>
    {
        private readonly ILogger<UpdateApplicationEffectiveDateHandler> _logger;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;

        public UpdateApplicationEffectiveDateHandler(ILogger<UpdateApplicationEffectiveDateHandler> logger, IUserAuthorization userAuthorization, IDomesticContext domesticContext, IApiMapper apiMapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
        }

        public async Task<Application> Handle(UpdateApplicationEffectiveDate request, CancellationToken cancellationToken)
        {
            var userType = _userAuthorization.GetUserType(request.User);
            if (userType != UserType.OcasUser) throw new ForbiddenException();

            var application = await _domesticContext.GetApplication(request.ApplicationId)
                ?? throw new NotFoundException($"Application not found: {request.ApplicationId}");

            application.EffectiveDate = request.EffectiveDate.ToDateTime();
            application.ModifiedBy = request.User.GetUpnOrEmail();
            var updatedApplication = await _domesticContext.UpdateApplication(application);

            return _apiMapper.MapApplication(updatedApplication);
        }
    }
}
