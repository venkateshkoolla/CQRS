using System;
using AutoMapper;
using Ocas.Domestic.Apply.Core.Extensions;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public class DateTimeTypeConverter : ITypeConverter<DateTime, string>, ITypeConverter<DateTime?, string>
    {
        /// <summary>
        /// Converts a UTC DateTime to the Date (in Eastern Standard Time timezone) then to string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Convert(DateTime source, string destination, ResolutionContext context)
        {
            return source.ToStringOrDefault();
        }

        /// <summary>
        /// Converts a UTC DateTime to the Date (in Eastern Standard Time timezone) then to string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Convert(DateTime? source, string destination, ResolutionContext context)
        {
            return source.ToStringOrDefault();
        }
    }
}
