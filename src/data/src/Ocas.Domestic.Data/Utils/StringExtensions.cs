using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ocas.Domestic.Data
{
    internal static class StringExtensions
    {
        internal static string AsTitleCase(this string title)
        {
            var workingTitle = title;

            if (string.IsNullOrEmpty(workingTitle)) return workingTitle;
            const string pattern = @"\((.*?)\)";
            var bracketMatches = Regex.Matches(workingTitle, pattern).Cast<Match>().Select(x => x.Groups[1].Value).ToList();

            char[] space = { ' ' };

            var artsAndPreps = new List<string>
            {
                "a", "an", "and", "any", "at", "from", "into", "of", "on", "or", "some", "the", "to", "some", "as", "by", "but", "from", "into",
                "like", "onto", "off", "with", "within", "without", "up", "upon", "via", "de", "for", "in"
            };

            var textInfo = CultureInfo.CurrentCulture.TextInfo;

#pragma warning disable CA1308 // Normalize strings to uppercase
            workingTitle = textInfo.ToTitleCase(title.ToLowerInvariant());

            var tokens = workingTitle.Split(space, StringSplitOptions.RemoveEmptyEntries).ToList();

            workingTitle = tokens[0];

            tokens.RemoveAt(0);

            workingTitle += tokens.Aggregate(string.Empty, (prev, input)
                => prev +
                   (artsAndPreps.Contains(input.ToLowerInvariant())
                       ? " " + input.ToLowerInvariant()
                       : " " + input));
#pragma warning restore CA1308 // Normalize strings to uppercase

            var transformBracketMatches = Regex.Matches(workingTitle, pattern).Cast<Match>().Select(x => x.Groups[1].Value).ToList();

            var index = 0;
            foreach (var match in transformBracketMatches)
            {
                workingTitle = workingTitle.Replace(match, bracketMatches[index]);
                index++;
            }

            return workingTitle;
        }

        /// <summary>
        /// Takes the first N characters of a string.
        /// </summary>
        /// <param name="s">The string to truncate.</param>
        /// <param name="maxLength">The length to truncate to.</param>
        /// <returns>The truncated string.</returns>
        internal static string Truncate(this string s, int maxLength)
        {
            return string.IsNullOrEmpty(s) ? s : s.Substring(0, Math.Min(s.Length, maxLength));
        }

        /// <summary>
        /// Converts a string in yyyy-MM format to a utc datetime
        /// </summary>
        /// <param name="s">The string in YM format</param>
        /// <returns></returns>
        internal static DateTime? YearMonthToDateTime(this string s)
        {
            if (string.IsNullOrEmpty(s)) return null;

            return DateTime.ParseExact(s, "yyyy-MM", CultureInfo.InvariantCulture);
        }
    }
}
