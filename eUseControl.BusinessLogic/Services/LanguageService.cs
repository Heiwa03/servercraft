using System;
using System.Globalization;
using System.Threading;
using System.Web;
using eUseControl.Domain.Interfaces.Services;

namespace eUseControl.BusinessLogic.Services
{
    public class LanguageService : ILanguageService
    {
        public void SetCulture(string culture)
        {
            if (string.IsNullOrEmpty(culture))
            {
                return;
            }

            var cultureInfo = new CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var cookie = new HttpCookie("_culture", culture)
            {
                Expires = DateTime.Now.AddYears(1)
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public string GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentUICulture.Name;
        }
    }
} 