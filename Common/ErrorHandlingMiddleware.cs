using System.Net;
using System.Text.Json;

namespace BankApi.Common;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (AppException ex)
        {
            await HandleAppExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleSystemExceptionAsync(context, ex);
        }
    }

    private static async Task HandleAppExceptionAsync(HttpContext context, AppException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)ex.StatusCode; // берём из исключения

        var lang = context.GetUserLanguage();
        string translatedText = ErrorMessages.Get(ex.ErrorCode, lang, ex.Args ?? []);

        var response = new { code = ex.ErrorCode, message = translatedText };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static async Task HandleSystemExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var lang = context.GetUserLanguage();

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            code = ErrorCodes.InternalServerError,
            message = ErrorMessages.Get(ErrorCodes.InternalServerError, lang)
        }));
    }
}