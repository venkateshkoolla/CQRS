using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class UpdateApplicantMessageHandler : IRequestHandler<UpdateApplicantMessage>
    {
        private readonly ILogger<UpdateApplicantMessageHandler> _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;

        public UpdateApplicantMessageHandler(ILogger<UpdateApplicantMessageHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
        }

        public async Task<Unit> Handle(UpdateApplicantMessage request, CancellationToken cancellationToken)
        {
            var message = await _domesticContext.GetApplicantMessage(request.Id, Locale.English)
                        ?? throw new NotFoundException($"Message {request.Id} not found");
            await _userAuthorization.CanAccessApplicantAsync(request.User, message.ApplicantId);

            if (message.HasRead == true) return Unit.Value;

            message.HasRead = true;
            await _domesticContext.UpdateApplicantMessage(message, Locale.English);

            return Unit.Value;
        }
    }
}
