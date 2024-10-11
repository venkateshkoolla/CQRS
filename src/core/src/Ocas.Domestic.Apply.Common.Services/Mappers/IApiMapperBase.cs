using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.AppSettings.Extras;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Services.Mappers
{
    public interface IApiMapperBase
    {
        Applicant MapApplicant(Dto.Contact dbDto, IList<AboriginalStatus> aboriginalStatuses, IList<LookupItem> titles);
        Applicant MapApplicant(Dto.Contact dbDto, IList<AboriginalStatus> aboriginalStatuses, IList<LookupItem> titles, IList<LookupItem> sources, IList<College> colleges, IList<ReferralPartner> referralPartners);
        Task<Education> MapEducation(Dto.Education entity, IList<LookupItem> instituteTypes, IList<College> colleges, IList<HighSchool> highSchools, IList<University> universities, IDomesticContext domesticContext);
        FinancialTransaction MapFinancialTransaction(Dto.FinancialTransaction dbDto);
        IList<Offer> MapOffers(IList<Dto.Offer> model, IList<LookupItem> offerStates, IList<LookupItem> applicationStatuses, IAppSettingsExtras appSettingsExtras);
        Order MapOrder(Dto.Order dbDto, Dto.FinancialTransaction financialTransaction = null);
        IList<ProgramChoice> MapProgramChoices(IList<Dto.ProgramChoice> dbDtos, IList<Dto.ProgramIntake> intakes, IList<LookupItem> intakeAvailabilities, IList<Dto.ShoppingCartDetail> details);
        ProgramChoice MapProgramChoice(Dto.ProgramChoice dbDto);
        IList<ShoppingCartDetail> MapShoppingCartDetail(IList<Dto.ShoppingCartDetail> shoppingCartDetails, Dto.Contact applicant, Dto.Application application);
    }
}
