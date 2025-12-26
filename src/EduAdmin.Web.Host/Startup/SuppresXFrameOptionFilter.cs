using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace EduAdmin.Web.Host.Startup
{
    public class SuppresXFrameOptionFilter : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context,ResultExecutionDelegate next)
        {
            context.HttpContext.Response.Headers.Remove("X-Frame-Options");

            await next();
        }
    }
}
