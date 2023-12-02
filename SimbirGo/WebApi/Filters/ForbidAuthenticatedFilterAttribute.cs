using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Filters
{
    public class ForbidAuthenticatedFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (IsAuthenticated(context))
            {
                context.Result = new ForbidResult(JwtBearerDefaults.AuthenticationScheme);
            }
            return Task.CompletedTask;
        }

        private bool IsAuthenticated(AuthorizationFilterContext context)
        {
            return context.HttpContext.User.Identity?.IsAuthenticated ?? false;
        }
    }
}
