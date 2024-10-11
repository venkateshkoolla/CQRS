using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class DeleteTranscriptRequestHandler : IRequestHandler<DeleteTranscriptRequest>
    {
        private readonly ILogger<DeleteTranscriptRequestHandler> _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;

        public DeleteTranscriptRequestHandler(ILogger<DeleteTranscriptRequestHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
        }

        public async Task<Unit> Handle(DeleteTranscriptRequest request, CancellationToken cancellationToken)
        {
            var transcriptRequest = await _domesticContext.GetTranscriptRequest(request.TranscriptRequestId)
                ?? throw new NotFoundException("Transcript request not found.");

            if (!transcriptRequest.ApplicantId.HasValue)
            {
                const string applicantLogEx = "Transcript request does not have an applicant.";
                _logger.LogWarning(applicantLogEx);
                throw new ValidationException(applicantLogEx);
            }

            await _userAuthorization.CanAccessApplicantAsync(request.User, transcriptRequest.ApplicantId.Value);

            if (transcriptRequest.PeteRequestLogId.HasValue)
            {
                const string requestLogEx = "Transcript request cannot have a request log.";
                _logger.LogWarning(requestLogEx);
                throw new ValidationException(requestLogEx);
            }

            await _domesticContext.DeleteTranscriptRequest(transcriptRequest.Id);

            return Unit.Value;
        }
    }
}
