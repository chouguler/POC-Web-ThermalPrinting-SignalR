using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using Microsoft.AspNet.SignalR;
using System;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Linq;
using POCThinClientServices.Hub;

[assembly: OwinStartup(typeof(POCThinClientServices.Startup))]

namespace POCThinClientServices
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();            
        }
    }
}
