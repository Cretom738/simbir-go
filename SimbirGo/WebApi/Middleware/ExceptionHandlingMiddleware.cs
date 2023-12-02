using Application.Dtos;
using Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace WebApp.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ErrorCodeException e)
            {
                await HandleException(context, e);
            }
            catch (Exception e)
            {
                HandleException(context, e);
            }
        }

        private async Task HandleException(HttpContext context, ErrorCodeException exception)
        {
            context.Response.StatusCode = exception.ErrorCode;
            if (exception.Message != string.Empty)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDto
                {
                    Error = exception.Message
                }));
            }
        }

        private void HandleException(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IServiceCollection AddExceptionHandlingMiddleware(this IServiceCollection services)
        {
            return services.AddSingleton<ExceptionHandlingMiddleware>();
        }

        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
