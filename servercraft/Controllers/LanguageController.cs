using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace servercraft.Controllers
{
    public class LanguageController : Controller
    {
        public ActionResult Change(string culture)
        {
            if (culture != null)
            {
                var cultureInfo = new CultureInfo(culture);
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;

                var cookie = new HttpCookie("_culture", culture)
                {
                    Expires = DateTime.Now.AddYears(1)
                };
                Response.Cookies.Add(cookie);
            }

            return Redirect(Request.UrlReferrer?.ToString() ?? "~/");
        }
    }
} 