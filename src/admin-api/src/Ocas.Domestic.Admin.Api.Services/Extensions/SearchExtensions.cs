namespace Ocas.Domestic.Apply.Admin.Api.Services.Extensions
{
    public static class SearchExtensions
    {
        public static(int skipRows, int takeRows) CalculateSkipTakeRows(this GetPageableOptions options, int defaultPageSize = 10, int maxPageSize = 100)
        {
            if (options?.Page.HasValue != true)
            {
                return (0, maxPageSize); // Don't page results
            }

            var takeRows = options.PageSize ?? defaultPageSize;

            if (takeRows <= 0)
            {
                takeRows = defaultPageSize;
            }
            else if (takeRows > maxPageSize)
            {
                takeRows = maxPageSize;
            }

            var page = options.Page ?? 1;

            if (page <= 0)
            {
                page = 1;
            }

            return ((page - 1) * takeRows, takeRows);
        }

        public static string TrimSearchString(this GetSearchableOptions searchableOptions)
        {
            return TrimSearchString(searchableOptions?.Search);
        }

        private static string TrimSearchString(string searchTerm)
        {
            searchTerm = searchTerm?.Trim(' ');

            return string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm;
        }
    }
}
