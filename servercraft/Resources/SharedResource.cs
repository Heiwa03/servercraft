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
        
        // New resource strings
        public static string ServersDeployed => resourceManager.GetString("ServersDeployed");
        public static string SatisfiedClients => resourceManager.GetString("SatisfiedClients");
        public static string CountriesServed => resourceManager.GetString("CountriesServed");
        public static string Support => resourceManager.GetString("Support");
        public static string All => resourceManager.GetString("All");
        public static string Enterprise => resourceManager.GetString("Enterprise");
        public static string Cloud => resourceManager.GetString("Cloud");
        public static string Storage => resourceManager.GetString("Storage");
        public static string QuickView => resourceManager.GetString("QuickView");
        public static string AddToCart => resourceManager.GetString("AddToCart");
        public static string WhatOurClientsSay => resourceManager.GetString("WhatOurClientsSay");
        public static string Testimonial1 => resourceManager.GetString("Testimonial1");
        public static string ServerDetails => resourceManager.GetString("ServerDetails");
    }
} 