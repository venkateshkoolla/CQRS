using FluentValidation;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class GetMcuCodesValidator : AbstractValidator<GetMcuCodes>
    {
        public GetMcuCodesValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(v => v.Params.Search)
                .MaximumLength(100)
                .When(y => !string.IsNullOrWhiteSpace(y.Params.Search));
        }
    }
}
