using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.Apply.TestFramework.RuleCollections;
using Ocas.Domestic.Data.TestFramework;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.TestFramework
{
    public class ModelFakerFixture
    {
        public SeedDataFixture SeedDataFixture { get; } = new SeedDataFixture();
        private readonly AllLookups _lookups;

        public ModelFakerFixture()
        {
            _lookups = AllApplyLookups;
        }

        public AllLookups AllApplyLookups
        {
            get
            {
                var aboriginalStatuses = SeedDataFixture.AboriginalStatuses.Select(dto =>
                    new AboriginalStatus
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName,
                        ColtraneCode = dto.ColtraneCode,
                        ShowInPortal = dto.ShowInPortal
                    }).ToList() as IList<AboriginalStatus>;

                var accountStatuses = SeedDataFixture.AccountStatuses.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var achievementLevels = SeedDataFixture.LevelAchieveds.Select(dto =>
                   new LookupItem
                   {
                       Id = dto.Id,
                       Code = dto.Code,
                       Label = dto.LocalizedName
                   }).ToList() as IList<LookupItem>;

                var applicationCycles = SeedDataFixture.ApplicationCycles.Select(dto =>
                    new ApplicationCycle
                    {
                        Id = dto.Id,
                        EndDate = dto.EndDate.ToStringOrDefault(),
                        StartDate = dto.StartDate.ToStringOrDefault(),
                        Year = dto.Year,
                        Status = SeedDataFixture.ApplicationCycleStatuses.FirstOrDefault(s => s.Id == dto.StatusId)?.Code
                    }).ToList() as IList<ApplicationCycle>;

                var applicationCycleStatuses = SeedDataFixture.ApplicationCycleStatuses.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var applicationStatuses = SeedDataFixture.ApplicationStatuses.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var basisForAdmissions = SeedDataFixture.BasisForAdmissions.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var campuses = SeedDataFixture.Campuses.Select(dto =>
                new Campus
                {
                    Id = dto.Id,
                    Code = dto.Code,
                    Name = dto.Name,
                    CollegeId = dto.ParentId
                }).ToList() as IList<Campus>;

                var canadianStatuses = SeedDataFixture.CanadianStatuses.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var cities = SeedDataFixture.Cities.Select(dto =>
                    new City
                    {
                        Id = dto.Id,
                        Label = dto.LocalizedName,
                        ProvinceId = dto.ProvinceId,
                        Name = dto.Name
                    }).ToList() as IList<City>;

                var collegeApplicationCycles = SeedDataFixture.CollegeApplicationCycles.Select(dto =>
                {
                    var applicationCycle = SeedDataFixture.ApplicationCycles.FirstOrDefault(a => a.Id == dto.ApplicationCycleId);
                    return new CollegeApplicationCycle
                    {
                        Id = dto.Id,
                        MasterId = dto.ApplicationCycleId,
                        Year = applicationCycle?.Year,
                        CollegeId = dto.CollegeId,
                        Name = dto.Name,
                        StartDate = applicationCycle?.StartDate.ToStringOrDefault(),
                        EndDate = applicationCycle?.EndDate.ToStringOrDefault(),
                        StatusId = applicationCycle?.StatusId ?? Guid.Empty
                    };
                }).ToList() as IList<CollegeApplicationCycle>;

                var colleges = SeedDataFixture.Colleges.Select(dto =>
                    new College
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Name = dto.Name,
                        HasEtms = dto.HasEtms,
                        IsOpen = SeedDataFixture.SchoolStatuses.FirstOrDefault(s => s.Id == dto.SchoolStatusId)?.Code == Constants.SchoolStatuses.Open,
                        TranscriptFee = dto.TranscriptFee,
                        AllowCba = dto.AllowCba,
                        AllowCbaMultiCollegeApply = dto.AllowCbaMultiCollegeApply,
                        AllowCbaReferralCodeAsSource = dto.AllowCbaReferralCodeAsSource,
                        Address = new MailingAddress
                        {
                            City = dto.MailingAddress.City,
                            Country = dto.MailingAddress.Country,
                            PostalCode = dto.MailingAddress.PostalCode,
                            ProvinceState = dto.MailingAddress.ProvinceState,
                            Street = dto.MailingAddress.Street
                        }
                    }).ToList() as IList<College>;

                var communityInvolvements = SeedDataFixture.CommunityInvolvements.Select(dto =>
                   new LookupItem
                   {
                       Id = dto.Id,
                       Code = dto.Code,
                       Label = dto.LocalizedName
                   }).ToList() as IList<LookupItem>;

                var countries = SeedDataFixture.Countries.Select(dto =>
                    new Country
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName,
                        Name = dto.Name
                    }).ToList() as IList<Country>;

                var courseDeliveries = SeedDataFixture.CourseDeliveries.Select(dto =>
                   new LookupItem
                   {
                       Id = dto.Id,
                       Code = dto.Code,
                       Label = dto.LocalizedName
                   }).ToList() as IList<LookupItem>;

                var courseStatuses = SeedDataFixture.CourseStatuses.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var courseTypes = SeedDataFixture.CourseTypes.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var credentialEvaluationAgencies = SeedDataFixture.CredentialEvaluationAgencies.Select(dto =>
                new LookupItem
                {
                    Id = dto.Id,
                    Code = dto.Code,
                    Label = dto.Name
                }).ToList() as IList<LookupItem>;

                var credentials = SeedDataFixture.Credentials.Select(dto =>
                   new LookupItem
                   {
                       Id = dto.Id,
                       Code = dto.Code,
                       Label = dto.LocalizedName
                   }).ToList() as IList<LookupItem>;

                var currents = SeedDataFixture.Currents.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var currencies = SeedDataFixture.Currencies.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.Name
                    }).ToList() as IList<LookupItem>;

                var documentPrints = SeedDataFixture.DocumentPrints.Select(dto =>
                new DocumentPrint
                {
                    Id = dto.Id,
                    CollegeId = dto.CollegeId,
                    DocumentTypeId = dto.DocumentTypeId,
                    SendToColtrane = dto.SendToColtrane
                }).ToList() as IList<DocumentPrint>;

                var entryLevels = SeedDataFixture.EntryLevels.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var firstGenerationApplicants = SeedDataFixture.FirstGenerationApplicants.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var firstLanguages = SeedDataFixture.FirstLanguages.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var genders = SeedDataFixture.Genders.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var gradeTypes = SeedDataFixture.GradeTypes.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var grades = SeedDataFixture.LastGradeCompleteds.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var highestEducations = SeedDataFixture.HighestEducations.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Name,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var highSchools = SeedDataFixture.HighSchools.Select(dto =>
                    new HighSchool
                    {
                        Id = dto.Id,
                        BoardMident = dto.BoardMident,
                        Code = dto.Code,
                        LocalizedName = dto.LocalizedName,
                        Mident = dto.Mident,
                        Name = dto.Name,
                        ParentId = dto.ParentId,
                        TranscriptFee = dto.TranscriptFee,
                        HasEtms = dto.HasEtms,
                        SchoolTypeId = dto.SchoolTypeId,
                        SchoolType = dto.SchoolType,
                        SchoolBoardName = dto.SchoolBoardName,
                        SchoolId = dto.SchoolId,
                        SchoolStatusId = dto.SchoolStatusId,
                        SchoolStatus = dto.SchoolStatus,
                        ShowInEducation = dto.ShowInEducation,
                        AddressTypeCode = dto.AddressTypeCode,
                        Address = new MailingAddress
                        {
                            City = dto.MailingAddress.City,
                            Country = dto.MailingAddress.Country,
                            PostalCode = dto.MailingAddress.PostalCode,
                            ProvinceState = dto.MailingAddress.ProvinceState,
                            Street = dto.MailingAddress.Street
                        }
                    }).ToList() as IList<HighSchool>;

                var highSkillsMajors = SeedDataFixture.HighSkillsMajors.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var institutes = SeedDataFixture.Institutes.Select(dto =>
                new LookupItem
                {
                    Id = dto.Id,
                    Code = dto.Code,
                    Label = dto.LocalizedName
                }).ToList() as IList<LookupItem>;

                var instituteTypes = SeedDataFixture.InstituteTypes.Select(dto =>
                   new LookupItem
                   {
                       Id = dto.Id,
                       Code = dto.Code,
                       Label = dto.LocalizedName
                   }).ToList() as IList<LookupItem>;

                var literacyTests = SeedDataFixture.LiteracyTests.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var instituteWarnings = SeedDataFixture.TranscriptRequestExceptions.Select(dto =>
                    new InstituteWarning
                    {
                        Id = dto.Id,
                        InstituteId = dto.InstituteId,
                        Content = dto.LocalizedName,
                        Type = new Faker().PickRandom<InstituteWarningType>()
                    }) as IList<InstituteWarning>;

                var internationalCreditAssessmentStatuses = SeedDataFixture.InternationalCreditAssessmentStatuses.Select(dto =>
                   new LookupItem
                   {
                       Id = dto.Id,
                       Code = dto.Code,
                       Label = dto.LocalizedName
                   }).ToList() as IList<LookupItem>;

                var offerStates = SeedDataFixture.OfferStates.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var offerStatuses = SeedDataFixture.OfferStatuses.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var offerTypes = SeedDataFixture.OfferTypes.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var officials = SeedDataFixture.Officials.Select(dto =>
                new LookupItem
                {
                    Id = dto.Id,
                    Code = dto.Code,
                    Label = dto.LocalizedName
                }).ToList() as IList<LookupItem>;

                var paymentMethods = SeedDataFixture.PaymentMethods.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var paymentResults = SeedDataFixture.PaymentResults.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.Name
                    }).ToList() as IList<LookupItem>;

                var preferredCorrespondenceMethods = SeedDataFixture.PreferredCorrespondenceMethods.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var preferredLanguages = SeedDataFixture.PreferredLanguages.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var programIntakeAvailabilities = SeedDataFixture.ProgramIntakeAvailabilities.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var programIntakeStatuses = SeedDataFixture.ProgramIntakeStatuses.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var promotions = SeedDataFixture.Promotions.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var provinceStates = SeedDataFixture.ProvinceStates.Select(dto =>
                    new ProvinceState
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        CountryId = dto.CountryId,
                        Label = dto.LocalizedName
                    }).ToList() as IList<ProvinceState>;

                var referrealPartners = SeedDataFixture.ReferralPartners.Select(dto =>
                new ReferralPartner
                {
                    Id = dto.Id,
                    Code = dto.Code,
                    Name = dto.Name,
                    AllowCba = dto.AllowCba,
                    AllowCbaReferralCodeAsSource = dto.AllowCbaReferralCodeAsSource
                }).ToList() as IList<ReferralPartner>;

                var sources = SeedDataFixture.Sources.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var sponsorAgencies = SeedDataFixture.PreferredSponsorAgencies.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var studyLevels = SeedDataFixture.LevelOfStudies.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var studyMethods = SeedDataFixture.OfferStudyMethods.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var supportingDocumentTypes = SeedDataFixture.SupportingDocumentTypes.Select(dto =>
                new LookupItem
                {
                    Id = dto.Id,
                    Code = dto.Code,
                    Label = dto.LocalizedName
                }).ToList() as IList<LookupItem>;

                var standardizedTestTypes = SeedDataFixture.TestTypes.Select(dto =>
                new LookupItem
                {
                    Id = dto.Id,
                    Code = dto.Code,
                    Label = dto.LocalizedName
                }).ToList() as IList<LookupItem>;

                var titles = SeedDataFixture.Titles.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var transcriptRequestStatuses = SeedDataFixture.TranscriptRequestStatuses.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var transcriptTransmissions = SeedDataFixture.TranscriptTransmissions.Select(dto =>
                new TranscriptTransmission
                {
                    Id = dto.Id,
                    Code = dto.Code,
                    Label = dto.LocalizedName,
                    EligibleUntil = dto.TermDueDate,
                    InstituteTypeId = dto.InstitutionType == Domestic.Enums.InstitutionType.College ? SeedDataFixture.InstituteTypes.First(x => x.Code == Constants.InstituteTypes.College).Id
                        : dto.InstitutionType == Domestic.Enums.InstitutionType.University ? SeedDataFixture.InstituteTypes.First(x => x.Code == Constants.InstituteTypes.University).Id
                        : (Guid?)null
                })
                .Where(dto => dto.Code == Constants.TranscriptTransmissions.SendTranscriptNow || dto.EligibleUntil.HasValue)
                .ToList() as IList<TranscriptTransmission>;

                var universities = SeedDataFixture.Universities.Select(dto =>
                    new University
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Name = dto.Name,
                        HasEtms = dto.HasEtms,
                        SchoolStatusId = dto.SchoolStatusId,
                        TranscriptFee = dto.TranscriptFee,
                        ShowInEducation = dto.ShowInEducation,
                        Address = new MailingAddress
                        {
                            City = dto.MailingAddress.City,
                            Country = dto.MailingAddress.Country,
                            PostalCode = dto.MailingAddress.PostalCode,
                            ProvinceState = dto.MailingAddress.ProvinceState,
                            Street = dto.MailingAddress.Street
                        }
                    }).ToList() as IList<University>;

                var visaStatuses = SeedDataFixture.StatusOfVisas.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                return new AllLookups
                {
                    AboriginalStatuses = aboriginalStatuses,
                    AccountStatuses = accountStatuses,
                    AchievementLevels = achievementLevels,
                    ApplicationCycles = applicationCycles,
                    ApplicationCycleStatuses = applicationCycleStatuses,
                    ApplicationStatuses = applicationStatuses,
                    BasisForAdmissions = basisForAdmissions,
                    Campuses = campuses,
                    CanadianStatuses = canadianStatuses,
                    Cities = cities,
                    CollegeApplicationCycles = collegeApplicationCycles,
                    Colleges = colleges,
                    CommunityInvolvements = communityInvolvements,
                    Countries = countries,
                    CourseDeliveries = courseDeliveries,
                    CourseStatuses = courseStatuses,
                    CourseTypes = courseTypes,
                    CredentialEvaluationAgencies = credentialEvaluationAgencies,
                    Credentials = credentials,
                    Currencies = currencies,
                    Currents = currents,
                    DocumentPrints = documentPrints,
                    EntryLevels = entryLevels,
                    FirstGenerationApplicants = firstGenerationApplicants,
                    FirstLanguages = firstLanguages,
                    Genders = genders,
                    Grades = grades,
                    GradeTypes = gradeTypes,
                    HighSchools = highSchools,
                    HighestEducations = highestEducations,
                    HighSkillsMajors = highSkillsMajors,
                    InstituteWarnings = instituteWarnings,
                    Institutes = institutes,
                    InstituteTypes = instituteTypes,
                    InternationalCreditAssessmentStatuses = internationalCreditAssessmentStatuses,
                    LiteracyTests = literacyTests,
                    OfferStates = offerStates,
                    OfferStatuses = offerStatuses,
                    OfferTypes = offerTypes,
                    Officials = officials,
                    PaymentMethods = paymentMethods,
                    PaymentResults = paymentResults,
                    PreferredCorrespondenceMethods = preferredCorrespondenceMethods,
                    PreferredLanguages = preferredLanguages,
                    ProgramIntakeAvailabilities = programIntakeAvailabilities,
                    ProgramIntakeStatuses = programIntakeStatuses,
                    Promotions = promotions,
                    ProvinceStates = provinceStates,
                    ReferralPartners = referrealPartners,
                    Sources = sources,
                    SponsorAgencies = sponsorAgencies,
                    StudyLevels = studyLevels,
                    StudyMethods = studyMethods,
                    StandardizedTestTypes = standardizedTestTypes,
                    SupportingDocumentTypes = supportingDocumentTypes,
                    Titles = titles,
                    TranscriptRequestStatuses = transcriptRequestStatuses,
                    TranscriptTransmissions = transcriptTransmissions,
                    Universities = universities,
                    VisaStatuses = visaStatuses
                };
            }
        }

        public string Oen => OenExtension.Oen();

        public Faker<Applicant> GetApplicant()
        {
            return new Faker<Applicant>()
                .ApplyApplicantRules(_lookups)
                .RuleFor(o => o.Id, Guid.NewGuid());
        }

        public Faker<ApplicantAddress> GetApplicantAddress()
        {
            return new Faker<ApplicantAddress>()
                .ApplyApplicantAddressRules(_lookups);
        }

        public Faker<ApplicantBase> GetApplicantBase()
        {
            return new Faker<ApplicantBase>()
                .ApplyApplicantBaseRules();
        }

        public Faker<Application> GetApplication()
        {
            return new Faker<Application>()
                .ApplyApplicationRules(_lookups);
        }

        public Faker<ApplicationBase> GetApplicationBase()
        {
            return new Faker<ApplicationBase>()
                .ApplyApplicationBaseRules(_lookups);
        }

        public Faker<Education> GetEducation()
        {
            return new Faker<Education>()
                .ApplyEducationRules(_lookups)
                .RuleFor(x => x.CanDelete, _ => true)
                .RuleFor(x => x.Id, Guid.NewGuid);
        }

        public Faker<EducationBase> GetEducationBase()
        {
            return new Faker<EducationBase>()
                .ApplyEducationRules(_lookups);
        }

        public Faker<IntlCredentialAssessment> GetIntlCredentialAssessment()
        {
            return new Faker<IntlCredentialAssessment>()
                .ApplyIntlCredentialAssessmentRules(_lookups);
        }

        public Faker<MailingAddress> GetMailingAddress()
        {
            return new Faker<MailingAddress>()
                .ApplyMailingAddressRules(_lookups);
        }

        public Faker<Dto.Offer> GetOffer()
        {
            return new Faker<Dto.Offer>()
                .ApplyOfferRules(_lookups);
        }

        public Faker<Order> GetOrder()
        {
            return new Faker<Order>()
                .ApplyOrderRules();
        }

        public Faker<PayOrderInfo> GetPayOrderInfo()
        {
            return new Faker<PayOrderInfo>()
                .ApplyPayOrderInfoRules();
        }

        public Faker<ProgramChoice> GetProgramChoice()
        {
            return new Faker<ProgramChoice>()
                .ApplyProgramChoiceRules(_lookups);
        }

        public Faker<ProgramChoiceBase> GetProgramChoiceBase()
        {
            return new Faker<ProgramChoiceBase>()
                .ApplyProgramChoiceBaseRules(_lookups);
        }

        public Faker<TranscriptRequest> GetTranscriptRequest()
        {
            return new Faker<TranscriptRequest>()
                .ApplyTranscriptRequestRules(_lookups)
                .RuleFor(x => x.Id, Guid.NewGuid);
        }

        public Faker<TranscriptRequestBase> GetTranscriptRequestBase()
        {
            return new Faker<TranscriptRequestBase>()
                .ApplyTranscriptRequestRules(_lookups);
        }

        public Faker<Dto.Voucher> GetVoucher()
        {
            return new Faker<Dto.Voucher>()
                .ApplyVoucherRules();
        }
    }
}
