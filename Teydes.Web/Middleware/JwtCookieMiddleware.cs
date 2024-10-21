namespace Teydes.Web.Middleware;

public class JwtCookieMiddleware
{
    private readonly RequestDelegate _next;
    public JwtCookieMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Cookies.TryGetValue("X-Access-Token", out var accessToken))
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                string bearerToken = String.Format("Bearer {0}", accessToken);
                httpContext.Request.Headers.Add("Authorization", bearerToken);
            }
        }
        return _next(httpContext);
    }
}
