using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class RemoveEducationHandler : IRequestHandler<RemoveEducation>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;

        public RemoveEducationHandler(ILogger<RemoveEducationHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
        }

        public async Task<Unit> Handle(RemoveEducation request, CancellationToken cancellationToken)
        {
            var education = await _domesticContext.GetEducation(request.EducationId)
                ?? throw new NotFoundException("Education request not found.");

            if (!education.ApplicantId.HasValue)
            {
                const string applicantLogEx = "Education request does not have an applicant.";
                _logger.LogWarning(applicantLogEx);
                throw new ValidationException(applicantLogEx);
            }

            await _userAuthorization.CanAccessApplicantAsync(request.User, education.ApplicantId.Value);

            if (!education.HasMoreThanOneEducation) throw new ValidationException("Cannot remove the only education record.");
            if (education.HasPaidApplication) throw new ValidationException("Cannot remove education with paid application.");
            if (education.HasTranscripts) throw new ValidationException("Cannot remove education with transcript requests.");

            await _domesticContext.BeginTransaction();
            try
            {
                education.ModifiedBy = request.User.GetUpnOrEmail();
                await _domesticContext.UpdateEducation(education);
                await _domesticContext.DeleteEducation(education.Id);

                await _domesticContext.CommitTransaction();
            }
            catch (Exception e)
            {
                await _domesticContext.RollbackTransaction(e);

                throw;
            }

            return Unit.Value;
        }
    }
}
