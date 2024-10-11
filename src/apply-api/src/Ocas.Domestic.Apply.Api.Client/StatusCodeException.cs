using System;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using Ocas.Common;

namespace Ocas.Domestic.Apply.Api.Client
{
    [Serializable]
    public class StatusCodeException : Exception
    {
        public StatusCodeException(HttpStatusCode statusCode)
            : this(statusCode, null, $"Status of {statusCode.ToString()} code {((int)statusCode).ToString()}")
        {
        }

        public StatusCodeException(HttpStatusCode statusCode, OcasHttpResponseMessage ocasHttpResponseMessage)
            : this(statusCode, ocasHttpResponseMessage, FormatMessage(statusCode, ocasHttpResponseMessage))
        {
        }

        public StatusCodeException(HttpStatusCode statusCode, OcasHttpResponseMessage ocasHttpResponseMessage, string message)
            : base(message)
        {
            StatusCode = statusCode;
            OcasHttpResponseMessage = ocasHttpResponseMessage;
        }

        public StatusCodeException(HttpStatusCode statusCode, OcasHttpResponseMessage ocasHttpResponseMessage, string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            OcasHttpResponseMessage = ocasHttpResponseMessage;
        }

        public StatusCodeException()
        {
        }

        public StatusCodeException(string message)
            : base(message)
        {
        }

        public StatusCodeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected StatusCodeException(HttpStatusCode statusCode, OcasHttpResponseMessage ocasHttpResponseMessage, SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            StatusCode = statusCode;
            OcasHttpResponseMessage = ocasHttpResponseMessage;
        }

        protected StatusCodeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning disable CA2235 // Mark all non-serializable fields
        public HttpStatusCode StatusCode { get; set; }
        public OcasHttpResponseMessage OcasHttpResponseMessage { get; set; }
#pragma warning restore CA2235 // Mark all non-serializable fields

        private static string FormatMessage(HttpStatusCode statusCode, OcasHttpResponseMessage ocasHttpResponseMessage)
        {
            var sb = new StringBuilder();
            sb.AppendLine()
                .AppendLine("==============================================================")
                .Append("Status of ").Append(statusCode.ToString()).Append(" code ").AppendLine(((int)statusCode).ToString());

            if (ocasHttpResponseMessage != null)
            {
                sb.AppendLine("OcasHttpResponseMessage | Level   | Code  | Message");
                foreach (var details in ocasHttpResponseMessage.Errors)
                {
                    sb.Append("OcasHttpResponseMessage | Error   | ").Append(details.Code).Append(" | ").AppendLine(details.Message);
                }

                foreach (var details in ocasHttpResponseMessage.Warnings)
                {
                    sb.Append("OcasHttpResponseMessage | Warning | ").Append(details.Code).Append(" | ").AppendLine(details.Message);
                }
            }

            sb.AppendLine("==============================================================");
            return sb.ToString();
        }
    }
}