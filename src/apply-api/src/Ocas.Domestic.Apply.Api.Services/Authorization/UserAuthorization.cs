using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Settings;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Api.Services
{
    public class UserAuthorization : IUserAuthorization
    {
        private readonly ILogger<UserAuthorization> _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IAppSettings _appSettings;

        public UserAuthorization(ILogger<UserAuthorization> logger, IDomesticContext domesticContext, IAppSettings appSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task CanAccessApplicantAsync(IPrincipal user, Guid applicantId, bool skipActiveCheck = false)
        {
            if (applicantId.IsEmpty())
                throw new ValidationException("'Applicant Id' must not be empty.");

            if (IsOcasUser(user)) return;

            var subjectId = await _domesticContext.GetContactSubjectId(applicantId);
            if (string.IsNullOrEmpty(subjectId)) throw new NotFoundException($"Applicant {applicantId} not found");

            if (!user.GetUpnOrEmail().Equals(subjectId, StringComparison.InvariantCultureIgnoreCase)
                && !user.GetSubject().Equals(subjectId, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ForbiddenException();
            }

            if (skipActiveCheck) return;

            // IsActive == not duplicate or deceased
            var isActive = await _domesticContext.IsActive(applicantId);
            if (!isActive) throw new ValidationException("Applicant must be active");
        }

        public bool IsOcasUser(IPrincipal user)
        {
            return _appSettings.IdSvrRolesOcasUser.Any(user.IsInRole);
        }
    }
}