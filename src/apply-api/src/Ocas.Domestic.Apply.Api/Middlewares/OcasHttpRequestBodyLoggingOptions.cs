using System.Collections.Generic;

namespace Ocas.Domestic.Apply.Api
{
    /// <summary>
    /// Options for configuring the <see cref="OcasHttpRequestBodyLoggingMiddleware"/> class.
    /// </summary>
    public sealed class OcasHttpRequestBodyLoggingOptions
    {
        /// <summary>
        /// Gets or sets the maximum number of bytes from the request to persist to durable storage.
        /// </summary>
        public int? MaximumRecordedRequestLength { get; set; }

        /// <summary>
        /// Gets or sets will exclude logging completely if the path contains any of these strings in the array
        /// </summary>
        public ICollection<string> ExclusionPaths { get; set; }
    }
}
