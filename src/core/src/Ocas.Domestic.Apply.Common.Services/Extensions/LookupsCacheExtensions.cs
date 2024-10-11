using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ocas.Domestic.Apply.Services.Extensions
{
    public static class LookupsCacheExtensions
    {
        public static async Task<Guid> GetSourceId(this ILookupsCacheBase lookupsCache, string partnerCode)
        {
            var sources = await lookupsCache.GetSources(Constants.Localization.FallbackLocalization);

            if (!string.IsNullOrEmpty(partnerCode))
            {
                var sourcePartnerColleges = await lookupsCache.GetColleges(Constants.Localization.FallbackLocalization);
                var sourcePartnerCollege = sourcePartnerColleges.FirstOrDefault(c => c.Code == partnerCode && c.AllowCba);
                if (sourcePartnerCollege != null)
                {
                    if (sourcePartnerCollege.AllowCbaReferralCodeAsSource)
                    {
                        var sourceCode = Constants.Sources.CBUI + sourcePartnerCollege.Code;
                        return sources.FirstOrDefault(s => s.Code == sourceCode)?.Id ?? sources.First(s => s.Code == Constants.Sources.CBUIUNKNOWN).Id;
                    }

                    return sources.First(x => x.Code == Constants.Sources.CBUI).Id;
                }

                var sourcePartnerReferrals = await lookupsCache.GetReferralPartners();
                var sourcePartnerReferral = sourcePartnerReferrals.FirstOrDefault(p => p.Code == partnerCode && p.AllowCba);
                if (sourcePartnerReferral != null)
                {
                    if (sourcePartnerReferral.AllowCbaReferralCodeAsSource)
                    {
                        var sourceCode = Constants.Sources.CBUI + sourcePartnerReferral.Code;
                        return sources.FirstOrDefault(s => s.Code == sourceCode)?.Id ?? sources.First(s => s.Code == Constants.Sources.CBUIUNKNOWN).Id;
                    }

                    return sources.First(x => x.Code == Constants.Sources.CBUI).Id;
                }
            }

            return sources.First(x => x.Code == Constants.Sources.A2C2).Id;
        }
    }
}
