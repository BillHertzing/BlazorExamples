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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace Server {
    class Program {
        static ILog Log { get; set; }

        public static async Task Main(string[] args) {

            // To ensure every class uses the same Global Logger, set the LogManager's LogFactory before initializing the hosting environment
            //  set the LogFactory to ServiceStack's NLogFactory
            LogManager.LogFactory=new NLogFactory();
            // Create a logger instance for this class
            Log=LogManager.GetLogger(typeof(Program));
            Log.Debug("Entering Program.Main");

            // determine where this program's entry point's executing assembly resides
            var loadedFromDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                
            // Set the program's current directory to the location where the executing assembly resides
            Directory.SetCurrentDirectory(loadedFromDir);
            /*
            // Create a kestrel server and host it iis in-process
            //  The WebHost.CreateDefaultBuilder helper enables IISIntegration
            var webHostBuilderInProcessIIS = WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(new ConfigurationBuilder()
                    .AddCommandLine(args)
                    .Build())
                // In V30P4, all SS interfaces return an error that "synchronous writes are disallowed", see following issue
                //  https://github.com/aspnet/AspNetCore/issues/8302
                // Woraround is to configure the default web server to AllowSynchronousIO=true
                // ToDo: see if this is fixed in a release after V30P4
                .ConfigureKestrel((context, options) => {
                    options.AllowSynchronousIO=true;
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")??"http://localhost:21200/")
                ;

            var webHostBuilder = webHostBuilderInProcessIIS;

            // ToDo: Treat errors differently based on environment (Debug, or Production)
            Log.Debug("in Program.Main: modify webHostBuilder based on the environment in whihc the Net Core Host is executing ");
            webHostBuilder=webHostBuilder
                .CaptureStartupErrors(true)
                .UseSetting("detailedErrors", "true")
           ;
                
            // Create the web server host
            Log.Debug("in Program.Main: create webHost by calling .Build() on the webHostBuilder");
            var webHost = webHostBuilder.Build();

            // Start the webHost
            Log.Debug("in Program.Main: webHost created, starting RunAsync and awaiting it");

            await webHost.RunAsync();
            */
            var genericHostBuilder = CreateGenericHostBuilder(args);
            var genericHost = genericHostBuilder.Build();
            await genericHost.RunAsync();

            Log.Debug($"webHost.RunAsync called at {DateTime.Now}, listening on {"ToDo: get the list of listening proto:host:port from the webHost"}");

            Console.WriteLine("press any key to close the hosting environment that is running as a ConsoleApp");
            Console.ReadKey();
            Log.Debug("Leaving Program.Main");

        }

        // This (older) post has great info and examples on setting up the Kestrel options
        //https://github.com/aspnet/KestrelHttpServer/issues/1334
        public static IHostBuilder CreateGenericHostBuilder(string[] args) { 
            // CreateDefaultBuilder includes IISIntegration which is NOT desired, so
            // The Kestral Web Server must be manually configured into the Generic Host
            // Configure Kestral
            return new HostBuilder()
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseKestrel()
                    // In V30P4, all SS interfaces return an error that "synchronous writes are disallowed", see following issue
                    //  https://github.com/aspnet/AspNetCore/issues/8302
                    // Woraround is to configure the default web server to AllowSynchronousIO=true
                    // ToDo: see if this is fixed in a release after V30P4
                    .ConfigureKestrel((context, options) => {
                        options.AllowSynchronousIO=true;
                    })
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")??"http://localhost:21200/")
                    ;
                });

        }

    }
    public class Startup {
        static ILog Log { get; set; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration=configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // Create a logger instance for this class
            Log=LogManager.GetLogger(typeof(Startup));
            Log.Debug("Entering Startup.ConfigureServices");
            Log.Debug("Leaving Startup.ConfigureServices");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            // Create a logger instance for this class
            Log=LogManager.GetLogger(typeof(Startup));
            Log.Debug("Entering Startup.Configure");

            app.UseServiceStack(new SSAppHost() {
                AppSettings=new NetCoreAppSettings(Configuration) // Use **appsettings.json** and config sources
            });

            // The supplied lambda becomes the final handler in the HTTP pipeline
            app.Run(async  (context) => {
                Log.Debug("Last HTTP Pipeline handler");
                context.Response.StatusCode=404;
                await Task.FromResult(0);
            });

            Log.Debug("Leaving Startup.Configure");

        }
    }

    /*
        public static async Task Main(string[] args) {


            var isService = !(Debugger.IsAttached||args.Contains("--console"));



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


            Log.Debug($"SSAppHost Created at {DateTime.Now}, listening on {listeningOn}");

            Console.WriteLine("press any key to close the ServiceStack app");
            Console.ReadKey();
            Log.Debug("Leaving Program.Main");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args).
        );
		
	 //await webHostBuilder.RunConsoleAsync(); // If console specified or debugger attached

*/
}

