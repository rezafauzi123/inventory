using System.Globalization;

namespace InventoryManagement.WebApi.Common
{
    public class LocalizationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var cultureKey = context.Request.Headers["Accept-Language"];
            if (string.IsNullOrEmpty(cultureKey) || !DoesCultureExist(cultureKey!))
            {
                cultureKey = "en-US";
            }

            var culture = new System.Globalization.CultureInfo(cultureKey!);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            await next(context);
        }

        private static bool DoesCultureExist(string cultureName)
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures).Any(culture => string.Equals(culture.Name, cultureName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}