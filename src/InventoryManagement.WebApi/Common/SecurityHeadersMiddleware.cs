namespace InventoryManagement.WebApi.Common;

public sealed class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        context.Response.Headers.Append("X-Frame-Options", "DENY");

        context.Response.Headers.Append("X-Permitted-Cross-Domain-Policies", "none");

        context.Response.Headers.Append("X-Xss-Protection", "1; mode=block");

        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

        context.Response.Headers.Append("Referrer-Policy", "no-referrer");

        context.Response.Headers.Append("Permissions-Policy", "camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), usb=()");

        context.Response.Headers.Append("Content-Security-Policy",
            "default-src 'self'");

        return _next(context);
    }
}