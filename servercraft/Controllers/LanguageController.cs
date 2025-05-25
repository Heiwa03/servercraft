using System;
using System.Web.Mvc;
using eUseControl.BusinessLogic.Services;
using eUseControl.Domain.Interfaces.Controllers;
using eUseControl.Domain.Interfaces.Services;

namespace servercraft.Controllers
{
    public class LanguageController : Controller, ILanguageController
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        public ActionResult Change(string culture)
        {
            _languageService.SetCulture(culture);
            return Redirect(Request.UrlReferrer?.ToString() ?? "~/");
        }
    }
} 