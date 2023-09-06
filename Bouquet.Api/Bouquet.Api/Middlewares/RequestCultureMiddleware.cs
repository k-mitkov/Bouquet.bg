using System.Globalization;
using System.Text.RegularExpressions;

namespace Bouquet.Api.Middlewares;

    public class RequestCultureMiddleware
    {
    private readonly RequestDelegate _next;

    public RequestCultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Sets Culture thread culture
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        try
        {

        // Extract the requested culture from the Accept-Language header
        var culture = context.Request.Headers["Accept-Language"].FirstOrDefault();

        if (!string.IsNullOrEmpty(culture)&& Regex.IsMatch(culture, @"^[a-z]{2}$"))
        {
            var cultureInfo = new CultureInfo(culture);
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }

        // Call the next middleware in the pipeline
        await _next(context);
        }catch(Exception ex) {
            var a = ex;
        }
    }
}

