using AutoMapper;
using Microsoft.Owin;
using Owin;
using Web.Helpers;

[assembly: OwinStartupAttribute(typeof(Web.Startup))]
namespace Web
{
    public partial class Startup
    {
        public void InitializeAutoMapper()
        {
            AutoMapper.Mapper.Initialize(cfg => cfg.AddProfile<MappingProfile>());
        }


        public void Configuration(IAppBuilder app)
        {
            //app.MapSignalR();
            ConfigureAuth(app);
            InitializeAutoMapper();
        }
    }
}
