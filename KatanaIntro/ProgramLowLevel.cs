using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KatanaIntro
{
    using System.IO;
    using AppFunc = Func<IDictionary<string, object>, Task>;

    class ProgramLowLevel
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:8080";

            using (WebApp.Start<Startup2>(url))
            {
                Console.WriteLine("Server started...");
                Console.ReadKey();
                Console.WriteLine("Server stopped.");
            }
        }
    }

    public class Startup2
    {
        public void Configuration(IAppBuilder app)
        {
            // insert a component before our HelloWorldComponent
            app.Use(async (environment, next) =>
            {
                foreach (var pair in environment.Environment)
                {
                    Console.WriteLine($"{pair.Key}: {pair.Value}");
                }

                await next();
            });

            app.Use(async (environment, next) =>
            {
                Console.WriteLine("Requesting: " + environment.Request.Path);

                await next();

                Console.WriteLine("Response: " + environment.Response.StatusCode);
            });

            app.Use<HelloWorldComponent>();
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