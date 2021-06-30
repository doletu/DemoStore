using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DemoStore_Admin.Startup))]
namespace DemoStore_Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
