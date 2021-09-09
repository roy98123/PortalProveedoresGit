using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Extensions.Http;
using Owin;
using Microsoft.Extensions.DependencyInjection;

[assembly: OwinStartup(typeof(Grow.PortalProveedores.Startup))]

namespace Grow.PortalProveedores
{
    public partial class Startup
    {


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
