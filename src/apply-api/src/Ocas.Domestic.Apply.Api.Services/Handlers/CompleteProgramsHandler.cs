using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class CompleteProgramsHandler : IRequestHandler<CompletePrograms>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;

        public CompleteProgramsHandler(ILogger<CompleteProgramsHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
        }

        public async Task<Unit> Handle(CompletePrograms request, CancellationToken cancellationToken)
        {
            var application = await _domesticContext.GetApplication(request.ApplicationId) ??
                throw new NotFoundException($"Application {request.ApplicationId} not found");

            await _userAuthorization.CanAccessApplicantAsync(request.User, application.ApplicantId);

            if (application.CompletedSteps.HasValue && application.CompletedSteps.Value >= (int)ApplicationCompletedSteps.ProgramChoice)
            {
                return Unit.Value;
            }

            var hasChoices = await _domesticContext.HasProgramChoices(request.ApplicationId);

            if (!hasChoices)
                throw new ValidationException("Must have program choices");

            application.CompletedSteps = (int)ApplicationCompletedSteps.ProgramChoice;
            application.ModifiedBy = request.User.GetUpnOrEmail();

            await _domesticContext.UpdateApplication(application);

            return Unit.Value;
        }
    }
}
