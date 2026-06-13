namespace BankApi.Common;

public static class HttpContextExtensions
{
    public static string GetUserLanguage(this HttpContext context)
    {
        var lang = context.Request.Headers["Accept-Language"].ToString().ToLower().Split(',')[0].Trim();
        
        return lang is "ru" or "en" or "ky" ? lang : "ru";
    }
}