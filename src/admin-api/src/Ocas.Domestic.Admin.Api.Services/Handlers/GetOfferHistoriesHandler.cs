using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Extensions;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetOfferHistoriesHandler : IRequestHandler<GetOfferHistories, IList<OfferHistory>>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly IUserAuthorization _userAuthorization;
        private readonly string _locale;

        public GetOfferHistoriesHandler(ILogger<GetOfferHistoriesHandler> logger, IDomesticContext domesticContext, IApiMapper apiMapper, IUserAuthorization userAuthorization, RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<IList<OfferHistory>> Handle(GetOfferHistories request, CancellationToken cancellationToken)
        {
            if (!_userAuthorization.IsOcasUser(request.User))
                throw new ForbiddenException();

            var application = await _domesticContext.GetApplication(request.ApplicationId) ??
                            throw new NotFoundException($"No application found with id: {request.ApplicationId}");

            var offerHistories = await _domesticContext.GetOfferAcceptances(new Dto.GetOfferAcceptancesOptions { ApplicationId = application.Id }, _locale.ToLocaleEnum());

            return _apiMapper.MapOfferHistories(offerHistories);
        }
    }
}
