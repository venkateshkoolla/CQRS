using System;
using System.Globalization;

namespace Ocas.Domestic.Data.TestFramework.Extensions
{
    public static class DateTimeExtensions
    {
        private const string Format = "yyyy-MM-dd";

        public static string ToStringOrDefault(this DateTime? source, string defaultValue)
        {
            if (source == null) return defaultValue;

            return source.Value.ToDateInEstAsUtc().ToString(Format);
        }

        public static string ToStringOrDefault(this DateTime source)
        {
            return ToStringOrDefault(source, null);
        }

        public static string ToStringOrDefault(this DateTime? source)
        {
            return ToStringOrDefault(source, null);
        }

        public static bool IsDate(this string source)
        {
            return DateTime.TryParseExact(source, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue);
        }

        public static DateTime ToDateTime(this string source, string format = Format)
        {
            if (DateTime.TryParseExact(source, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue))
            {
                var estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                return TimeZoneInfo.ConvertTimeToUtc(dateValue, estTimeZone);
            }

            throw new ArgumentException($"Failed to parse {source}, expected {format}", nameof(source));
        }

        public static DateTime ToDateInEstAsUtc(this DateTime utcNow)
        {
            if (utcNow.Kind != DateTimeKind.Utc) throw new ArgumentException($"utcNow must be DateTimeKind.Utc but received: {utcNow.Kind}", nameof(utcNow));

            var estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var estNow = TimeZoneInfo.ConvertTime(utcNow, estTimeZone);

            return TimeZoneInfo.ConvertTimeToUtc(estNow.Date, estTimeZone);
        }

        public static DateTime? YearMonthToDateTime(this string s)
        {
            if (string.IsNullOrEmpty(s)) return null;

            return DateTime.ParseExact(s, "yyyy-MM", CultureInfo.InvariantCulture);
        }
    }
}
