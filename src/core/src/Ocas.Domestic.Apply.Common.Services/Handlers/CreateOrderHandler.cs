using System;
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
using Ocas.Domestic.Data.Extras;

namespace Ocas.Domestic.Apply.Services.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrder, Order>
    {
        private readonly ILogger _logger;
        private readonly ILookupsCacheBase _lookupsCache;
        private readonly string _sourcePartner;
        private readonly IDomesticContext _domesticContext;
        private readonly IDomesticContextExtras _domesticContextExtras;
        private readonly IUserAuthorizationBase _userAuthorization;
        private readonly IApiMapperBase _apiMapper;

        public CreateOrderHandler(ILogger<CreateOrderHandler> logger, IDomesticContext domesticContext, IUserAuthorizationBase userAuthorization, IApiMapperBase apiMapper, ILookupsCacheBase lookupsCache, RequestCache requestCache, IDomesticContextExtras domesticContextExtras)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _sourcePartner = requestCache.Get<string>(Constants.RequestCacheKeys.Partner);
            _domesticContextExtras = domesticContextExtras ?? throw new ArgumentNullException(nameof(domesticContextExtras));
        }

        public async Task<Order> Handle(CreateOrder request, CancellationToken cancellationToken)
        {
            var application = await _domesticContext.GetApplication(request.ApplicationId)
                 ?? throw new NotFoundException($"Application {request.ApplicationId} not found");

            await _userAuthorization.CanAccessApplicantAsync(request.User, application.ApplicantId);

            var sourceId = await _lookupsCache.GetSourceId(_sourcePartner);
            var modifiedBy = request.User.GetUpnOrEmail();
            var order = await _domesticContextExtras.CreateOrder(request.ApplicationId, application.ApplicantId, modifiedBy, sourceId, request.IsOfflinePayment);

            if (order is null)
            {
                _logger.LogCritical($"No orders created for application: {request.ApplicationId}");
                throw new ValidationException("No orders created");
            }

            return _apiMapper.MapOrder(order);
        }
    }
}
