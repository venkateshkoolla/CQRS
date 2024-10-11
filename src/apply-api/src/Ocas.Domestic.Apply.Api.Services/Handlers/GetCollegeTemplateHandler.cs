using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetCollegeTemplateHandler : IRequestHandler<GetCollegeTemplate, CollegeTemplate>
    {
        private readonly ILogger _logger;
        private readonly ILookupsCache _lookupsCache;
        private readonly string _locale;

        public GetCollegeTemplateHandler(ILogger<GetCollegeTemplateHandler> logger, ILookupsCache lookupsCache, RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<CollegeTemplate> Handle(GetCollegeTemplate request, CancellationToken cancellationToken)
        {
            var collegeInformations = await _lookupsCache.GetCollegeInformation(_locale);
            var collegeInformation = collegeInformations.FirstOrDefault(x => x.CollegeId == request.CollegeId) ??
                throw new NotFoundException($"College information missing for: {request.CollegeId}");

            var collegeTemplate = new CollegeTemplate { Key = request.Key };
            switch (request.Key)
            {
                case CollegeTemplateKey.OfferAccepted:
                    collegeTemplate.Content = HttpUtility.HtmlDecode(collegeInformation.WelcomeText);
                    break;
            }

            return collegeTemplate;
        }
    }
}
