using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.AppSettings.Extras;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetNextSendDateHandler : IRequestHandler<GetNextSendDate, DateTime?>
    {
        private readonly ILogger _logger;
        private readonly IAppSettingsExtras _appSettingsExtras;

        public GetNextSendDateHandler(
            ILogger<GetNextSendDateHandler> logger,
            IAppSettingsExtras appSettingsExtras)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettingsExtras = appSettingsExtras ?? throw new ArgumentNullException(nameof(appSettingsExtras));
        }

        public Task<DateTime?> Handle(GetNextSendDate request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_appSettingsExtras.GetNextSendDate(request.UtcNow));
        }
    }
}
