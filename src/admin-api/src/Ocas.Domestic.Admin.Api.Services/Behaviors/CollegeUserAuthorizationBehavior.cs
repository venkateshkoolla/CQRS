using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Behaviors
{
    public class CollegeUserAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICollegeUser
    {
        private readonly IUserAuthorization _userAuthorization;

        public CollegeUserAuthorizationBehavior(IUserAuthorization userAuthorization)
        {
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            await _userAuthorization.CanAccessCollegeAsync(request.User, request.CollegeId);

            return await next();
        }
    }
}
