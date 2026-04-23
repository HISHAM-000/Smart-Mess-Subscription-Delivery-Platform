using MessMate.Application.Common.Exceptions;
using MessMate.Application.Common.Responses;
using FluentValidation;
using System.Net;
using System.Text.Json;
using System.Linq;

namespace MessMate.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var traceId = context.TraceIdentifier;

                _logger.LogError(ex, "TraceId: {TraceId} | Error: {Message}", traceId, ex.Message);

                await HandleExceptionAsync(context, ex, traceId);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, string traceId)
        {
            context.Response.ContentType = "application/json";

            int statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "An error occurred";
            IEnumerable<string>? errors = null;

            switch (exception)
            {
                case AlreadyExistsException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    message = exception.Message;
                    errors = new List<string> { exception.Message };
                    break;

                case ConflictException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    message = exception.Message;
                    errors = new List<string> { exception.Message };
                    break;

                case ForbiddenException:
                    statusCode = (int)HttpStatusCode.Forbidden;
                    message = exception.Message;
                    errors = new List<string> { exception.Message };
                    break;

                case NotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = exception.Message;
                    errors = new List<string> { exception.Message };
                    break;

                case BadRequestException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = exception.Message;
                    errors = new List<string> { exception.Message };
                    break;

                case UnauthorizedException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    message = exception.Message;
                    errors = new List<string> { exception.Message };
                    break;

                case ValidationException validationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "Validation failed";
                    errors = validationException.Errors
                        .Select(e => e.ErrorMessage);
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred";
                    errors = new List<string> { "Internal server error" };
                    break;
            }

            context.Response.StatusCode = statusCode;

            var apiResponse = ApiResponse<string>.FailureResponse(
                message,
                errors ?? new List<string> { "Internal server error" },
                traceId
            );

            var json = JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}