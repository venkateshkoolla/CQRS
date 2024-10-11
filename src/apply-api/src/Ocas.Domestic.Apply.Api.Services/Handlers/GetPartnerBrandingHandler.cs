using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetPartnerBrandingHandler : IRequestHandler<GetPartnerBranding, PartnerBranding>
    {
        private readonly ILogger _logger;
        private readonly ILookupsCache _lookupsCache;

        public GetPartnerBrandingHandler(ILogger<GetPartnerBrandingHandler> logger, ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
        }

        public async Task<PartnerBranding> Handle(GetPartnerBranding request, CancellationToken cancellationToken)
        {
            var colleges = await _lookupsCache.GetColleges(Constants.Localization.EnglishCanada);
            var collegeBranding = colleges.FirstOrDefault(x => x.Code == request.Code.ToUpperInvariant());

            if (collegeBranding?.AllowCba == true)
            {
                return new PartnerBranding
                {
                    Partner = collegeBranding.Code,
                    Type = PartnerBrandingType.College
                };
            }

            var referralPatners = await _lookupsCache.GetReferralPartners();
            var partnerBranding = referralPatners.FirstOrDefault(x => x.Code == request.Code.ToUpperInvariant());

            if (partnerBranding?.AllowCba == true)
            {
                return new PartnerBranding
                {
                    Partner = partnerBranding.Code,
                    Type = PartnerBrandingType.Referral
                };
            }

            throw new NotFoundException($"Partner branding for code '{request.Code}' not found.");
        }
    }
}
