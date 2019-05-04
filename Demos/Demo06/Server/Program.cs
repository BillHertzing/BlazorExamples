using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;
using Funq;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace Server {
    class Program {
        static ILog Log { get; set; }

        public static async Task Main(string[] args) {

            // To ensure every class uses the same Global Logger, set the LogManager's LogFactory before initializing the ServiceStack's AppHost
            LogManager.LogFactory=new NLogFactory();
            // Create a logger instance for this class
            Log=LogManager.GetLogger(typeof(Program));
            Log.Debug("Entering Program.Main");

            var loadedFromDir =
              Path
              .GetDirectoryName(Assembly
              .GetExecutingAssembly()
                .Location);

            Directory.SetCurrentDirectory(loadedFromDir);

            var webHostBuilder = new WebHostBuilder()
                .CaptureStartupErrors(true)
                .UseSetting("detailedErrors", "true")
                .UseContentRoot(loadedFromDir)
                .UseKestrel()
                .UseStartup<Startup>()
                .UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")??"http://localhost:21200/");

            //webHostBuilder.RunConsoleAsync();
            Log.Debug("in Program.Main: webHostBuilder created");
            var webHost = webHostBuilder.Build();
            Log.Debug("in Program.Main: webHost created");
            await webHost.RunAsync();

            Log.Debug("Leaving Program.Main");

        }
    }
    public class Startup {
        static ILog Log { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // Create a logger instance for this class
            Log=LogManager.GetLogger(typeof(Startup));
            Log.Debug("Entering Startup.ConfigureServices");
            Log.Debug("Leaving Startup.ConfigureServices");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            // Create a logger instance for this class
            Log=LogManager.GetLogger(typeof(Startup));
            Log.Debug("Entering Startup.Configure");

           // app.UseServiceStack(new AppHost());

            app.Run(context => {
                context.Response.Redirect("/metadata");
                return Task.FromResult(0);
            });

            Log.Debug("Leaving Startup.Configure");

        }


    }/*
        public static async Task Main(string[] args) {

            // To ensure every class uses the same Global Logger, set the LogManager's LogFactory before initializing the ServiceStack's AppHost
            LogManager.LogFactory=new NLogFactory();
            // Create a logger instance for this class
            Log=LogManager.GetLogger(typeof(Program));
            Log.Debug("Entering Program.Main");

            var isService = !(Debugger.IsAttached||args.Contains("--console"));

            var loadedFromDir =
              Path
              .GetDirectoryName(Assembly
              .GetExecutingAssembly()
                .Location);
            Log.Debug($"loadedFromDir is {loadedFromDir}");
            Directory.SetCurrentDirectory(loadedFromDir);

            // set the port on which this ServiceStack application will listen.
            var listeningOn = "http://localhost:21200/";

            // Create the Host via a HostBuilder, configure it
            var builder = new HostBuilder()
                .UseContentRoot(loadedFromDir)
                 .ConfigureHostConfiguration(configHost => {
                     configHost.SetBasePath(Directory.GetCurrentDirectory());
                     configHost.AddJsonFile("hostsettings.json", optional: true);
                     configHost.AddEnvironmentVariables(prefix: "Demo_");
                     configHost.AddCommandLine(args);
                 })
                                 .ConfigureServices((hostContext, services) => {
                                     ;
                                 })
                                 .ConfigureLogging((hostContext, configLogging) => {
                                     configLogging.AddConsole();
                                     configLogging.AddDebug();
                                 })
                ;
            if (isService) {
                await builder.RunAsServiceAsync();
            } else {
                await builder.RunConsoleAsync();
            }


            Log.Debug($"AppHost Created at {DateTime.Now}, listening on {listeningOn}");

            Console.WriteLine("press any key to close the ServiceStack app");
            Console.ReadKey();
            Log.Debug("Leaving Program.Main");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args).
        );
*/
}

