using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DemoStore.Startup))]
namespace DemoStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
