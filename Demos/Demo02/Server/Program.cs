using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace Server {
    class Program {

        const string ListenOnURLs = "http://localhost:20200/";

        static ServiceStack.Logging.ILog Log { get; set; }

        public static async Task Main(string[] args) {

            // To ensure every class uses the same Global Logger, set the LogManager's LogFactory before initializing the hosting environment
            //  set the LogFactory to ServiceStack's NLogFactory
            LogManager.LogFactory=new NLogFactory();
            // Create a logger instance for this class
            Log=LogManager.GetLogger(typeof(Program));
            Log.Debug("Entering Program.Main");

            // determine where this program's entry point's executing assembly resides
            //   then change the working directory to the location where the assembly (and configuration files) are installed to.
            var loadedFromDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Directory.SetCurrentDirectory(loadedFromDir);

            // Create an IntegratedIISInProcess web host
            Log.Debug("in Program.Main: create webHostBuilder by calling static method CreateIntegratedIISInProcessWebHostBuilder");
            var webHostBuilder = CreateIntegratedIISInProcessWebHostBuilder(args);

            // Create the web server webHost
            Log.Debug("in Program.Main: create webHost by calling .Build() on the webHostBuilder");
            var webHost = webHostBuilder.Build();

            // Start the webHost
            Log.Debug("in Program.Main: webHost created, starting RunAsync and awaiting it");
            await webHost.RunAsync();
            Log.Debug($"in Program.Main: webHost.RunAsync called at {DateTime.Now}, listening on {ListenOnURLs}");

            // Hold the Console window open as we await the webHost
            Console.WriteLine("press any key to close the web hosting environment that is running as a ConsoleApp");
            Console.ReadKey();
            Log.Debug("in Program.Main: Leaving Program.Main");
        }

        // This Builder pattern creates a WebHostBuilder populated with an Integrated IIS InProcess web host 
        public static IWebHostBuilder CreateIntegratedIISInProcessWebHostBuilder(string[] args) =>
            // The method CreateDefaultBuilder includes IISIntegration which IS desired
            WebHost.CreateDefaultBuilder()
                //.UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                // Use hard-coded URLs for this demo to listen on
                .UseUrls(ListenOnURLs)
                ;
    }

    public class Startup {
        static readonly ILog Log = LogManager.GetLogger(typeof(Startup));
        public IConfiguration Configuration { get; }

        // This class gets created by the runtime when .Build is called on the webHostBuilder. The .ctor populates this class' Configuration property .
        public Startup(IConfiguration configuration) {
            Log.Debug("entering Program.Startup.ctor");
            Log.Debug("populating the Program.Startup.Configuration property by Constructior Injection");
            Configuration=configuration;
            Log.Debug("leaving Program.Startup.ctor");
        }

        // This method gets called by the runtime after this class' .ctor is finished. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            Log.Debug("Entering Program.Startup.ConfigureServices");
            Log.Debug("in Program.Startup.ConfigureServices: no service(s) have been injected in this Demo");
            Log.Debug("Leaving Program.Startup.ConfigureServices");
        }

        // This method gets called by the runtime when .Run or .RunAsync is called on the webHost instance. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            Log.Debug("Entering Program.Startup.Configure");
            Log.Debug($"in Program.Startup.Configure: GetCurrentProcess().ProcessName = {System.Diagnostics.Process.GetCurrentProcess().ProcessName}");
            Log.Debug("in Program.Startup.Configure: Create the SSApp");

            app.UseServiceStack(new SSAppHost() {
                AppSettings=new NetCoreAppSettings(Configuration) // Use **appsettings.json** and config sources
            });
            Log.Debug("in Program.Startup.Configure: SSApp creation is finished");
            Log.Debug("in Program.Startup.Configure: Provide the terminal middleware handler delegate to the IApplicationBuilder via .Run");
            // The supplied lambda becomes the final handler in the HTTP pipeline
            app.Run(async (context) => {
                Log.Debug("Last HTTP Pipeline handler");
                context.Response.StatusCode=404;
                await Task.FromResult(0);
            });

            Log.Debug("Leaving Program.Startup.Configure");
        }
    }


}