namespace ExcelReadApi.Middleware;

public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            string errorMessage = $"An error occured! Error message :  {exception.Message}";
            logger.LogError(exception, errorMessage);
 
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                Title = "Server Error",
                Status = context.Response.StatusCode,
                Message = errorMessage
            });
        }
    }
}