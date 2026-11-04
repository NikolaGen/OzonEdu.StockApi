using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OzonEdu.StockApi.Configuration.Middlewares
{
    public class ReadyLiveMiddleware
    {
        public ReadyLiveMiddleware(RequestDelegate next)
        {
            
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsync($"{StatusCodes.Status200OK} OK");
        }
    }
}