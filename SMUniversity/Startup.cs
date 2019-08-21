using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SMUniversity.Startup))]
namespace SMUniversity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
