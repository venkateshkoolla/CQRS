using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.AppSettings.Extras;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetOffersHandler : IRequestHandler<GetOffers, IList<Offer>>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly IAppSettingsExtras _appSettingsExtras;
        private readonly ILookupsCache _lookupsCache;

        public GetOffersHandler(ILogger<GetOffersHandler> logger, IDomesticContext domesticContext, IApiMapper apiMapper, ILookupsCache lookupsCache, IAppSettingsExtras appSettingsExtras)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _appSettingsExtras = appSettingsExtras ?? throw new ArgumentNullException(nameof(appSettingsExtras));
        }

        public async Task<IList<Offer>> Handle(GetOffers request, CancellationToken cancellationToken)
        {
            // get lookup ids
            var applicationStatuses = await _lookupsCache.GetApplicationStatuses(Constants.Localization.EnglishCanada);
            var paidStatusId = applicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.Active).Id;

            var offers = await _domesticContext.GetOffers(new Dto.GetOfferOptions
            {
                ApplicantId = request.ApplicantId,
                ApplicationStatusId = paidStatusId
            });

            return _apiMapper.MapOffers(offers, await _lookupsCache.GetOfferStates(Constants.Localization.EnglishCanada), applicationStatuses, _appSettingsExtras);
        }
    }
}
