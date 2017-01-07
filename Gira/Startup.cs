using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Gira.Startup))]
namespace Gira
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
