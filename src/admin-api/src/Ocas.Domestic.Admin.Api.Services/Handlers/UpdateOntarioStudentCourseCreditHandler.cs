using System;
using System.Globalization;
using System.Linq;
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
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class UpdateOntarioStudentCourseCreditHandler : IRequestHandler<UpdateOntarioStudentCourseCredit, OntarioStudentCourseCredit>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IDtoMapper _dtoMapper;
        private readonly IApiMapper _apiMapper;
        private readonly string _locale;

        public UpdateOntarioStudentCourseCreditHandler(ILogger<UpdateOntarioStudentCourseCreditHandler> logger, IDomesticContext domesticContext, IDtoMapper dtoMapper, IUserAuthorization userAuthorization, IApiMapper apiMapper, RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _dtoMapper = dtoMapper ?? throw new ArgumentNullException(nameof(dtoMapper));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<OntarioStudentCourseCredit> Handle(UpdateOntarioStudentCourseCredit request, CancellationToken cancellationToken)
        {
            if (!_userAuthorization.IsOcasTier2User(request.User) && !_userAuthorization.IsHighSchoolUser(request.User))
            {
                throw new ForbiddenException();
            }

            var ontarioStudentCourseCredit = await _domesticContext.GetOntarioStudentCourseCredit(request.OntarioStudentCourseCreditId)
                ?? throw new NotFoundException("OntarioStudentCourseCredit does not exist");

            if (ontarioStudentCourseCredit.ApplicantId != request.ApplicantId)
                throw new ValidationException($"'Ontario Student Course Credit' does not belong to applicant: {request.ApplicantId}");

            // No duplicates allowed
            var ontarioStudentCourseCredits = await _domesticContext.GetOntarioStudentCourseCredits(new Dto.GetOntarioStudentCourseCreditOptions { ApplicantId = request.ApplicantId });
            if (ontarioStudentCourseCredits.Any(x => x.CourseCode == request.OntarioStudentCourseCredit.CourseCode && x.CompletedDate == request.OntarioStudentCourseCredit.CompletedDate && x.Id != ontarioStudentCourseCredit.Id))
            {
                throw new ValidationException($"OntarioStudentCourseCredit.CourseCode already exists for this Applicant: {request.OntarioStudentCourseCredit.CourseCode}");
            }

            _dtoMapper.PatchOntarioStudentCourseCredit(ontarioStudentCourseCredit, request.OntarioStudentCourseCredit, request.User.GetUpnOrEmail());

            var result = await _domesticContext.UpdateOntarioStudentCourseCredit(ontarioStudentCourseCredit);

            return _apiMapper.MapOntarioStudentCourseCredit(result);
        }
    }
}