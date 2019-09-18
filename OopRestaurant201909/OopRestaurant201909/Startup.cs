using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OopRestaurant201909.Startup))]
namespace OopRestaurant201909
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
