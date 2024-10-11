using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Api.Services.Extensions;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetApplicantMessagesHandler : IRequestHandler<GetApplicantMessages, IList<ApplicantMessage>>
    {
        private readonly ILogger<GetApplicantMessagesHandler> _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly string _locale;

        public GetApplicantMessagesHandler(ILogger<GetApplicantMessagesHandler> logger, IDomesticContext domesticContext, IApiMapper apiMapper, RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<IList<ApplicantMessage>> Handle(GetApplicantMessages request, CancellationToken cancellationToken)
        {
            var options = new Dto.GetApplicantMessageOptions
            {
                ApplicantId = request.ApplicantId,
                CreatedOn = request.After
            };

            return _apiMapper.MapApplicantMessages(await _domesticContext.GetApplicantMessages(options, _locale.ToLocaleEnum()));
        }
    }
}
