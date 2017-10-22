using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BC.Web.Startup))]
namespace BC.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
