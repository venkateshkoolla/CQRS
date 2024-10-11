using FluentValidation;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class GetMcuCodeValidator : AbstractValidator<GetMcuCode>
    {
        public GetMcuCodeValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(v => v.McuCode)
                .NotEmpty()
                .Length(5)
                .Unless(v => v.McuCode == "0"); //Unknown code is only one char
        }
    }
}