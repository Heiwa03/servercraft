namespace eUseControl.BusinessLogic.Interfaces.Services
{
    public interface ILanguageService
    {
        void SetCulture(string culture);
        string GetCurrentCulture();
    }
} 