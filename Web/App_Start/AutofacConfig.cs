using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Data.DAL;
using Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Service;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Web.Infrastructure;

namespace Web
{
    public class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            //bạn sẽ khai báo các class và interface tương ứng ở đây
            builder.RegisterControllers(Assembly.GetExecutingAssembly()); //Register MVC Controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()); //Register WebApi Controllers
            // MVC - OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();
            builder.RegisterModule(new AutofacWebTypesModule());

            builder.RegisterType<DatabaseContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<RoleManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<SignInManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ClaimedActionsProvider>().AsSelf().InstancePerRequest();
            builder.Register(c => new UserStore<ApplicationUser>(c.Resolve<DatabaseContext>())).AsImplementedInterfaces().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).As<IAuthenticationManager>();
            builder.Register(c => new IdentityFactoryOptions<UserManager>
            {
                DataProtectionProvider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("Application​")
            });
            builder.RegisterType<ConfigServices>().As<IConfigServices>().InstancePerDependency();
            builder.RegisterType<MenuServices>().As<IMenuServices>().InstancePerDependency();
            builder.RegisterType<MenuInRolesServices>().As<IMenuInRolesServices>().InstancePerDependency();
            builder.RegisterType<AccountServices>().As<IAccountServices>().InstancePerDependency();
            builder.RegisterType<RoleServices>().As<IRoleServices>().InstancePerDependency();
            builder.RegisterType<DbLogServices>().As<IDbLogServices>().InstancePerDependency();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container); //Set the WebApi DependencyResolver

        }
    }
}