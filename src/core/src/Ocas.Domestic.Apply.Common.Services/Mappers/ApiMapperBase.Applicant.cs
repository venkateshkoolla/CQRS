using System;
using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Services.Mappers
{
    public partial class ApiMapperBase
    {
        public Applicant MapApplicant(Dto.Contact dbDto, IList<AboriginalStatus> aboriginalStatuses, IList<LookupItem> titles)
        {
            var model = _mapper.Map<Applicant>(dbDto);

            // Map single AboriginalStatusId to List of Guid
            if (dbDto.AboriginalStatusId.HasValue)
            {
                var aboriginalStatus = aboriginalStatuses.SingleOrDefault(x => x.Id == dbDto.AboriginalStatusId);
                var portalAboriginalStatuses = aboriginalStatuses.Where(x => x.ShowInPortal).ToList();

                var mask = Convert.ToInt32(aboriginalStatus?.ColtraneCode, 2);

                var selectedAboriginalStatuses = new List<Guid>();
                foreach (var portalAboriginalStatus in portalAboriginalStatuses)
                {
                    var maskValue = Convert.ToInt32(portalAboriginalStatus.ColtraneCode, 2);

                    if ((mask & maskValue) != 0)
                    {
                        selectedAboriginalStatuses.Add(portalAboriginalStatus.Id);
                    }
                }

                model.AboriginalStatuses = selectedAboriginalStatuses;
            }

            // Map home phone to empty if matches mobile
            if (!string.IsNullOrWhiteSpace(model.HomePhone) && model.HomePhone == model.MobilePhone)
            {
                model.HomePhone = null;
            }

            // From CBA: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/domesticapi?path=%2Fsrc%2FOCAS.Core.API%2FPlugin.CRM%2FRepository%2FCrmApplicantContactRepository.cs&version=GBmaster&line=204&lineStyle=plain&lineEnd=219&lineStartColumn=13&lineEndColumn=14
            // Clear unknown title type
            if (model.TitleId.HasValue)
            {
                var title = titles.FirstOrDefault(x => x.Id == model.TitleId);

                if (title?.Code == Constants.Titles.Unknown)
                {
                    model.TitleId = null;
                }
            }

            return model;
        }

        public Applicant MapApplicant(Dto.Contact dbDto, IList<AboriginalStatus> aboriginalStatuses, IList<LookupItem> titles, IList<LookupItem> sources, IList<College> colleges, IList<ReferralPartner> referralPartners)
        {
            var model = MapApplicant(dbDto, aboriginalStatuses, titles);
            model.Source = GetApplicantSource(dbDto, sources, colleges, referralPartners);
            return model;
        }

        private string GetApplicantSource(Dto.Contact dbDto, IList<LookupItem> sources, IList<College> colleges, IList<ReferralPartner> referralPartners)
        {
            var source = sources.FirstOrDefault(x => x.Id == dbDto.SourceId);

            if (dbDto.SourcePartnerId.HasValue)
            {
                var sourcePartnerCollege = colleges.FirstOrDefault(x => x.Id == dbDto.SourcePartnerId);
                if (sourcePartnerCollege != null)
                {
                    return source.Label + '-' + sourcePartnerCollege.Name;
                }

                var sourcePartnerReferral = referralPartners.FirstOrDefault(x => x.Id == dbDto.SourcePartnerId);
                if (sourcePartnerReferral != null)
                {
                    return source.Label + '-' + sourcePartnerReferral.Name;
                }
            }

            return source.Label;
        }
    }
}
