using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using WebApi.Extensions;

namespace WebApi.Middleware
{
    public class SessionJwtValidationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!IsAnonymousAllowed(context)
                && context.Session.GetActiveJwt() != GetHeaderJwt(context))
            {
                throw new UnauthorizedException();
            }
            await next(context);
        }

        private bool IsAnonymousAllowed(HttpContext context)
        {
            return context.GetEndpoint()?.Metadata?.GetMetadata<IAllowAnonymous>() != null;
        }

        private string GetHeaderJwt(HttpContext context)
        {
            return context.Request.Headers.Authorization
                .ToString()
                .Split(' ')[1];
        }
    }

    public static class SessionJwtValidationMiddlewareExtensions
    {
        public static IServiceCollection AddSessionValidationMiddleware(this IServiceCollection services)
        {
            return services.AddSingleton<SessionJwtValidationMiddleware>();
        }

        public static IApplicationBuilder UseSessionJwtValidationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionJwtValidationMiddleware>();
        }
    }
}
