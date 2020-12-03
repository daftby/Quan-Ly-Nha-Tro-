using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuanLyNhaTro.Startup))]
namespace QuanLyNhaTro
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
