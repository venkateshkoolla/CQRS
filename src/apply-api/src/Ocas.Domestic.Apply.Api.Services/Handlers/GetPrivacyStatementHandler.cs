using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Api.Services.Extensions;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetPrivacyStatementHandler : IRequestHandler<GetPrivacyStatement, PrivacyStatement>
    {
        private readonly IMapper _mapper;
        private readonly string _locale;
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;

        public GetPrivacyStatementHandler(
            ILogger<GetPrivacyStatement> logger,
            IDomesticContext domesticContext,
            IMapper mapper,
            RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<PrivacyStatement> Handle(GetPrivacyStatement request, CancellationToken cancellationToken)
        {
            var privacyStatement = await _domesticContext.GetPrivacyStatement(request.Id, _locale.ToLocaleEnum()) ??
                                   throw new Common.Exceptions.NotFoundException($"PrivacyStatement {request.Id} not found");

            return _mapper.Map<PrivacyStatement>(privacyStatement);
        }
    }
}
