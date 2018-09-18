using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Infrastructure;
using Microsoft.Owin;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using Data.DAL;
using Data.Models;
using Service;

namespace Web
{
    public static class SimpleInjector
    {
        public static void Configure()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.Register<DatabaseContext>(Lifestyle.Scoped);

            container.Register<UserManager>(Lifestyle.Scoped);

            container.Register<RoleManager>(Lifestyle.Scoped);

            container.Register<ClaimedActionsProvider>(Lifestyle.Scoped);

            container.Register<SignInManager>(Lifestyle.Scoped);

            container.Register(() => 
                container.IsVerifying 
                    ? new OwinContext().Authentication 
                    : HttpContext.Current.GetOwinContext().Authentication, 
                Lifestyle.Scoped);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());


            container.RegisterMvcIntegratedFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}