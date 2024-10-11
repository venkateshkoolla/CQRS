using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Core.Settings;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Admin.Api.Services
{
    public class UserAuthorization : IUserAuthorization
    {
        private readonly ILogger<UserAuthorization> _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly ILookupsCache _lookupsCache;
        private readonly IAppSettings _appSettings;

        public UserAuthorization(ILogger<UserAuthorization> logger, IDomesticContext domesticContext, ILookupsCache lookupsCache, IAppSettings appSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task CanAccessApplicantAsync(IPrincipal user, Guid applicantId, bool skipActiveCheck = false)
        {
            var userType = GetUserType(user);

            switch (userType)
            {
                case UserType.CollegeUser:
                case UserType.HighSchoolUser:
                case UserType.HighSchoolBoardUser:
                    if (await _domesticContext.CanAccessApplicant(applicantId, user.GetPartnerId(), userType)) return;
                    throw new ForbiddenException();
                case UserType.OcasUser:
                    var subjectId = await _domesticContext.GetContactSubjectId(applicantId);
                    if (string.IsNullOrEmpty(subjectId)) throw new NotFoundException($"Applicant {applicantId} not found");
                    return;
                default:
                    throw new ForbiddenException();
            }
        }

        public async Task CanAccessCollegeAsync(IPrincipal user, Guid collegeId)
        {
            if (GetUserType(user) == UserType.OcasUser) return;

            var colleges = await _lookupsCache.GetColleges(Constants.Localization.FallbackLocalization);
            var college = colleges.FirstOrDefault(c => c.Id == collegeId);

            if (college == null)
                throw new ValidationException($"College does not exist: {collegeId}");

            if (college.Code != user.GetPartnerId())
                throw new NotAuthorizedException("User does not have access to college");
        }

        public UserType GetUserType(IPrincipal user)
        {
            if (_appSettings.IdSvrRolesOcasUser.Any(x => user.IsInRole(x))) return UserType.OcasUser;
            if (_appSettings.IdSvrRolesPartnerCollegeUser.Any(x => user.IsInRole(x))) return UserType.CollegeUser;
            if (_appSettings.IdSvrRolesPartnerHighSchoolUser.Any(x => user.IsInRole(x))) return UserType.HighSchoolUser;
            if (_appSettings.IdSvrRolesPartnerHSBoardUser.Any(x => user.IsInRole(x))) return UserType.HighSchoolBoardUser;

            throw new ForbiddenException();
        }

        public bool IsOcasUser(IPrincipal user)
        {
            return GetUserType(user) == UserType.OcasUser;
        }

        public bool IsHighSchoolUser(IPrincipal user)
        {
            return GetUserType(user) == UserType.HighSchoolBoardUser || GetUserType(user) == UserType.HighSchoolUser;
        }

        public bool IsOcasTier2User(IPrincipal user)
        {
            return GetUserType(user) == UserType.OcasUser
                && Constants.Roles.OcasTier2.Any(x => user.IsInRole(x));
        }
    }
}