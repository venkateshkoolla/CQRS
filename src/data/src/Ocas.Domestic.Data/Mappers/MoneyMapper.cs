using Microsoft.Xrm.Sdk;

namespace Ocas.Domestic.Data.Mappers
{
    public static class MoneyMapper
    {
        public static Money ToMoney(this decimal source)
        {
            return new Money(source);
        }

        public static Money ToMoney(this decimal? source)
        {
            return source.HasValue ? new Money(source.Value) : null;
        }
    }
}
