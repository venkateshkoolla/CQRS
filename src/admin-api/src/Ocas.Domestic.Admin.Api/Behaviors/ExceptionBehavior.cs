using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ocas.Common.Exceptions;

namespace Ocas.Domestic.Apply.Admin.Api
{
    public class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception e) when (!(e is ExceptionBase))
            {
                // This ensures any error coming out of our Mediator pipeline is in our custom exception
                throw new UnhandledException("Unhandled Exception In Mediator Pipeline", e);
            }
        }
    }
}
