using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Core.Settings;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;
using Ocas.Domestic.Data.Extras;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetApplicantHandler : IRequestHandler<GetApplicant, Applicant>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IDomesticContextExtras _domesticContextExtras;
        private readonly IApiMapper _apiMapper;
        private readonly ILookupsCache _lookupsCache;
        private readonly IAppSettings _appSettings;
        private readonly IUserAuthorization _userAuthorization;

        public GetApplicantHandler(ILogger<GetApplicantHandler> logger, IDomesticContext domesticContext, IDomesticContextExtras domesticContextExtras, IApiMapper apiMapper, ILookupsCache lookupsCache, IAppSettings appSettings, IUserAuthorization userAuthorization)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _domesticContextExtras = domesticContextExtras ?? throw new ArgumentNullException(nameof(domesticContextExtras));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
        }

        public async Task<Applicant> Handle(GetApplicant request, CancellationToken cancellationToken)
        {
            Dto.Contact dbDto;

            if (request.ApplicantId.HasValue)
            {
                dbDto = await _domesticContext.GetContact(request.ApplicantId.Value) ??
                        throw new NotFoundException($"Applicant {request.ApplicantId} not found");
            }
            else
            {
                dbDto = await _domesticContext.GetContact(subjectId: request.User.GetSubject()) ??
                        throw new NotFoundException($"Applicant {request.User.GetSubject()} not found");
            }

            if (dbDto.ContactType != ContactType.Applicant)
                throw new ForbiddenException();

            await _userAuthorization.CanAccessApplicantAsync(request.User, dbDto.Id, true);

            try
            {
                // Update education status, if missing, for existing applicants of original A2C based on already answered BOA questions
                var eduStatusChanges = await _domesticContextExtras.PatchEducationStatus(
                    dbDto,
                    request.User.GetUpnOrEmail(),
                    await _lookupsCache.GetBasisForAdmissionsDto(Constants.Localization.FallbackLocalization),
                    await _lookupsCache.GetCurrentsDto(Constants.Localization.FallbackLocalization),
                    await _lookupsCache.GetApplicationCyclesDto());
                if (eduStatusChanges) dbDto = await _domesticContext.UpdateContact(dbDto);

                var originalLastLoginExceed = dbDto.LastLoginExceed;

                var completedSteps = await _domesticContext.GetCompletedStep(dbDto.Id);
                dbDto.LastLoginExceed = completedSteps != null
                                        && completedSteps >= CompletedSteps.Experience
                                        && dbDto.LastLogin.TotalMonths() <= (Math.Abs(_appSettings.GetAppSetting<int>(Constants.AppSettings.LoginExpiryMonths)) * -1);

                // don't change LastLogin time if this is an OCAS user that is accessing the applicant
                var originalLastLogin = dbDto.LastLogin;
                if (!dbDto.LastLoginExceed && !_userAuthorization.IsOcasUser(request.User))
                    dbDto.LastLogin = DateTime.UtcNow;

                // only update Contact if fields have changed
                if (dbDto.LastLogin != originalLastLogin || dbDto.LastLoginExceed != originalLastLoginExceed)
                {
                    dbDto.ModifiedBy = request.User.GetUpnOrEmail();
                    await _domesticContext.UpdateContact(dbDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return _apiMapper.MapApplicant(
                dbDto,
                await _lookupsCache.GetAboriginalStatuses(Constants.Localization.EnglishCanada),
                await _lookupsCache.GetTitles(Constants.Localization.EnglishCanada));
        }
    }
}
