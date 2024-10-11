using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply
{
    public interface ILookupsCacheBase
    {
        Task<IList<ApplicationCycle>> GetApplicationCycles();
        Task<IList<Dto.ApplicationCycle>> GetApplicationCyclesDto();
        Task<IList<LookupItem>> GetApplicationCycleStatuses(string locale);
        Task<IList<LookupItem>> GetApplicationStatuses(string locale);
        Task<IList<LookupItem>> GetBasisForAdmissions(string locale);
        Task<IList<Dto.BasisForAdmission>> GetBasisForAdmissionsDto(string locale);
        Task<IList<City>> GetCities(string locale);
        Task<IList<CollegeApplicationCycle>> GetCollegeApplicationCycles();
        Task<IList<College>> GetColleges(string locale);
        Task<IList<LookupItem>> GetCommunityInvolvements(string locale);
        Task<IList<Country>> GetCountries(string locale);
        Task<IList<LookupItem>> GetCourseDeliveries(string locale);
        Task<IList<LookupItem>> GetCourseStatuses(string locale);
        Task<IList<LookupItem>> GetCourseTypes(string locale);
        Task<IList<LookupItem>> GetCurrencies();
        Task<IList<LookupItem>> GetCurrents(string locale);
        Task<IList<Dto.Current>> GetCurrentsDto(string locale);
        Task<IList<LookupItem>> GetEntryLevels(string locale);
        Task<IList<LookupItem>> GetGradeTypes(string locale);
        Task<IList<LookupItem>> GetHighestEducations(string locale);
        Task<IList<HighSchool>> GetHighSchools(string locale);
        Task<IList<LookupItem>> GetHighSkillsMajors(string locale);
        Task<IList<LookupItem>> GetLiteracyTests(string locale);
        Task<IList<LookupItem>> GetOfferStatuses(string locale);
        Task<IList<LookupItem>> GetPaymentMethods(string locale);
        Task<IList<Dto.PaymentMethod>> GetPaymentMethodsDto(string locale);
        Task<IList<LookupItem>> GetPaymentResults();
        Task<IList<LookupItem>> GetProgramIntakeAvailabilities(string locale);
        Task<IList<LookupItem>> GetProgramIntakeStatuses(string locale);
        Task<IList<LookupItem>> GetPromotions(string locale);
        Task<IList<ProvinceState>> GetProvinceStates(string locale);
        Task<IList<ReferralPartner>> GetReferralPartners();
        Task<IList<LookupItem>> GetSources(string locale);
        Task<IList<LookupItem>> GetStandardizedTestTypes(string locale);
        Task<IList<University>> GetUniversities();
        Task<IList<LookupItem>> GetTranscriptRequestStatuses(string locale);
    }
}
