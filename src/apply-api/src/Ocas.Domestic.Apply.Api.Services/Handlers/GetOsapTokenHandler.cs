using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Api.Services.Clients;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetOsapTokenHandler : IRequestHandler<GetOsapToken, OsapToken>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IOsapClient _osapClient;

        public GetOsapTokenHandler(ILogger<GetOsapTokenHandler> logger, IDomesticContext domesticContext, IOsapClient osapClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _osapClient = osapClient ?? throw new ArgumentNullException(nameof(osapClient));
        }

        public async Task<OsapToken> Handle(GetOsapToken request, CancellationToken cancellationToken)
        {
            var applicant = await _domesticContext.GetContact(request.ApplicantId);
            var tokenResponse = await _osapClient.GetApplicantToken(applicant.AccountNumber, applicant.BirthDate);

            return new OsapToken
            {
                AccessToken = tokenResponse.AccessToken,
                ExpiresIn = tokenResponse.ExpiresIn,
                RefreshToken = tokenResponse.RefreshToken
            };
        }
    }
}
