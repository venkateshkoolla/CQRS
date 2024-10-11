using System;
using System.Globalization;

namespace Ocas.Domestic.Apply.Core.Extensions
{
    public static class DateTimeExtensions
    {
        private const string Format = "yyyy-MM-dd";

        public static bool IsDate(this string source)
        {
            return IsDate(source, Format);
        }

        public static bool IsDate(this string source, string format)
        {
            return DateTime.TryParseExact(source, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue);
        }

        public static int TotalMonths(this DateTime startDate)
        {
            return TotalMonths(startDate, DateTime.UtcNow);
        }

        public static int TotalMonths(this DateTime? startDate)
        {
            return TotalMonths(startDate, DateTime.UtcNow);
        }

        public static int TotalMonths(this DateTime? startDate, DateTime endDate)
        {
            if (startDate == null) return 0;

            return TotalMonths(startDate.Value, endDate);
        }

        public static int TotalMonths(this DateTime startDate, DateTime endDate)
        {
            return ((startDate.Year - endDate.Year) * 12) + startDate.Month - endDate.Month;
        }

        public static string ToStringOrDefault(this DateTime? source, string defaultValue, string format)
        {
            if (source == null) return defaultValue;

            return source.Value.ToDateInEstAsUtc().ToString(format);
        }

        public static string ToStringOrDefault(this DateTime source, string format = Format)
        {
            return ToStringOrDefault(source, null, format);
        }

        public static string ToStringOrDefault(this DateTime? source, string format = Format)
        {
            return ToStringOrDefault(source, null, format);
        }

        public static DateTime ToDateTime(this string source)
        {
            return ToDateTime(source, Format);
        }

        public static DateTime ToDateTime(this string source, string format)
        {
            if (DateTime.TryParseExact(source, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue))
            {
                var estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                return TimeZoneInfo.ConvertTimeToUtc(dateValue, estTimeZone);
            }

            throw new ArgumentException($"Failed to parse {source}, expected {Format}", nameof(source));
        }

        public static DateTime? ToNullableDateTime(this string source, string format = Format)
        {
            if (string.IsNullOrEmpty(source)) return null;

            return ToDateTime(source, format);
        }

        public static DateTime ToDateInEstAsUtc(this DateTime utcNow)
        {
            if (utcNow.Kind != DateTimeKind.Utc) throw new ArgumentException($"utcNow must be DateTimeKind.Utc but received: {utcNow.Kind}", nameof(utcNow));

            var estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var estNow = TimeZoneInfo.ConvertTime(utcNow, estTimeZone);

            return TimeZoneInfo.ConvertTimeToUtc(estNow.Date, estTimeZone);
        }
    }
}
