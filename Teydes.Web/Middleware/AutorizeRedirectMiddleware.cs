using System.Net;

namespace Teydes.Web.Middleware;

public class AutorizeRedirectMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IApplicationBuilder _appBuilder;

    public AutorizeRedirectMiddleware(IApplicationBuilder appBuilder, RequestDelegate next)
    {
        _next = next;
        _appBuilder = appBuilder;
    }

    public Task InvokeAsync(HttpContext httpContext)
    {
        _appBuilder.UseStatusCodePages(async context =>
        {
            if ( context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                context.HttpContext.Response.Redirect("/accounts/login");
            }
        });
        return _next(httpContext);
    }
}
