using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChatDemo.Startup))]
namespace ChatDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
