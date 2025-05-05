using System.Resources;

namespace servercraft.Resources
{
    public class SharedResource
    {
        private static ResourceManager resourceManager = new ResourceManager("servercraft.Resources.SharedResource", typeof(SharedResource).Assembly);

        public static string AppName => resourceManager.GetString("AppName");
        public static string Home => resourceManager.GetString("Home");
        public static string About => resourceManager.GetString("About");
        public static string Contact => resourceManager.GetString("Contact");
        public static string Servers => resourceManager.GetString("Servers");
        public static string Cart => resourceManager.GetString("Cart");
        public static string Login => resourceManager.GetString("Login");
        public static string LogIn => resourceManager.GetString("LogIn");
        public static string Register => resourceManager.GetString("Register");
        public static string Logout => resourceManager.GetString("Logout");
        public static string LogOff => resourceManager.GetString("LogOff");
        public static string Hello => resourceManager.GetString("Hello");
        public static string EnterpriseServerSolutions => resourceManager.GetString("EnterpriseServerSolutions");
        public static string EnterpriseServerSolutionsMessage => resourceManager.GetString("EnterpriseServerSolutionsMessage");
        public static string SearchForServers => resourceManager.GetString("SearchForServers");
        public static string Search => resourceManager.GetString("Search");
        public static string FeaturedServers => resourceManager.GetString("FeaturedServers");
        public static string EnterpriseSupport => resourceManager.GetString("EnterpriseSupport");
        public static string EnterpriseSupportMessage => resourceManager.GetString("EnterpriseSupportMessage");
        public static string CustomSolutions => resourceManager.GetString("CustomSolutions");
        public static string CustomSolutionsMessage => resourceManager.GetString("CustomSolutionsMessage");
        public static string WarrantyAndService => resourceManager.GetString("WarrantyAndService");
        public static string WarrantyAndServiceMessage => resourceManager.GetString("WarrantyAndServiceMessage");
    }
} 