using System.Collections.Generic;
using Ocas.Domestic.Apply.Models.Lookups;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public IList<LookupItem> MapLookupItem(IList<Dto.AccountStatus> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.ApplicationCycleStatus> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.ApplicationStatus> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.BasisForAdmission> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.CanadianStatus> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.CommunityInvolvement> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.CourseDelivery> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.CourseStatus> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.CourseType> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.Credential> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.Currency> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.CredentialEvaluationAgency> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.Current> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.EntryLevel> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.FirstGenerationApplicant> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.FirstLanguage> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.Gender> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.GradeType> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.HighestEducation> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.HighSkillsMajor> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.Institute> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.InstituteType> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.InternationalCreditAssessmentStatus> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.LastGradeCompleted> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.LevelAchieved> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.LevelOfStudy> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.LiteracyTest> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.OfferState> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.OfferStatus> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.OfferStudyMethod> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.OfferType> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.Official> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.OstNote> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.PaymentMethod> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.PaymentResult> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.PreferredCorrespondenceMethod> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.PreferredLanguage> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.PreferredSponsorAgency> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.ProgramIntakeAvailability> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.ProgramIntakeStatus> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.Promotion> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.Source> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.StatusOfVisa> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.SupportingDocumentType> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.TestType> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.Title> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }

        public IList<LookupItem> MapLookupItem(IList<Dto.TranscriptRequestStatus> list)
        {
            return _mapper.Map<IList<LookupItem>>(list);
        }
    }
}
