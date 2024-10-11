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
    public class VerifyProfileHandler : IRequestHandler<VerifyProfile>
    {
        private readonly ILogger<VerifyProfileHandler> _logger;
        private readonly IDomesticContext _domesticContext;

        public VerifyProfileHandler(ILogger<VerifyProfileHandler> logger, IDomesticContext domesticContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
        }

        public async Task<Unit> Handle(VerifyProfile request, CancellationToken cancellationToken)
        {
            var contact = await _domesticContext.GetContact(request.ApplicantId) ??
                throw new NotFoundException($"Applicant {request.ApplicantId} not found");

            if (contact.ContactType != ContactType.Applicant)
                throw new ForbiddenException();

            contact.LastLoginExceed = false;
            contact.LastLogin = DateTime.UtcNow;
            contact.ModifiedBy = request.User.GetUpnOrEmail();
            await _domesticContext.UpdateContact(contact);

            return Unit.Value;
        }
    }
}
