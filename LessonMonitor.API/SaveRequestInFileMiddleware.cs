using Microsoft.Extensions.Primitives;

public class SaveRequestInFileMiddleware
{
    private readonly RequestDelegate _next;
    private const string CUSTOM_HEADER = "postdata";

    public SaveRequestInFileMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var customHeader = context.Request.Headers.SingleOrDefault(header => header.Key == CUSTOM_HEADER);

        if (customHeader.Key == null)
        {
            if(context.Request.Method == "POST") 
            {
                context.Response.StatusCode = 200;
            }
            else
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync($"Отсутствует кастомный хедер: {CUSTOM_HEADER}");
            }
        }
        else
        {
            _next?.Invoke(context);
        }
    }
}
