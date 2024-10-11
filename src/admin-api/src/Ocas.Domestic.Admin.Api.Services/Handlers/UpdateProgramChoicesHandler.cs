using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class UpdateProgramChoicesHandler : IRequestHandler<UpdateProgramChoice, ProgramChoice>
    {
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IApiMapper _apiMapper;

        public UpdateProgramChoicesHandler(IDomesticContext domesticContext, IUserAuthorization userAuthorization, IApiMapper apiMapper)
        {
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
        }

        public async Task<ProgramChoice> Handle(UpdateProgramChoice request, CancellationToken cancellationToken)
        {
            if (_userAuthorization.GetUserType(request.User) != UserType.OcasUser)
                throw new ForbiddenException();

            var dtoProgramChoice = await _domesticContext.GetProgramChoice(request.ProgramChoiceId) ??
                throw new NotFoundException($"Program choice {request.ProgramChoiceId} not found");

            dtoProgramChoice.EffectiveDate = request.EffectiveDate.ToDateTime();

            var result = await _domesticContext.UpdateProgramChoice(dtoProgramChoice);
            return _apiMapper.MapProgramChoice(result);
        }
    }
}
