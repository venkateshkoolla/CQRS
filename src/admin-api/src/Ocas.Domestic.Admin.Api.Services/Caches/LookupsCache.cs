using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CacheManager.Core;
using Ocas.Domestic.Apply.Admin.Api.Services.Extensions;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Core.Settings;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.AppSettings.Extras;
using Ocas.Domestic.Data;
using AllLookups = Ocas.Domestic.Apply.Admin.Models.AllLookups;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services
{
    public class LookupsCache : ILookupsCache
    {
        private static readonly List<MethodInfo> _cacheMethods;
        private static readonly string[] _defaultCacheFilter;
        private static readonly Dictionary<string, PropertyInfo> _properties;
        private static readonly ICacheManager<object> _backupCache;
        private static readonly ConcurrentDictionary<object, SemaphoreSlim> _locks;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline", Justification = "Make it clear to the reader that static initialization is thread-safe")]
        static LookupsCache()
        {
            _cacheMethods = typeof(LookupsCache)
               .GetMethods()
               .Where(x => x.Name.StartsWith("Get") && x.Name != "GetAllLookups" && x.Name != "GetOrAddMissing")
               .Select(x => x.Name)
               .Distinct()
               .OrderBy(x => x)
               .Select(key => typeof(LookupsCache).GetMethod(key))
               .ToList();

            _defaultCacheFilter = _cacheMethods
                .Select(x => x.Name.Substring("Get".Length).ToCamelCase())
                .ToArray();

            _properties = typeof(AllLookups)
                .GetProperties()
                .ToDictionary(x => x.Name.ToCamelCase(), x => x);

            _backupCache = CacheFactory.Build("backupCache", settings => settings.WithSystemRuntimeCacheHandle("backupCacheMemoryLayer"));

            _locks = new ConcurrentDictionary<object, SemaphoreSlim>();
        }

        private readonly ICacheManager<object> _cacheManager;
        private readonly IDomesticContext _domesticContext;
        private readonly IAppSettings _appSettings;
        private readonly IApiMapper _apiMapper;
        private readonly IAppSettingsExtras _appSettingsExtras;

        public LookupsCache(
            ICacheManager<object> cacheManager,
            IDomesticContext domesticContext,
            IAppSettings appSettings,
            IAppSettingsExtras appSettingsExtras,
            IApiMapper apiMapper)
        {
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _appSettingsExtras = appSettingsExtras ?? throw new ArgumentNullException(nameof(appSettingsExtras));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
        }

        public Task<IList<AboriginalStatus>> GetAboriginalStatuses(string locale)
        {
            var lookupKey = $"AboriginalStatuses_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapAboriginalStatus(
                await _domesticContext.GetAboriginalStatuses(crmLocale)));
        }

        public Task<IList<LookupItem>> GetAccountStatuses(string locale)
        {
            var lookupKey = $"AccountStatuses_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetAccountStatuses(crmLocale)));
        }

        public Task<IList<LookupItem>> GetAchievementLevels(string locale)
        {
            var lookupKey = $"AchievementLevels_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetLevelAchieveds(crmLocale)));
        }

        public Task<IList<LookupItem>> GetAdultTrainings(string locale)
        {
            var lookupKey = $"AdultTrainings_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetAdultTrainings(crmLocale)));
        }

        public Task<IList<ApplicationCycle>> GetApplicationCycles()
        {
            const string lookupKey = "ApplicationCycles";

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapApplicationCycles(
                await GetApplicationCyclesDto(),
                await GetApplicationCycleStatuses(Constants.Localization.EnglishCanada),
                _appSettingsExtras));
        }

        public Task<IList<Dto.ApplicationCycle>> GetApplicationCyclesDto()
        {
            const string lookupKey = "ApplicationCyclesDto";

            return GetOrAddMissing(lookupKey, () => _domesticContext.GetApplicationCycles());
        }

        public Task<IList<LookupItem>> GetApplicationCycleStatuses(string locale)
        {
            var lookupKey = $"ApplicationCycleStatuses_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetApplicationCycleStatuses(crmLocale)));
        }

        public Task<IList<LookupItem>> GetApplicationStatuses(string locale)
        {
            var lookupKey = $"ApplicationStatuses_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetApplicationStatuses(crmLocale)));
        }

        public Task<IList<LookupItem>> GetBasisForAdmissions(string locale)
        {
            var lookupKey = $"BasisForAdmissions_{locale}";

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await GetBasisForAdmissionsDto(locale)));
        }

        public Task<IList<Dto.BasisForAdmission>> GetBasisForAdmissionsDto(string locale)
        {
            var lookupKey = $"BasisForAdmissionsDto_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, () => _domesticContext.GetBasisForAdmissions(crmLocale));
        }

        public Task<IList<Campus>> GetCampuses()
        {
            const string lookupKey = "Campuses";

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapCampuses(
                await _domesticContext.GetCampuses()));
        }

        public Task<IList<LookupItem>> GetCanadianStatuses(string locale)
        {
            var lookupKey = $"CanadianStatuses_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetCanadianStatuses(crmLocale)));
        }

        public Task<IList<City>> GetCities(string locale)
        {
            var lookupKey = $"Cities_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapCity(
                await _domesticContext.GetCities(crmLocale)));
        }

        public Task<IList<CollegeApplicationCycle>> GetCollegeApplicationCycles()
        {
            const string lookupKey = "CollegeApplicationCycles";

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapCollegeApplicationCycles(
                await _domesticContext.GetCollegeApplicationCycles(),
                await GetApplicationCycles(),
                await GetApplicationCycleStatuses(Constants.Localization.EnglishCanada)));
        }

        public Task<IList<CollegeInformation>> GetCollegeInformation(string locale)
        {
            var lookupKey = $"CollegeInformations_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapCollegeInformations(
                await _domesticContext.GetCollegeInformations(crmLocale)));
        }

        public Task<IList<College>> GetColleges(string locale)
        {
            var lookupKey = $"Colleges_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapColleges(
                await _domesticContext.GetColleges(),
                await _domesticContext.GetSchoolStatuses(crmLocale)));
        }

        public Task<IList<LookupItem>> GetCommunityInvolvements(string locale)
        {
            var lookupKey = $"CommunityInvolvements_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetCommunityInvolvements(crmLocale)));
        }

        public Task<IList<Country>> GetCountries(string locale)
        {
            var lookupKey = $"Countries_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapCountry(
                await _domesticContext.GetCountries(crmLocale)));
        }

        public Task<IList<LookupItem>> GetCourseDeliveries(string locale)
        {
            var lookupKey = $"GetCourseDeliveries_{locale}";
            var crmLocale = locale.ToLocaleEnum();
            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetCourseDeliveries(crmLocale)));
        }

        public Task<IList<LookupItem>> GetCourseStatuses(string locale)
        {
            var lookupKey = $"GetCourseStatuses_{locale}";
            var crmLocale = locale.ToLocaleEnum();
            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetCourseStatuses(crmLocale)));
        }

        public Task<IList<LookupItem>> GetCourseTypes(string locale)
        {
            var lookupKey = $"GetCourseTypes_{locale}";
            var crmLocale = locale.ToLocaleEnum();
            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetCourseTypes(crmLocale)));
        }

        public Task<IList<LookupItem>> GetCredentialEvaluationAgencies()
        {
            const string lookupKey = "CredentialEvaluationAgencies";

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetCredentialEvaluationAgencies()));
        }

        public Task<IList<LookupItem>> GetCredentials(string locale)
        {
            var lookupKey = $"Credentials_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetCredentials(crmLocale)));
        }

        public Task<IList<LookupItem>> GetCurrents(string locale)
        {
            var lookupKey = $"Currents_{locale}";

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await GetCurrentsDto(locale)));
        }

        public Task<IList<Dto.Current>> GetCurrentsDto(string locale)
        {
            var lookupKey = $"CurrentsDto_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, () => _domesticContext.GetCurrents(crmLocale));
        }

        public Task<IList<LookupItem>> GetCurrencies()
        {
            const string lookupKey = "Currencies";

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetCurrencies()));
        }

        public Task<IList<DocumentPrint>> GetDocumentPrints()
        {
            const string lookupKey = "DocumentPrints";

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapDocumentPrints(
                await _domesticContext.GetDocumentPrints()));
        }

        public Task<IList<LookupItem>> GetEntryLevels(string locale)
        {
            var lookupKey = $"EntryLevels_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetEntryLevels(crmLocale)));
        }

        public Task<IList<LookupItem>> GetFirstGenerationApplicants(string locale)
        {
            var lookupKey = $"FirstGenerationApplicants_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetFirstGenerationApplicants(crmLocale)));
        }

        public Task<IList<LookupItem>> GetFirstLanguages(string locale)
        {
            var lookupKey = $"FirstLanguages_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetFirstLanguages(crmLocale)));
        }

        public Task<IList<LookupItem>> GetGenders(string locale)
        {
            var lookupKey = $"Genders_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetGenders(crmLocale)));
        }

        public Task<IList<LookupItem>> GetGrades(string locale)
        {
            var lookupKey = $"Grades_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetLastGradeCompleteds(crmLocale)));
        }

        public Task<IList<LookupItem>> GetGradeTypes(string locale)
        {
            var lookupKey = $"GetGradeTypes_{locale}";
            var crmLocale = locale.ToLocaleEnum();
            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetGradeTypes(crmLocale)));
        }

        public Task<IList<LookupItem>> GetHighestEducations(string locale)
        {
            var lookupKey = $"HighestEducations_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetHighestEducations(crmLocale)));
        }

        public Task<IList<LookupItem>> GetHighlyCompetitives(string locale)
        {
            var lookupKey = $"HighlyCompetitives_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetHighlyCompetitives(crmLocale)));
        }

        public Task<IList<HighSchool>> GetHighSchools(string locale)
        {
            var lookupKey = $"HighSchools_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapHighSchools(
                await _domesticContext.GetHighSchools(crmLocale)));
        }

        public Task<IList<LookupItem>> GetHighSkillsMajors(string locale)
        {
            var lookupKey = $"HighSkillsMajors_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetHighSkillsMajors(crmLocale)));
        }

        public Task<IList<LookupItem>> GetInstitutes()
        {
            const string lookupKey = "Institutes";

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetInstitutes()));
        }

        public Task<IList<LookupItem>> GetInstituteTypes(string locale)
        {
            var lookupKey = $"InstituteTypes_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetInstituteTypes(crmLocale)));
        }

        public Task<IList<InstituteWarning>> GetInstituteWarnings(string locale)
        {
            var lookupKey = $"InstituteWarnings_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapInstituteWarnings(
                await _domesticContext.GetTranscriptRequestExceptions(crmLocale), _appSettings.GetAppSetting<IList<Guid>>(Constants.AppSettings.ExceptionsEducation)));
        }

        public Task<IList<LookupItem>> GetIntakeAvailabilities(string locale)
        {
            var lookupKey = $"IntakeAvailabilities_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetProgramIntakeAvailabilities(crmLocale)));
        }

        public Task<IList<LookupItem>> GetIntakeExpiryActions(string locale)
        {
            var lookupKey = $"IntakeExpiryActions_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetExpiryActions(crmLocale)));
        }

        public Task<IList<LookupItem>> GetIntakeStatuses(string locale)
        {
            var lookupKey = $"IntakeStatuses_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetProgramIntakeStatuses(crmLocale)));
        }

        public Task<IList<LookupItem>> GetInternationalCreditAssessmentStatuses(string locale)
        {
            var lookupKey = $"InternationalCreditAssessmentStatuses_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetInternationalCreditAssessmentStatuses(crmLocale)));
        }

        public Task<IList<LookupItem>> GetLanguages(string locale)
        {
            return GetProgramLanguages(locale);
        }

        public Task<PrivacyStatement> GetLatestPrivacyStatement(string locale)
        {
            var lookupKey = $"LatestPrivacyStatement_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapPrivacyStatement(
                await _domesticContext.GetLatestApplicantPrivacyStatement(crmLocale)));
        }

        public Task<IList<LookupItem>> GetLiteracyTests(string locale)
        {
            var lookupKey = $"LiteracyTests_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetLiteracyTests(crmLocale)));
        }

        public Task<IList<McuCode>> GetMcuCodes(string locale)
        {
            var lookupKey = $"McuCodes_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapMcuCodes(
                await _domesticContext.GetMcuCodes(crmLocale)));
        }

        public Task<IList<LookupItem>> GetMinistryApprovals(string locale)
        {
            var lookupKey = $"MinistryApprovals_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetMinistryApprovals(crmLocale)));
        }

        public Task<IList<LookupItem>> GetOfferStates(string locale)
        {
            var lookupKey = $"OfferStates_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetOfferStates(crmLocale)));
        }

        public Task<IList<LookupItem>> GetOfferStatuses(string locale)
        {
            var lookupKey = $"OfferStatuses_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetOfferStatuses(crmLocale)));
        }

        public Task<IList<LookupItem>> GetOfferTypes(string locale)
        {
            var lookupKey = $"OfferTypes_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetOfferTypes(crmLocale)));
        }

        public Task<IList<LookupItem>> GetOfficials(string locale)
        {
            var lookupKey = $"Officials_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetOfficials(crmLocale)));
        }

        public Task<IList<LookupItem>> GetOstNotes(string locale)
        {
            var lookupKey = $"OstNotes_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetOstNotes(crmLocale)));
        }

        public Task<IList<LookupItem>> GetPaymentMethods(string locale)
        {
            var lookupKey = $"PaymentMethods_{locale}";

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await GetPaymentMethodsDto(locale)));
        }

        public Task<IList<Dto.PaymentMethod>> GetPaymentMethodsDto(string locale)
        {
            var lookupKey = $"PaymentMethodsDto_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, () => _domesticContext.GetPaymentMethods(crmLocale));
        }

        public Task<IList<LookupItem>> GetPaymentResults()
        {
            const string lookupKey = "PaymentResults";

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetPaymentResults()));
        }

        public Task<IList<LookupItem>> GetPreferredCorrespondenceMethods(string locale)
        {
            var lookupKey = $"PreferredCorrespondenceMethods_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetPreferredCorrespondenceMethods(crmLocale)));
        }

        public Task<IList<LookupItem>> GetPreferredLanguages(string locale)
        {
            var lookupKey = $"PreferredLanguages_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetPreferredLanguages(crmLocale)));
        }

        public Task<IList<LookupItem>> GetProgramCategories(string locale)
        {
            var lookupKey = $"ProgramCategories_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetProgramCategories(crmLocale)));
        }

        public Task<IList<LookupItem>> GetProgramCredentials(string locale)
        {
            var lookupKey = $"ProgramCredentials_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetCredentials(crmLocale)));
        }

        public Task<IList<LookupItem>> GetProgramDeliveries(string locale)
        {
            return GetStudyMethods(locale);
        }

        public Task<IList<LookupItem>> GetProgramIntakeAvailabilities(string locale)
        {
            var lookupKey = $"ProgramIntakeAvailabilities_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetProgramIntakeAvailabilities(crmLocale)));
        }

        public Task<IList<LookupItem>> GetProgramIntakeStatuses(string locale)
        {
            var lookupKey = $"ProgramIntakeStatuses_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetProgramIntakeStatuses(crmLocale)));
        }

        public Task<IList<LookupItem>> GetProgramLanguages(string locale)
        {
            var lookupKey = $"ProgramLanguages_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetProgramLanguages(crmLocale)));
        }

        public Task<IList<LookupItem>> GetProgramLengths(string locale)
        {
            var lookupKey = $"ProgramLengths_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetUnitOfMeasures(crmLocale)));
        }

        public Task<IList<LookupItem>> GetProgramLengthTypes(string locale)
        {
            return GetProgramLengths(locale);
        }

        public Task<IList<LookupItem>> GetProgramLevels(string locale)
        {
            var lookupKey = $"ProgramLevels_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetProgramLevels(crmLocale)));
        }

        public Task<IList<LookupItem>> GetProgramPromotions(string locale)
        {
            return GetPromotions(locale);
        }

        public Task<IList<SubCategory>> GetProgramSubCategories(string locale)
        {
            var lookupKey = $"ProgramSubCategories_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapSubCategory(
                await _domesticContext.GetProgramSubCategories(crmLocale)));
        }

        public Task<IList<LookupItem>> GetProgramTypes(string locale)
        {
            var lookupKey = $"ProgramTypes_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetProgramTypes(crmLocale)));
        }

        public Task<IList<LookupItem>> GetPromotions(string locale)
        {
            var lookupKey = $"Promotions_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetPromotions(crmLocale)));
        }

        public Task<IList<ProvinceState>> GetProvinceStates(string locale)
        {
            var lookupKey = $"ProvinceStates_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapProvinceState(
                await _domesticContext.GetProvinceStates(crmLocale)));
        }

        public Task<IList<ReferralPartner>> GetReferralPartners()
        {
            const string lookupKey = "ReferralPartners";

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapReferralPartners(
                await _domesticContext.GetReferralPartners()));
        }

        public Task<IList<LookupItem>> GetShsmCompletions()
        {
            const string lookupKey = "ShsmCompletions";

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetShsmCompletions()));
        }

        public Task<IList<LookupItem>> GetSources(string locale)
        {
            var lookupKey = $"Sources_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetSources(crmLocale)));
        }

        public Task<IList<LookupItem>> GetSponsorAgencies(string locale)
        {
            var lookupKey = $"SponsorAgencies_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetPreferredSponsorAgencies(crmLocale)));
        }

        public Task<IList<LookupItem>> GetStandardizedTestTypes(string locale)
        {
            var lookupKey = $"StandardizedTestTypes_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetTestTypes(crmLocale)));
        }

        public Task<IList<LookupItem>> GetStudyAreas(string locale)
        {
            var lookupKey = $"StudyAreas_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetStudyAreas(crmLocale)));
        }

        public Task<IList<LookupItem>> GetStudyLevels(string locale)
        {
            var lookupKey = $"StudyLevels_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetLevelOfStudies(crmLocale)));
        }

        public Task<IList<LookupItem>> GetStudyMethods(string locale)
        {
            var lookupKey = $"StudyMethods_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetOfferStudyMethods(crmLocale)));
        }

        public Task<IList<LookupItem>> GetSupportingDocumentTypes(string locale)
        {
            var lookupKey = $"SupportingDocumentTypes_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetSupportingDocumentTypes(crmLocale)));
        }

        public Task<IList<LookupItem>> GetTitles(string locale)
        {
            var lookupKey = $"Titles_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetTitles(crmLocale)));
        }

        public Task<IList<LookupItem>> GetTranscriptRequestStatuses(string locale)
        {
            var lookupKey = $"TranscriptRequestStatuses_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetTranscriptRequestStatuses(crmLocale)));
        }

        public Task<IList<LookupItem>> GetTranscriptSources(string locale)
        {
            var lookupKey = $"TranscriptSources_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetTranscriptSources(crmLocale)));
        }

        public Task<IList<TranscriptTransmission>> GetTranscriptTransmissions(string locale)
        {
            var lookupKey = $"TranscriptTransmissions_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapTranscriptTransmissions(
                await _domesticContext.GetTranscriptTransmissionDates(crmLocale), await _domesticContext.GetInstituteTypes(crmLocale)));
        }

        public Task<IList<University>> GetUniversities()
        {
            const string lookupKey = "Universities";

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapUniversity(
                await _domesticContext.GetUniversities()));
        }

        public Task<IList<LookupItem>> GetVisaStatuses(string locale)
        {
            var lookupKey = $"VisaStatuses_{locale}";
            var crmLocale = locale.ToLocaleEnum();

            return GetOrAddMissing(lookupKey, async () => _apiMapper.MapLookupItem(
                await _domesticContext.GetStatusOfVisas(crmLocale)));
        }

        public async Task<AllLookups> GetAllLookups(string locale, string[] keys)
        {
            if (keys is null)
            {
                keys = _defaultCacheFilter;
            }
            else
            {
                Array.Sort(keys);
            }

            var lookups = new AllLookups();

            foreach (var methodInfo in _cacheMethods)
            {
                var key = methodInfo.Name.Substring("Get".Length).ToCamelCase();

                // check that current cache item exists in the requested items
                if (Array.BinarySearch(keys, key) < 0) continue;

                // check that current cache item exists as a property on the return type (i.e. AllLookups)
                _properties.TryGetValue(key, out var propertyInfo);
                if (propertyInfo is null) continue;

                // some lookups don't require locale as a parameter
                var parameters = Array.Empty<object>();
                if (methodInfo.GetParameters().Length == 1)
                {
                    parameters = new object[] { locale };
                }

                // https://stackoverflow.com/a/48033941
                var task = (Task)methodInfo.Invoke(this, parameters);
                await task.ConfigureAwait(false);

                propertyInfo.SetValue(lookups, ((dynamic)task).Result, null);
            }

            return lookups;
        }

        public void PurgeAllLookups(string[] keys)
        {
            if (keys is null)
            {
                _backupCache.Clear();
            }
            else
            {
                foreach (var key in keys)
                {
                    var keyCamelCase = key.ToCamelCase();
                    _backupCache.Remove(keyCamelCase);

                    foreach (var locale in Constants.Localization.SupportedLocalizations)
                    {
                        _backupCache.Remove($"{keyCamelCase}_{locale}");
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function that will load the lookups from the data layer if the backup cache is missing/expired.
        /// http://michaco.net/blog/WhatIfRedisStopsWorkingHowDoIkeepMyAppRunning
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="ctxFunc">Function that returns data for the cache item</param>
        /// <returns></returns>
#pragma warning disable RCS1165 // Unconstrained type parameter checked for null.
        private async Task<T> GetOrAddMissing<T>(string key, Func<Task<T>> ctxFunc)
        {
            var cacheExpiryMinutes = _appSettings.GetAppSettingOrDefault<double>("ocas:cacheExpiryMinutes", 1440);
            var cacheExpiry = TimeSpan.FromMinutes(cacheExpiryMinutes);

            // Try the memory and Redis cache first
            var hit = _cacheManager.Get<T>(key);

            if (hit != null) return hit;

            // If Redis is down, check our MemoryCache backup
            var miss = _backupCache.Get<T>(key);

            if (miss != null) return miss;

            // If MemoryCache is invalid, call data context
            var cacheLock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            await cacheLock.WaitAsync();
            try
            {
                miss = _backupCache.Get<T>(key);

                if (miss != null) return miss;

                var item = await ctxFunc.Invoke();
                var cacheItem = new CacheItem<object>(key, item, ExpirationMode.Absolute, cacheExpiry);
                _backupCache.Add(cacheItem);

                return item;
            }
            finally
            {
                cacheLock.Release();
            }
        }

#pragma warning restore RCS1165 // Unconstrained type parameter checked for null.
    }
}
