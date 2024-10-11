using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetOntarioHighSchoolCourseCodeHandler : IRequestHandler<GetOntarioHighSchoolCourseCode, OntarioHighSchoolCourseCode>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IApiMapper _apiMapper;

        public GetOntarioHighSchoolCourseCodeHandler(ILogger<GetOntarioHighSchoolCourseCodeHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization, IApiMapper apiMapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
        }

        public async Task<OntarioHighSchoolCourseCode> Handle(GetOntarioHighSchoolCourseCode request, CancellationToken cancellationToken)
        {
            var courseCode = await _domesticContext.GetOntarioHighSchoolCourseCode(request.Code) ??
                            throw new NotFoundException($"No course code found with code: {request.Code}");

            return _apiMapper.MapOntarioHighSchoolCourseCode(courseCode);
        }
    }
}
