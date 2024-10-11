using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class CompleteTranscriptsHandler : IRequestHandler<CompleteTranscripts>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;

        public CompleteTranscriptsHandler(ILogger<CompleteTranscriptsHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
        }

        public async Task<Unit> Handle(CompleteTranscripts request, CancellationToken cancellationToken)
        {
            var application = await _domesticContext.GetApplication(request.ApplicationId) ??
                throw new NotFoundException($"Application {request.ApplicationId} not found");

            await _userAuthorization.CanAccessApplicantAsync(request.User, application.ApplicantId);

            if (application.CompletedSteps is null || application.CompletedSteps.Value < (int)ApplicationCompletedSteps.TranscriptRequests)
            {
                application.CompletedSteps = (int)ApplicationCompletedSteps.TranscriptRequests;
                application.ModifiedBy = request.User.GetUpnOrEmail();

                await _domesticContext.UpdateApplication(application);
            }

            return Unit.Value;
        }
    }
}
