using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OzonEdu.StockApi.Configuration.Middlewares
{
    public class VersionMiddleware
    {
        public VersionMiddleware(RequestDelegate next)
        {
            
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString() ?? "no version";
            var serviceName = Assembly.GetExecutingAssembly().GetName().Name;

            await context.Response.WriteAsJsonAsync(new
            {
                version = version,
                serviceName = serviceName
            });
        }
    }
}