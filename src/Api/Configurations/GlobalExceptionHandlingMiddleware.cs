using Api.Exceptions;
using System.Net;
using System.Text.Json;

namespace Api.Configurations
{
    public class GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
    
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var stackTrace = exception.StackTrace;
            var message = exception.Message;
            var exceptionType = exception.GetType();
            HttpStatusCode statusCode;
            if (exceptionType == typeof(Exceptions.UnauthorizedAccessException))
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(Exceptions.NotImplementedException))
            {
                statusCode = HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(NotFoundException))
            {
                statusCode = HttpStatusCode.NotFound;
            }
            else if (exceptionType == typeof(Exceptions.KeyNotFoundException))
            {
                statusCode = HttpStatusCode.NotFound;
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
            }
            var exceptionResult = JsonSerializer.Serialize(new { error = message, stackTrace });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(exceptionResult);
        }
    }
}
