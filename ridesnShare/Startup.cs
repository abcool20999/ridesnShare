using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ridesnShare.Startup))]
namespace ridesnShare
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
