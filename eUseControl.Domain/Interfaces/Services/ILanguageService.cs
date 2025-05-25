namespace eUseControl.Domain.Interfaces.Services
{
    public interface ILanguageService
    {
        void SetCulture(string culture);
        string GetCurrentCulture();
    }
} 