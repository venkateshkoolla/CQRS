using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class UpdateApplicantInfoHandler : IRequestHandler<UpdateApplicantInfo, ApplicantUpdateInfo>
    {
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IDtoMapper _dtoMapper;

        public UpdateApplicantInfoHandler(IDomesticContext domesticContext, IDtoMapper dtoMapper, IUserAuthorization userAuthorization, IApiMapper apiMapper)
        {
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _dtoMapper = dtoMapper ?? throw new ArgumentNullException(nameof(dtoMapper));
        }

        public async Task<ApplicantUpdateInfo> Handle(UpdateApplicantInfo request, CancellationToken cancellationToken)
        {
            var userType = _userAuthorization.GetUserType(request.User);
            if (userType != UserType.OcasUser) throw new ForbiddenException();

            var current = await _domesticContext.GetContact(request.ApplicantId) ??
                throw new NotFoundException($"Applicant {request.ApplicantId} not found");

            if (current.FirstName == request.ApplicantUpdateInfo.FirstName && current.LastName == request.ApplicantUpdateInfo.LastName && current.BirthDate == request.ApplicantUpdateInfo.BirthDate.ToDateTime())
            {
                // Nothing got changed!
                return _apiMapper.MapApplicantUpdateInfo(current);
            }

            var isDuplicate = await _domesticContext.IsDuplicateContact(current.Id, request.ApplicantUpdateInfo.FirstName, request.ApplicantUpdateInfo.LastName, request.ApplicantUpdateInfo.BirthDate.ToDateTime());
            if (isDuplicate) throw new ConflictException($"Applicant already exists with info: {request.ApplicantUpdateInfo.FirstName} {request.ApplicantUpdateInfo.LastName}, {request.ApplicantUpdateInfo.BirthDate}");

            _dtoMapper.PatchApplicantUpdateInfo(current, request.ApplicantUpdateInfo);
            current.ModifiedBy = request.User.GetUpnOrEmail();

            var result = await _domesticContext.UpdateContact(current);

            return _apiMapper.MapApplicantUpdateInfo(result);
        }
    }
}
