using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetApplicantBriefsHandler : IRequestHandler<GetApplicantBriefs, PagedResult<ApplicantBrief>>
    {
        private readonly ILogger<GetApplicantBriefsHandler> _logger;
        private readonly IUserAuthorization _userAuthorization;
        private readonly ILookupsCache _lookupsCache;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;

        public GetApplicantBriefsHandler(ILogger<GetApplicantBriefsHandler> logger, IUserAuthorization userAuthorization, ILookupsCache lookupsCache, IDomesticContext domesticContext, IApiMapper apiMapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
        }

        public async Task<PagedResult<ApplicantBrief>> Handle(GetApplicantBriefs request, CancellationToken cancellationToken)
        {
            var userType = _userAuthorization.GetUserType(request.User);
            var partnerCode = request.User.GetPartnerId();

            ValidatePaymentLocked(request.Params.PaymentLocked, request.User);
            await ValidateMident(request.Params.Mident, partnerCode, userType);

            var options = new Dto.GetApplicantBriefOptions
            {
                AccountNumber = request.Params.AccountNumber,
                ApplicationCycleId = request.Params.ApplicationCycleId,
                ApplicationNumber = request.Params.ApplicationNumber,
                ApplicationStatusId = request.Params.ApplicationStatusId,
                BirthDate = request.Params.BirthDate.ToNullableDateTime(),
                Email = request.Params.Email,
                FirstName = request.Params.FirstName,
                LastName = request.Params.LastName,
                MiddleName = request.Params.MiddleName,
                Mident = request.Params.Mident,
                OntarioEducationNumber = request.Params.OntarioEducationNumber,
                PaymentLocked = request.Params.PaymentLocked,
                PreviousLastName = request.Params.PreviousLastName,
                PageNumber = request.Params.Page,
                PageSize = request.Params.PageSize,
                PhoneNumber = request.Params.PhoneNumber,
                SortBy = (ApplicantBriefSortBy)request.Params.SortBy,
                SortDirection = request.Params.SortDirection == Enums.SortDirection.Ascending ? SortDirection.Asc : SortDirection.Desc
            };

            var dtoApplicantBriefs = await _domesticContext.GetApplicantBriefs(options, userType, partnerCode);

            return new PagedResult<ApplicantBrief>
            {
                TotalCount = dtoApplicantBriefs.TotalCount,
                Items = _apiMapper.MapApplicantBriefs(dtoApplicantBriefs.Items)
            };
        }

        private async Task ValidateMident(string mident, string partnerCode, UserType userType)
        {
            if (string.IsNullOrEmpty(mident)) return;

            switch (userType)
            {
                case UserType.OcasUser:
                case UserType.CollegeUser:
                    return;
                case UserType.HighSchoolUser when mident != partnerCode:
                    throw new ForbiddenException("User does not have access to mident.");
                case UserType.HighSchoolBoardUser:
                    var highSchools = await _lookupsCache.GetHighSchools(Constants.Localization.FallbackLocalization);
                    var boardHighSchools = highSchools.Where(h => h.BoardMident == partnerCode);
                    if (!boardHighSchools.Any(h => h.Mident == mident))
                        throw new ForbiddenException("User does not have access to mident.");

                    break;
            }
        }

        private void ValidatePaymentLocked(bool? paymentLocked, IPrincipal user)
        {
            if (paymentLocked.HasValue && !_userAuthorization.IsOcasTier2User(user))
                throw new ValidationException("User cannot filter by payment locked.");
        }
    }
}