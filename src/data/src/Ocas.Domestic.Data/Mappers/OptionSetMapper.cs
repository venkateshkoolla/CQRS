using System;
using Microsoft.Xrm.Sdk;

namespace Ocas.Domestic.Data.Mappers
{
    public static class OptionSetMapper
    {
        public static OptionSetValue ToOptionSet<TEnum>(this int source)
            where TEnum : struct
        {
            var type = typeof(TEnum);

            if (!type.IsEnum)
            {
                throw new ArgumentException($"{type} is not an enum.");
            }

            var value = Convert.ToInt32(source.ToEnum<TEnum>());
            return new OptionSetValue(value);
        }

        public static OptionSetValue ToOptionSet<TEnum>(this int? source)
            where TEnum : struct
        {
            return source?.ToOptionSet<TEnum>();
        }
    }
}
