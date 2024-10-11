using System;
using AutoMapper;
using Ocas.Domestic.Apply.Core.Extensions;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public class UtcStringTypeConverter : ITypeConverter<string, DateTime>, ITypeConverter<string, DateTime?>
    {
        /// <summary>
        /// Converts a date-formatted string yyyy-MM-dd from Eastern Standard Time timezone to UTC DateTime
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {
            return source.ToDateTime();
        }

        /// <summary>
        /// Converts a date-formatted string yyyy-MM-dd from Eastern Standard Time timezone to UTC DateTime
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public DateTime? Convert(string source, DateTime? destination, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source)) return null;

            return source.ToDateTime();
        }
    }
}
