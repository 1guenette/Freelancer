using Microsoft.Owin;
using Owin;
using MG_5_FreelanceJobsite;

namespace MG_5_FreelanceJobsite
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}