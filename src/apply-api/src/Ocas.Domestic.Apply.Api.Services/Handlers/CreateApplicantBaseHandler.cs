using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Extensions;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;
using Ocas.Domestic.Data.Extras;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class CreateApplicantBaseHandler : IRequestHandler<CreateApplicantBase, Applicant>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IDomesticContextExtras _domesticContextExtras;
        private readonly IApiMapper _apiMapper;
        private readonly ILookupsCache _lookupsCache;
        private readonly string _locale;
        private readonly string _sourcePartner;

        public CreateApplicantBaseHandler(ILogger<CreateApplicantBaseHandler> logger, IDomesticContext domesticContext, IDomesticContextExtras domesticContextExtras, IApiMapper apiMapper, ILookupsCache lookupsCache, RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _domesticContextExtras = domesticContextExtras ?? throw new ArgumentNullException(nameof(domesticContextExtras));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
            _sourcePartner = requestCache.Get<string>(Constants.RequestCacheKeys.Partner);
        }

        public async Task<Applicant> Handle(CreateApplicantBase request, CancellationToken cancellationToken)
        {
            var existingApplicant = await _domesticContext.GetContact(request.User.GetUpnOrEmail());
            if (existingApplicant != null)
            {
                // Update education status, if missing, for existing applicants of original A2C based on already answered BOA questions
                var eduStatusChanges = await _domesticContextExtras.PatchEducationStatus(
                    existingApplicant,
                    request.User.GetUpnOrEmail(),
                    await _lookupsCache.GetBasisForAdmissionsDto(Constants.Localization.FallbackLocalization),
                    await _lookupsCache.GetCurrentsDto(Constants.Localization.FallbackLocalization),
                    await _lookupsCache.GetApplicationCyclesDto());
                if (eduStatusChanges) existingApplicant = await _domesticContext.UpdateContact(existingApplicant);

                return _apiMapper.MapApplicant(
                existingApplicant,
                await _lookupsCache.GetAboriginalStatuses(_locale),
                await _lookupsCache.GetTitles(_locale));
            }

            var accountStatuses = await _lookupsCache.GetAccountStatuses(_locale);
            var accountStatusId = accountStatuses.Single(x =>
            {
                const int status = (int)Core.Enums.AccountStatus.Active;
                return x.Code == status.ToString();
            }).Id;

            var preferredLanguages = await _lookupsCache.GetPreferredLanguages(_locale);
            var preferredLanguageId = preferredLanguages.Single(x =>
            {
                var lang = (int)_locale.ToPreferredLanguageEnum();
                return x.Code == lang.ToString();
            }).Id;

            var email = request.User.GetUpnOrEmail();

            var applicant = new Applicant
            {
                FirstName = request.ApplicantBase.FirstName,
                MiddleName = request.ApplicantBase.MiddleName,
                LastName = request.ApplicantBase.LastName,
                BirthDate = request.ApplicantBase.BirthDate,
                Email = email,
                UserName = email,
                SubjectId = request.User.GetSubject(),
                AccountStatusId = accountStatusId,
                PreferredLanguageId = preferredLanguageId
            };
            await ApplicantValidateAsync(applicant);

            var(sourceId, sourcePartnerId) = await _lookupsCache.GetSourcePartnerId(_sourcePartner);

            var dbDto = new Dto.ContactBase
            {
                FirstName = applicant.FirstName,
                MiddleName = applicant.MiddleName,
                LastName = applicant.LastName,
                BirthDate = applicant.BirthDate.ToDateTime(Constants.DateFormat.YearMonthDay),
                Email = applicant.Email,
                Username = applicant.UserName,
                SubjectId = applicant.SubjectId,
                AccountStatusId = applicant.AccountStatusId,
                PreferredLanguageId = applicant.PreferredLanguageId,
                SourceId = sourceId,
                ModifiedBy = request.User.GetUpnOrEmail(),
                ContactType = ContactType.Applicant,
                DoNotSendMM = true,
                SourcePartnerId = sourcePartnerId,
                LastLogin = (DateTime?)DateTime.UtcNow
            };

            return _apiMapper.MapApplicant(
                await _domesticContext.CreateContact(dbDto),
                await _lookupsCache.GetAboriginalStatuses(_locale),
                await _lookupsCache.GetTitles(_locale));
        }

        private async Task ApplicantValidateAsync(Applicant applicant)
        {
            var isDuplicateDetails = await _domesticContext.IsDuplicateContact(Guid.Empty, applicant.FirstName, applicant.LastName, applicant.BirthDate.ToDateTime());
            if (isDuplicateDetails) throw new ConflictException("Applicant exists with same first name, last name and date of birth");

            var isDuplicateEmail = await _domesticContext.IsDuplicateContact(Guid.Empty, applicant.Email);
            if (isDuplicateEmail) throw new ConflictException($"Applicant exists with {applicant.Email}");
        }
    }
}