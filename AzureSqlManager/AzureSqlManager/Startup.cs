using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzureSqlManager.Startup))]
namespace AzureSqlManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
