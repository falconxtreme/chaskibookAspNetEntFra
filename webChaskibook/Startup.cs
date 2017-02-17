using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(webChaskibook.Startup))]
namespace webChaskibook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
