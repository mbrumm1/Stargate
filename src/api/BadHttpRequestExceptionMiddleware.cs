using StargateAPI.Controllers;

namespace StargateAPI;

public class BadHttpRequestExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<BadHttpRequestExceptionMiddleware> _logger;

    public BadHttpRequestExceptionMiddleware(RequestDelegate next, ILogger<BadHttpRequestExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BadHttpRequestException e)
        {
            _logger.LogError(e.Message);
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(BaseResponse.InternalServerError(e.Message));
        }
    }
}
