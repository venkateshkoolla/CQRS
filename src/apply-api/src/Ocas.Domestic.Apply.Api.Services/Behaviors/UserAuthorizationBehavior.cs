using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Behaviors
{
    public class UserAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IApplicantUser
    {
        private readonly IUserAuthorization _userAuthorization;

        public UserAuthorizationBehavior(IUserAuthorization userAuthorization)
        {
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            await _userAuthorization.CanAccessApplicantAsync(request.User, request.ApplicantId);

            return await next();
        }
    }
}
