using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AngularJS.WebAPI.Startup))]
namespace AngularJS.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
