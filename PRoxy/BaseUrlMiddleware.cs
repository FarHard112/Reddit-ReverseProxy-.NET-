namespace PRoxy;

public class BaseUrlMiddleware
{
    private readonly RequestDelegate _next;

    public BaseUrlMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var baseUrl = $"{context.Request.Scheme}://{context.Request.Host}";
        context.Items["BaseUrl"] = baseUrl;
        await _next(context);
    }
}