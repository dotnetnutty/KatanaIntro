using Microsoft.Owin.Hosting;
using Owin;
using System;

namespace KatanaIntro
{
    /// <summary>
    /// 1. Install-Package Microsoft.Owin.Hosting
    /// 2. Install-Package Microsoft.Owin.Host.HttpListener
    /// To get pretty welcome page:
    /// 3. Install-Package Microsoft.Owin.Diagnostics and then app.UseWelcomePage()
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:8080";

            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Server started...");
                Console.ReadKey();
                Console.WriteLine("Server stopped.");
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWelcomePage();

            //app.Run(ctx =>
            //{
            //    return ctx.Response.WriteAsync("<h1>Hello World!</h1><p>This is Katana.</p>");
            //});
        }
    }
}