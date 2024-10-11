using System;
using System.Linq;
using Bogus;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.TestFramework.RuleCollections
{
    public static class OrderRuleCollection
    {
        public static Faker<TOrder> ApplyOrderRules<TOrder>(this Faker<TOrder> faker)
            where TOrder : Order
        {
            return faker
                .RuleFor(o => o.Id, _ => Guid.NewGuid())
                .RuleFor(o => o.Number, f => f.Random.AlphaNumeric(9))
                .RuleFor(o => o.Details, _ => new Faker<OrderDetail>().ApplyOrderDetailRules().Generate(2))
                .RuleFor(o => o.Amount, (_, o) => o.Details.ToList().Sum(d => d.Amount));
        }

        public static Faker<TOrderDetail> ApplyOrderDetailRules<TOrderDetail>(this Faker<TOrderDetail> faker)
            where TOrderDetail : OrderDetail
        {
            return faker
                .RuleFor(o => o.Type, f => f.PickRandom<ShoppingCartItemType>())
                .RuleFor(o => o.ContextId, (f, o) => o.Type == ShoppingCartItemType.Voucher ? f.Name.JobDescriptor() : Guid.NewGuid().ToString())
                .RuleFor(o => o.Amount, f => f.Finance.Amount());
        }
    }
}
