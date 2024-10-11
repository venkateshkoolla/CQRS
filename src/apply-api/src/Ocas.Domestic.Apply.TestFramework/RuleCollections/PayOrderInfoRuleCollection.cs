using System;
using Bogus;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.TestFramework.RuleCollections
{
    public static class PayOrderInfoRuleCollection
    {
        public static Faker<TPayOrderInfo> ApplyPayOrderInfoRules<TPayOrderInfo>(this Faker<TPayOrderInfo> faker)
            where TPayOrderInfo : PayOrderInfo
        {
            return faker
                .RuleSet(Constants.Payment.RuleSet.HostedTokenization, set =>
                {
                    set.RuleFor(x => x.CardHolderName, f => f.Name.FullName())
                        .RuleFor(x => x.Csc, f => f.Finance.CreditCardCvv())
                        .RuleFor(x => x.ExpiryDate, f => f.Date.Future(4, DateTime.UtcNow.AddMonths(1)).ToString(Constants.DateFormat.CcExpiry))
                        .RuleFor(x => x.CardNumberToken, f => "ot-" + f.Random.String(25));
                })
                .RuleSet(Constants.Payment.RuleSet.ZeroDollar, set =>
                {
                    set.RuleFor(x => x.CardHolderName, _ => null)
                        .RuleFor(x => x.Csc, _ => null)
                        .RuleFor(x => x.ExpiryDate, _ => null)
                        .RuleFor(x => x.CardNumberToken, _ => null);
                });
        }
    }
}