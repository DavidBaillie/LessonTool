namespace LessonTool.API.Endpoint.Middleware;

public class InternalServerErrorMiddleware(RequestDelegate request)
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
            await context.Response.WriteAsync(ex.Message);
#else
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("The server has encountered a critical error.");
#endif
        }
    }
}
