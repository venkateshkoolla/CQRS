using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ocas.Common;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core;

namespace Ocas.Domestic.Apply.Api.Middlewares
{
    public class OcasExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public OcasExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<OcasExceptionHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Exception Occurred");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var result = new OcasHttpResponseMessage();

            var ocasMessageDetails = new OcasMessageDetails { Code = ErrorCodes.General.UnknownError, Message = exception.Message };
            var statusCode = HttpStatusCode.InternalServerError;

            // Check for specific error code in captured exception
            string codeOverride = null;
            var errorCodeExc = exception as IErrorCodeException;
            if (!string.IsNullOrWhiteSpace(errorCodeExc?.ErrorCode))
            {
                codeOverride = errorCodeExc.ErrorCode;
            }

            if (exception is ConcurrencyException)
            {
                statusCode = HttpStatusCode.Conflict;
                ocasMessageDetails.Code = codeOverride ?? ErrorCodes.General.ConcurrencyError;
                ocasMessageDetails.Message = "Optimistic concurrency check failed";
            }
            else if (exception is ConflictException)
            {
                statusCode = HttpStatusCode.Conflict;
                ocasMessageDetails.Code = codeOverride ?? ErrorCodes.General.ConflictError;
            }
            else if (exception is NotAuthorizedException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                ocasMessageDetails.Code = codeOverride ?? ErrorCodes.General.UnauthorizedError;
            }
            else if (exception is ForbiddenException)
            {
                statusCode = HttpStatusCode.Forbidden;
                ocasMessageDetails.Code = codeOverride ?? ErrorCodes.General.ForbiddenError;
            }
            else if (exception is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                ocasMessageDetails.Code = codeOverride ?? ErrorCodes.General.NotFoundError;
            }
            else if (exception is ValidationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                var code = codeOverride ?? ErrorCodes.General.ValidationError;
                result.Errors.AddRange(exception.Message.Split(new[] { "#--#" }, StringSplitOptions.RemoveEmptyEntries).Select(x => new OcasMessageDetails { Code = code, Message = x }));
            }

            //else if(exception is DbUpdateException)
            //{
            //    var ex = exception as DbUpdateException;
            //    if (ex.InnerException.Message.ToLower().Contains("duplicate")) // Might be different for different databases, this is just an example
            //    {
            //        statusCode = HttpStatusCode.Conflict;
            //        ocasMessageDetails.Code = ErrorCodes.General.ConflictError;
            //        ocasMessageDetails.Message = "Error updating database. Duplicate value.";
            //    }
            //    else
            //    {
            //        ocasMessageDetails.Message = "Error updating database.";
            //    }
            //}

            if (result.Errors.Count == 0 && result.Warnings.Count == 0)
                result.Errors.Add(ocasMessageDetails);

            result.OperationId = context.TraceIdentifier;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}
