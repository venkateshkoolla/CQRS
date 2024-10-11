using System;
using Bogus;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.TestFramework.RuleCollections
{
    public static class VoucherRuleCollection
    {
        private const string AllowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static Faker<TVoucher> ApplyVoucherRules<TVoucher>(this Faker<TVoucher> faker)
            where TVoucher : Dto.Voucher
        {
            return faker
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.ApplicantId, Guid.NewGuid())
                .RuleFor(x => x.ApplicationId, Guid.NewGuid())
                .RuleFor(x => x.Code, (f, _) => f.Random.String2(10, AllowedChars))
                .RuleFor(x => x.VoucherState, VoucherState.Issued)
                .RuleFor(x => x.ProductId, Guid.NewGuid());
        }
    }
}
