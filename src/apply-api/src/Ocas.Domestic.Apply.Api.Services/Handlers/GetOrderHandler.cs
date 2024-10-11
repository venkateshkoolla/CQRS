using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Order = Ocas.Domestic.Apply.Models.Order;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetOrderHandler : IRequestHandler<GetOrder, Order>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IApiMapper _apiMapper;

        public GetOrderHandler(ILogger<GetOrderHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization, IApiMapper apiMapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
        }

        public async Task<Order> Handle(GetOrder request, CancellationToken cancellationToken)
        {
            var order = await _domesticContext.GetOrder(request.OrderId) ?? throw new NotFoundException($"Order {request.OrderId} not found");

            if (order.ApplicantId is null)
                throw new ValidationException($"Order {request.OrderId} has no ApplicantId");

            await _userAuthorization.CanAccessApplicantAsync(request.User, order.ApplicantId.Value);

            var financialTransactions =
                await _domesticContext.GetFinancialTransactions(new GetFinancialTransactionOptions
                {
                    OrderId = order.Id
                });

            var financialTransaction = financialTransactions.FirstOrDefault(x => x.TransactionType == TransactionType.Payment);

            return _apiMapper.MapOrder(order, financialTransaction);
        }
    }
}
