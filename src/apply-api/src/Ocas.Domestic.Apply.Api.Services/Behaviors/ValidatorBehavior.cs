using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Ocas.Domestic.Apply.Api.Services.Behaviors
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidatorBehavior(
            IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            // Validation
            var context = new ValidationContext(request);

            var tasks = _validators.Select(v => v.ValidateAsync(context, cancellationToken));
            var results = await Task.WhenAll(tasks);

            var failures = results
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count > 0)
                throw new Ocas.Common.Exceptions.ValidationException(string.Join("#--#", failures.Select(x => x.ErrorMessage)));

            return await next();
        }
    }
}
