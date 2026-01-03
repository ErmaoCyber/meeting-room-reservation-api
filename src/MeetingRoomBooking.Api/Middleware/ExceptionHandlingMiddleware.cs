using System.Text.Json;
using MeetingRoomBooking.Api.Errors;

namespace MeetingRoomBooking.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // 客户端取消请求（常见于刷新/断网），通常不当作服务端错误
            context.Response.StatusCode = StatusCodes.Status499ClientClosedRequest; // 非标准但常用；如果你不想用 499，可改 400
        }
        catch (Exception ex)
        {
            var traceId = context.TraceIdentifier;

            _logger.LogError(ex,
                "Unhandled exception. TraceId={TraceId} Path={Path} Method={Method}",
                traceId, context.Request.Path, context.Request.Method);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // 生产环境别把异常细节返回给客户端
            var message = context.RequestServices
                .GetRequiredService<IHostEnvironment>()
                .IsDevelopment()
                ? ex.Message
                : "An unexpected error occurred.";

            var payload = new ApiErrorResponse(
                Code: "UnhandledError",
                Message: message,
                TraceId: traceId
            );

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}
