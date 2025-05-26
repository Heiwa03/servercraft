using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;
using eUseControl.BusinessLogic.Services;
using eUseControl.BusinessLogic.Interfaces.Services;
using servercraft.Models.Repositories;
using servercraft.Models;

namespace servercraft
{
    public static class DependencyConfig
    {
        public static void RegisterDependencies()
        {
            var container = new UnityContainer();

            // Register DbContext
            container.RegisterType<ServerMarketContext>(new HierarchicalLifetimeManager());

            // Register Unit of Work
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());

            // Register Services
            container.RegisterType<IAuthService, AuthService>();
            container.RegisterType<ICartService, CartService>();
            container.RegisterType<IHomeService, HomeService>();
            container.RegisterType<ILanguageService, LanguageService>();
            container.RegisterType<IProductService, ProductService>();

            // Set the dependency resolver
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
} 