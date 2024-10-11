using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetFinancialTransactionsHandler : IRequestHandler<GetFinancialTransactions, IList<FinancialTransaction>>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IApiMapper _apiMapper;

        public GetFinancialTransactionsHandler(ILogger<GetFinancialTransactionsHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization, IApiMapper apiMapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
        }

        public async Task<IList<FinancialTransaction>> Handle(GetFinancialTransactions request, CancellationToken cancellationToken)
        {
            var application = await _domesticContext.GetApplication(request.ApplicationId) ?? throw new NotFoundException($"Application {request.ApplicationId} not found");

            await _userAuthorization.CanAccessApplicantAsync(request.User, application.ApplicantId);

            var financialTransactions = await _domesticContext.GetFinancialTransactions(new Dto.GetFinancialTransactionOptions
            {
                ApplicantId = application.ApplicantId
            });

            var result = financialTransactions
                .Where(x => x.ApplicationId is null || x.ApplicationId == request.ApplicationId)
                .Select(_apiMapper.MapFinancialTransaction)
                .ToList();

            var transfers = financialTransactions
                .Where(x => x.TransactionType == TransactionType.Transfer && x.OrderApplicationId == request.ApplicationId && result.All(y => y.Id != x.Id))
                .Select(x =>
                {
                    var model = _apiMapper.MapFinancialTransaction(x);
                    model.ApplicationId = model.OrderApplicationId;
                    return model;
                })
                .ToList();

            result.AddRange(transfers);

            return result;
        }
    }
}
