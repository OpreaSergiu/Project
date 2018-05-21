using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectPlatform.Startup))]
namespace ProjectPlatform
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
