using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BugOutNet.Startup))]
namespace BugOutNet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
