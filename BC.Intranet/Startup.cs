using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BC.Intranet.Startup))]
namespace BC.Intranet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
