using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyShopper.WebUI.Startup))]
namespace MyShopper.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
