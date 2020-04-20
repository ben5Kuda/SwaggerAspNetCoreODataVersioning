using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SwaggerAspCoreOData.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : System.Attribute, IAsyncActionFilter
    {
        // The name of the header key that should contain the Api Key
        private const string ApiKeyHeaderName = "X-Api-Key";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
            {
                // No Api Key Supplied. Request not Authorized 
                context.Result = new UnauthorizedResult();
                return;
            }

            // Get the list of allowed Api Keys from appsettings.json
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKeys = configuration.GetSection("ApiKeys").Get<List<string>>();

            if (!apiKeys.Contains(potentialApiKey))
            {
                // Supplied key is not valid. Request not Authorized
                context.Result = new UnauthorizedResult();
                return;
            }

            // Request Authorized. Proceed to the controller.
            await next();
        }
    }
}
