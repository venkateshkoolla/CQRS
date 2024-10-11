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
    public class AcceptOfferHandler : IRequestHandler<AcceptOffer, IList<Offer>>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;
        private readonly ILookupsCache _lookupsCache;
        private readonly IApiMapper _apiMapper;
        private readonly IAppSettingsExtras _appSettingsExtras;

        public AcceptOfferHandler(ILogger<AcceptOfferHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization, ILookupsCache lookupsCache, IApiMapper apiMapper, IAppSettingsExtras appSettingsExtras)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _appSettingsExtras = appSettingsExtras ?? throw new ArgumentNullException(nameof(appSettingsExtras));
        }

        public async Task<IList<Offer>> Handle(AcceptOffer request, CancellationToken cancellationToken)
        {
            // get lookup ids
            var applicationStatuses = await _lookupsCache.GetApplicationStatuses(Constants.Localization.EnglishCanada);
            var paidStatusId = applicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.Active).Id;

            var offerStates = await _lookupsCache.GetOfferStates(Constants.Localization.EnglishCanada);
            var activeStateId = offerStates.First(x => x.Code == Constants.Offers.State.Active).Id;

            var offerStatuses = await _lookupsCache.GetOfferStatuses(Constants.Localization.EnglishCanada);
            var acceptedId = offerStatuses.First(x => x.Code == Constants.Offers.Status.Accepted).Id;

            // get offer
            var offerDto = await _domesticContext.GetOffer(request.OfferId)
                        ?? throw new NotFoundException($"OfferId {request.OfferId} not found");

            await _userAuthorization.CanAccessApplicantAsync(request.User, offerDto.ApplicantId);

            if (offerDto.ApplicationStatusId != paidStatusId)
                throw new ValidationException($"ApplicationStatusId is not paid: {offerDto.ApplicationStatusId}");

            if (offerDto.OfferStatusId == acceptedId)
                throw new ValidationException("Offer is already accepted");

            if (offerDto.OfferStateId != activeStateId)
                throw new ValidationException($"Offer must be in Active state: {offerDto.OfferStateId}");

            var offersDto = await _domesticContext.GetOffers(new Dto.GetOfferOptions
            {
                ApplicantId = offerDto.ApplicantId,
                ApplicationStatusId = paidStatusId
            });

            var offers = _apiMapper.MapOffers(offersDto, offerStates, applicationStatuses, _appSettingsExtras);

            var offer = offers.First(x => x.Id == offerDto.Id);

            if (!offer.CanRespond)
                throw new ValidationException("CanRespond == false");

            // from A2C: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Services%2FOffer%2FOfferService.cs&version=GBmaster&line=338&lineStyle=plain&lineEnd=348&lineStartColumn=1&lineEndColumn=1
            if (offerDto.IntakeId.IsEmpty() || offerDto.ProgramId.IsEmpty() || offerDto.CollegeApplicationCycleId.IsEmpty())
            {
                _logger.LogCritical("Program related details of the Offer is missing."
                                            + $" Application #: {offerDto.ApplicationNumber}"
                                            + $" Offer: {offerDto.Id}"
                                            + $" Intake: {offerDto.IntakeId}"
                                            + $" Program: {offerDto.ProgramId}"
                                            + $" CollegeApplicationCycle: {offerDto.CollegeApplicationCycleId}"
                                            + $" ApplicationCycle: {offerDto.ApplicationCycleId}");
                throw new ValidationException("Program related details of the Offer is missing");
            }

            await _domesticContext.AcceptOffer(offer.Id, request.User.GetUpnOrEmail());

            var newOffers = await _domesticContext.GetOffers(new Dto.GetOfferOptions
            {
                ApplicantId = offerDto.ApplicantId,
                ApplicationStatusId = paidStatusId
            });

            return _apiMapper.MapOffers(newOffers, offerStates, applicationStatuses, _appSettingsExtras);
        }
    }
}
