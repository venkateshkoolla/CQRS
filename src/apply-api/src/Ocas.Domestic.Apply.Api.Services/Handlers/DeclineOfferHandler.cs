using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.AppSettings.Extras;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class DeclineOfferHandler : IRequestHandler<DeclineOffer, IList<Offer>>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;
        private readonly ILookupsCache _lookupsCache;
        private readonly IApiMapper _apiMapper;
        private readonly IAppSettingsExtras _appSettingsExtras;

        public DeclineOfferHandler(ILogger<DeclineOfferHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization, ILookupsCache lookupsCache, IApiMapper apiMapper, IAppSettingsExtras appSettingExtras)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _appSettingsExtras = appSettingExtras ?? throw new ArgumentNullException(nameof(appSettingExtras));
        }

        public async Task<IList<Offer>> Handle(DeclineOffer request, CancellationToken cancellationToken)
        {
            // get lookup ids
            var applicationStatuses = await _lookupsCache.GetApplicationStatuses(Constants.Localization.EnglishCanada);
            var paidStatusId = applicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.Active).Id;

            var offerStates = await _lookupsCache.GetOfferStates(Constants.Localization.EnglishCanada);
            var activeStateId = offerStates.First(x => x.Code == Constants.Offers.State.Active).Id;

            var offerStatuses = await _lookupsCache.GetOfferStatuses(Constants.Localization.EnglishCanada);
            var acceptedId = offerStatuses.First(x => x.Code == Constants.Offers.Status.Accepted).Id;
            var declinedId = offerStatuses.First(s => s.Code == Constants.Offers.Status.Declined).Id;

            // get offer
            var offerDto = await _domesticContext.GetOffer(request.OfferId)
                        ?? throw new NotFoundException($"OfferId {request.OfferId} not found");

            await _userAuthorization.CanAccessApplicantAsync(request.User, offerDto.ApplicantId);

            if (offerDto.ApplicationStatusId != paidStatusId)
                throw new ValidationException($"ApplicationStatusId is not paid: {offerDto.ApplicationStatusId}");

            if (_userAuthorization.IsOcasUser(request.User))
            {
                if (offerDto.OfferStatusId == declinedId)
                    throw new ValidationException("Offer must be not already be declined.");
            }
            else if (offerDto.OfferStatusId != acceptedId)
            {
                throw new ValidationException($"Offer must be in Accepted status: {offerDto.OfferStatusId}");
            }

            if (offerDto.OfferStateId != activeStateId)
                throw new ValidationException($"Offer must be in Active state: {offerDto.OfferStateId}");

            var offersDto = await _domesticContext.GetOffers(new Dto.GetOfferOptions
            {
                ApplicantId = offerDto.ApplicantId,
                ApplicationStatusId = paidStatusId
            });

            var offers = _apiMapper.MapOffers(offersDto, offerStates, applicationStatuses, _appSettingsExtras);

            var offer = offers.First(x => x.Id == offerDto.Id);

            if (DateTime.UtcNow < offer.OfferLockReleaseDate)
                throw new ValidationException("Lock-out time has not ended");

            var modifiedBy = request.User.GetUpnOrEmail();

            await _domesticContext.DeclineOffer(offer.Id, modifiedBy);

            //trigger the declined email
            try
            {
                await _domesticContext.TriggerDeclineEmail(offerDto.ApplicationId, modifiedBy);
            }
            catch (Exception e)
            {
                // don't throw if email fails to trigger
                _logger.LogCritical(e, $"Failed to trigger Offer Decline email for applicationId: {offerDto.ApplicationId}");
            }

            var newOffers = await _domesticContext.GetOffers(new Dto.GetOfferOptions
            {
                ApplicantId = offerDto.ApplicantId,
                ApplicationStatusId = paidStatusId
            });

            return _apiMapper.MapOffers(newOffers, offerStates, applicationStatuses, _appSettingsExtras);
        }
    }
}
