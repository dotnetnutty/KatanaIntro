using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// To allow routing:
/// 1. Install-Package Microsoft.AspNet.WebApi.OwinSelfHost
/// 
/// To move to IIS:
/// 2. Install-Package Microsoft.Owin.Host.SystemWeb
/// 3. Comment out the Main method
/// 4. Change output type to class library
/// 5. Change output path to bin directory (change from bin\debug)
/// 6. Appears you need to rename App.config to Web.config. Otherwise you get exception about Owin assembly.
///    See http://stackoverflow.com/questions/26218557/microsoft-owin-2-0-2-0-dependency-conflict-when-i-have-installed-3-0-0-0
/// 7. On command line, run:
/// 
///     c:\Program Files\IIS Express\iisexpress.exe /path:c:\path\to\project\root
/// 
/// Can then browse localhost:8080
/// 
/// Check your project has a class named "Startup"
/// </summary>
namespace KatanaIntro
{
    using System.IO;
    using System.Web.Http;
    using AppFunc = Func<IDictionary<string, object>, Task>;

    //Uncomment and undo the steps in "move to IIS" above to run this as a console application
    //class ProgramLowLevel
    //{
    //    static void Main(string[] args)
    //    {
    //        string url = "http://localhost:8080";

    //        using (WebApp.Start<Startup2>(url))
    //        {
    //            Console.WriteLine("Server started...");
    //            Console.ReadKey();
    //            Console.WriteLine("Server stopped.");
    //        }
    //    }
    //}

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // insert a component before our HelloWorldComponent
            //app.Use(async (environment, next) =>
            //{
            //    foreach (var pair in environment.Environment)
            //    {
            //        Console.WriteLine($"{pair.Key}: {pair.Value}");
            //    }

            //    await next();
            //});

            app.Use(async (environment, next) =>
            {
                Console.WriteLine("Requesting: " + environment.Request.Path);

                await next();

                Console.WriteLine("Response: " + environment.Response.StatusCode);
            });

            ConfigureWebApi(app);

            app.Use<HelloWorldComponent>(); // Responds unless something else responds first
        }

        /// <summary>
        /// Configures routing rules
        /// </summary>
        /// <param name="app"></param>
        private void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{message}");
            app.UseWebApi(config);
        }
    }

    /// <summary>
    /// This is middleware as there might be components ahead of it or after it in the pipeline
    /// </summary>
    public class HelloWorldComponent
    {
        AppFunc _next;

        public HelloWorldComponent(AppFunc next)
        {
            _next = next;
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            var response = environment["owin.ResponseBody"] as Stream;

            using (var writer = new StreamWriter(response))
            {
                return writer.WriteAsync("<h1>Hello World</h1>");
            }
        }

        //public async Task Invoke(IDictionary<string, object> environment)
        //{
        //    // goes to next step in pipeline
        //    //await _next(environment);
        //}
    }
}