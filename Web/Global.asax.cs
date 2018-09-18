using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using DataTablesDotNet.ModelBinding;
using DataTablesDotNet.Models;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.Infrastructure;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            ModelBinders.Binders.Add(typeof(DataTablesRequest), new DataTablesModelBinder());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);  
            AutofacConfig.Register();
            //SimpleInjector.Configure();
        }
    }
}
