using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<IList<ApplicantMessage>> GetApplicantMessages(GetApplicantMessageOptions options, Locale locale)
        {
            if (options.CreatedOn.HasValue && options.CreatedOn.Value.Kind != DateTimeKind.Utc)
                throw new ArgumentException($"CreatedOn must be DateTimeKind.Utc but received: {options.CreatedOn.Value.Kind}", nameof(options));

            return CrmExtrasProvider.GetApplicantMessages(options, locale);
        }

        public Task<ApplicantMessage> GetApplicantMessage(Guid id, Locale locale)
        {
            return CrmExtrasProvider.GetApplicantMessage(id, locale);
        }

        public async Task<ApplicantMessage> UpdateApplicantMessage(ApplicantMessage applicantMessage, Locale locale)
        {
            var crmEntity = CrmProvider.ApplicantMessages.Single(x => x.Id == applicantMessage.Id);

            CrmMapper.PatchApplicantMessage(applicantMessage, crmEntity);
            await CrmProvider.UpdateEntity(crmEntity);

            return await GetApplicantMessage(applicantMessage.Id, locale);
        }
    }
}
