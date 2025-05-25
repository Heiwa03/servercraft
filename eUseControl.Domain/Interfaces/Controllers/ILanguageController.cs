using System.Web.Mvc;

namespace eUseControl.Domain.Interfaces.Controllers
{
    public interface ILanguageController
    {
        ActionResult Change(string culture);
    }
} 