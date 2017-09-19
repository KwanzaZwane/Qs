using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Qs.Startup))]
namespace Qs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
