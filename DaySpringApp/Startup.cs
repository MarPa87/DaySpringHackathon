using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DaySpringApp.Startup))]
namespace DaySpringApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
