using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class AcceptPrivacyStatementHandler : AsyncRequestHandler<AcceptPrivacyStatement>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;

        public AcceptPrivacyStatementHandler(ILogger<AcceptPrivacyStatementHandler> logger, IDomesticContext domesticContext, IApiMapper apiMapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
        }

        protected override async Task Handle(AcceptPrivacyStatement request, CancellationToken cancellationToken)
        {
            var contact = await _domesticContext.GetContact(request.ApplicantId) ??
                throw new NotFoundException($"Applicant {request.ApplicantId} not found");

            if (request.ApplicantId != contact.Id || contact.ContactType != ContactType.Applicant)
                throw new UnauthorizedAccessException();

            var privacyStatement = await _domesticContext.GetPrivacyStatement(request.PrivacyStatementId, Locale.English);

            if (privacyStatement is null)
                throw new NotFoundException($"PrivacyStatement {request.PrivacyStatementId} not found");

            contact.AcceptedPrivacyStatementId = request.PrivacyStatementId;
            contact.ModifiedBy = request.User.GetUpnOrEmail();

            await _domesticContext.BeginTransaction();

            try
            {
                await _domesticContext.UpdateContact(contact);

                await _domesticContext.AddAcceptedPrivacyStatement(contact, privacyStatement, DateTime.UtcNow.ToDateInEstAsUtc());

                await _domesticContext.CommitTransaction();
            }
            catch (Exception e)
            {
                await _domesticContext.RollbackTransaction(e);

                throw;
            }
        }
    }
}
