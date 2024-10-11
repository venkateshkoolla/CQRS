using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Services.Extensions;
using Ocas.Domestic.Apply.Services.Mappers;
using Ocas.Domestic.Apply.Services.Messages;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Services.Handlers
{
    public class GetProgramChoicesHandler : IRequestHandler<GetProgramChoices, IList<ProgramChoice>>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly ILookupsCacheBase _lookupsCache;
        private readonly IUserAuthorizationBase _userAuthorization;
        private readonly IApiMapperBase _apiMapper;
        private readonly string _locale;

        public GetProgramChoicesHandler(ILogger<GetProgramChoicesHandler> logger, IDomesticContext domesticContext, ILookupsCacheBase lookupsCache, IUserAuthorizationBase userAuthorization, IApiMapperBase apiMapper, RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<IList<ProgramChoice>> Handle(GetProgramChoices request, CancellationToken cancellationToken)
        {
            // Applicant request
            if (!request.ApplicantId.IsEmpty())
            {
                await _userAuthorization.CanAccessApplicantAsync(request.User, request.ApplicantId.Value, true);
            }

            // Application request
            if (!request.ApplicationId.IsEmpty())
            {
                var application = await _domesticContext.GetApplication(request.ApplicationId.Value) ??
                    throw new NotFoundException($"Application {request.ApplicationId} not found.");
                await _userAuthorization.CanAccessApplicantAsync(request.User, application.ApplicantId, true);
            }

            var choiceOptions = new Dto.GetProgramChoicesOptions
            {
                ApplicationId = request.ApplicationId,
                ApplicantId = request.ApplicantId,
                StateCode = request.IsRemoved ? State.Inactive : State.Active,
                StatusCode = request.IsRemoved ? Status.Inactive : Status.Active
            };
            var choices = await _domesticContext.GetProgramChoices(choiceOptions);
            var filterChoices = choices.Where(c => c.SequenceNumber <= Constants.ProgramChoices.MaxTotalChoices).ToList();

            if (!request.IsRemoved && filterChoices.Any())
            {
                var intakeIds = filterChoices.Select(c => c.ProgramIntakeId).Distinct().ToList();
                var options = new Dto.GetProgramIntakeOptions
                {
                    Ids = intakeIds,
                    StateCode = null,
                    StatusCode = null
                };
                var intakes = await _domesticContext.GetProgramIntakes(options);

                IList<Dto.ShoppingCartDetail> shoppingCartDetails = null;
                if (filterChoices.Any(x => x.SupplementalFeePaid == false))
                {
                    shoppingCartDetails = await _domesticContext.GetShoppingCartDetails(
                        new Dto.GetShoppingCartDetailOptions
                        {
                            ApplicantId = request.ApplicantId,
                            ApplicationId = request.ApplicationId
                        },
                        _locale.ToLocaleEnum());
                }

                return _apiMapper.MapProgramChoices(filterChoices.OrderBy(c => c.SequenceNumber).ToList(), intakes, await _lookupsCache.GetProgramIntakeAvailabilities(_locale), shoppingCartDetails);
            }

            return filterChoices.OrderByDescending(c => c.ModifiedOn).Select(_apiMapper.MapProgramChoice).ToList();
        }
    }
}
