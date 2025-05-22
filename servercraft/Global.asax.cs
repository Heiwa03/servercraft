// Global.asax.cs
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using Servercraft.Domain.Entities;
using Servercraft.Data.Initializers;
using Servercraft.Data.Context;
using Servercraft.Domain.Repositories;

namespace servercraft
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);

            // Initialize database
            System.Data.Entity.Database.SetInitializer(new ServerMarketInitializer());
            using (var context = new ServerMarketContext())
            {
                context.Database.Initialize(true);
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
            {
                var cultureInfo = new CultureInfo(cultureCookie.Value);
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }
            else
            {
                var userLanguages = Request.UserLanguages;
                var userCulture = userLanguages != null && userLanguages.Length > 0
                    ? userLanguages[0]
                    : "en";

                if (userCulture.StartsWith("ro", StringComparison.OrdinalIgnoreCase))
                {
                    userCulture = "ro";
                }
                else if (userCulture.StartsWith("ru", StringComparison.OrdinalIgnoreCase))
                {
                    userCulture = "ru";
                }
                else
                {
                    userCulture = "en";
                }

                var cultureInfo = new CultureInfo(userCulture);
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;

                cultureCookie = new HttpCookie("_culture", userCulture)
                {
                    Expires = DateTime.Now.AddYears(1)
                };
                Response.Cookies.Add(cultureCookie);
            }
        }
    }
}