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
    public class DeclineAllOffersHandler : IRequestHandler<DeclineAllOffers, IList<Offer>>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;
        private readonly ILookupsCache _lookupsCache;
        private readonly IApiMapper _apiMapper;
        private readonly IAppSettingsExtras _appSettingsExtras;

        public DeclineAllOffersHandler(ILogger<DeclineAllOffersHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization, ILookupsCache lookupsCache, IApiMapper apiMapper, IAppSettingsExtras appSettingExtras)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _appSettingsExtras = appSettingExtras ?? throw new ArgumentNullException(nameof(appSettingExtras));
        }

        public async Task<IList<Offer>> Handle(DeclineAllOffers request, CancellationToken cancellationToken)
        {
            // get lookup ids
            var applicationStatuses = await _lookupsCache.GetApplicationStatuses(Constants.Localization.EnglishCanada);
            var paidStatusId = applicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.Active).Id;

            var offerStates = await _lookupsCache.GetOfferStates(Constants.Localization.EnglishCanada);
            var activeStateId = offerStates.First(x => x.Code == Constants.Offers.State.Active).Id;

            var offerStatuses = await _lookupsCache.GetOfferStatuses(Constants.Localization.EnglishCanada);
            var noDecisionId = offerStatuses.First(x => x.Code == Constants.Offers.Status.NoDecision).Id;
            var acceptedId = offerStatuses.First(x => x.Code == Constants.Offers.Status.Accepted).Id;

            // get offers if application is paid
            var offersDto = await _domesticContext.GetOffers(new Dto.GetOfferOptions
            {
                ApplicationId = request.ApplicationId,
                ApplicationStatusId = paidStatusId
            });

            if (!offersDto.Any())
                throw new NotFoundException("No offers found");

            var applicantId = offersDto.First().ApplicantId;

            await _userAuthorization.CanAccessApplicantAsync(request.User, applicantId);

            var offers = _apiMapper.MapOffers(offersDto, offerStates, applicationStatuses, _appSettingsExtras);

            var modifiedBy = request.User.GetUpnOrEmail();
            var estToday = DateTime.UtcNow.ToDateInEstAsUtc();

            // filter out already declined offers
            var filteredOffersDecline = offers.Where(o =>
                !o.OfferStatusId.IsEmpty()
                && o.OfferStateId == activeStateId
                && ((o.OfferStatusId == noDecisionId && estToday <= o.HardExpiryDate.ToDateTime()) // you can make a decision an offer up until 23:59:59 EST on the HardExpiryDate
                 || o.OfferStatusId == acceptedId))
                .ToList();

            // don't decline accepted offer
            if (request.IncludeAccepted != true)
            {
                filteredOffersDecline = filteredOffersDecline.Where(o => o.OfferStatusId != acceptedId).ToList();
            }

            if (!filteredOffersDecline.Any())
                throw new ValidationException("No offers to decline");

            if (filteredOffersDecline.Any(x => DateTime.UtcNow < x.OfferLockReleaseDate))
                throw new ValidationException("Lock-out time has not ended");

            await _domesticContext.BeginTransaction();

            try
            {
                foreach (var offer in filteredOffersDecline)
                {
                    await _domesticContext.DeclineOffer(offer.Id, modifiedBy);
                }

                await _domesticContext.CommitTransaction();
            }
            catch (Exception e)
            {
                await _domesticContext.RollbackTransaction(e);

                throw;
            }

            //trigger the declined email
            try
            {
                await _domesticContext.TriggerDeclineEmail(request.ApplicationId, modifiedBy);
            }
            catch (Exception e)
            {
                // don't throw if email fails to trigger
                _logger.LogCritical(e, $"Failed to trigger Offer Decline email for applicationId: {request.ApplicationId}");
            }

            var newOffers = await _domesticContext.GetOffers(new Dto.GetOfferOptions
            {
                ApplicantId = applicantId,
                ApplicationStatusId = paidStatusId
            });

            return _apiMapper.MapOffers(newOffers, offerStates, applicationStatuses, _appSettingsExtras);
        }
    }
}
