using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LeaveApp.Startup))]
namespace LeaveApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
