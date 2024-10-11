using System;
using FluentValidation;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Services.Validators.Models
{
    public class PayOrderInfoValidator : AbstractValidator<PayOrderInfo>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1116:Split parameters must start on line after declaration", Justification = "Fluent validation style")]
        public PayOrderInfoValidator(IDomesticContext domesticContext, Guid orderId)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            WhenAsync(async (y, _) =>
            {
                var order = await domesticContext.GetOrder(orderId)
                    ?? throw new NotFoundException($"Order not found: {orderId}");
                return order.FinalTotal == 0;
            }, () =>
            {
                // no payment info is required for a zero dollar order
                // so we expect either a fully valid model or a completely empty
                RuleFor(x => x)
                    .Must(x => x.CardHolderName is null
                                && x.CardNumberToken is null
                                && x.Csc is null
                                && x.ExpiryDate is null)
                    .WithMessage("PayOrderInfo must be empty for zero dollar order");
            })
            .Otherwise(() =>
            {
                RuleFor(x => x.CardHolderName)
                    .NotEmpty()
                    .MaximumLength(40)
                    .Matches(Patterns.Name);

                RuleFor(x => x.CardNumberToken)
                    .NotEmpty()
                    .MaximumLength(100);

                RuleFor(x => x.Csc)
                    .NotEmpty()
                    .MinimumLength(3)
                    .MaximumLength(4)
                    .Matches(Patterns.MonerisCvd);

                RuleFor(x => x.ExpiryDate)
                    .NotEmpty()
                    .Length(4)
                    .Must(y => y.IsDate(Constants.DateFormat.CcExpiry))
                    .WithMessage(x => $"'{{PropertyName}}' is not a date: {x.ExpiryDate}");
            });
        }
    }
}
