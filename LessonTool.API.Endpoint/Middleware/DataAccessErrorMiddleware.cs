namespace LessonTool.API.Endpoint.Middleware;

public class DataAccessErrorMiddleware(RequestDelegate request)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await request(context);
        }
        catch (Exception ex)
        {
#if DEBUG
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(string.Join("\n\n", "Server failed to access database:", ex.Message, ex.InnerException is not null ? ex.InnerException.Message : ""));
#else
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("The server has encountered a critical error.");
#endif
        }
    }
}
