using System;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class GetApplicantHistoryOptionsValidator : AbstractValidator<GetApplicantHistoryOptions>
    {
        public GetApplicantHistoryOptionsValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.FromDate)
                .Must(x => x.IsDate())
                .Must(x => x.ToDateTime() <= DateTime.UtcNow)
                .WithMessage("'{PropertyName}' can not be a future date")
                .When(x => !string.IsNullOrEmpty(x.FromDate));

            RuleFor(x => x.ToDate)
                .Must(x => x.IsDate())
                .Must(x => x.ToDateTime() <= DateTime.UtcNow)
                .WithMessage("'{PropertyName}' can not be a future date")
                .When(x => !string.IsNullOrEmpty(x.ToDate));

            RuleFor(x => x)
                .Must(x => x.FromDate.ToDateTime() <= x.ToDate.ToDateTime())
                .WithMessage(y => $"'From Date' {y.FromDate} can not be greater than 'To Date' {y.ToDate}")
                .When(x => !string.IsNullOrEmpty(x.FromDate) && !string.IsNullOrEmpty(x.ToDate));
        }
    }
}
