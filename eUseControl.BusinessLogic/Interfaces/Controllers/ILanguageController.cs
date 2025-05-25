using System.Web.Mvc;

namespace eUseControl.BusinessLogic.Interfaces.Controllers
{
    public interface ILanguageController
    {
        ActionResult Change(string culture);
    }
} 