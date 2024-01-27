using gRPC_Time_Server.Attributes;
using System.Reflection;

namespace gRPC_Time_Server.Middleware
{
    /*
     * Middleware to check if a query is done through HTTP with TLS
     * 
     * Every call to a function will be checked for the custom attribute.
     */
    public class RequireHttpsMiddleware
    {
        #region Props
        private readonly RequestDelegate _next;
        #endregion

        #region Methods
        public RequireHttpsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var requireHttpsAttribute = endpoint?.Metadata.GetMetadata<RequireHttpsAttribute>();

            if (requireHttpsAttribute != null && !context.Request.IsHttps)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("HTTPS is required for this method.");
                return;
            }

            await _next(context);
        }
        #endregion
    }
}
