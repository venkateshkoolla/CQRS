using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Models
{
    public class IntlCredentialAssessmentValidator : AbstractValidator<IntlCredentialAssessment>
    {
        private static Regex IntlReferenceNumberRegex { get; } = new Regex(@"^(\d{6,8})$");

        public IntlCredentialAssessmentValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.IntlReferenceNumber)
                .MinimumLength(6)
                .MaximumLength(8)
                .Must(x => IntlReferenceNumberRegex.IsMatch(x))
                .WithMessage("'{PropertyName}' must be numeric")
                .When(x => !string.IsNullOrEmpty(x.IntlReferenceNumber));

            RuleFor(x => x.IntlEvaluatorId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var evaluators = await lookupsCache.GetCredentialEvaluationAgencies();
                    return evaluators.Any(t => t.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.IntlEvaluatorId}")
                .When(x => !string.IsNullOrEmpty(x.IntlReferenceNumber));
        }
    }
}
