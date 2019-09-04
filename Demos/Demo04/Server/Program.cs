using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;


namespace Server {
    class Program {

        public const string ListenOnURLs = "http://localhost:20400/";

        public const string EnvironmentVariablePrefix = "BlazorDemos_";
        public const string EnvironmentVariableWebHostBuilder = "WebHostBuilder";
        public const string WebHostBuilderStringDefault = "CreateIntegratedIISInProcessWebHostBuilder";

        public const string InvalidWebHostBuilderStaticMethodNameExceptionMessage = "The WebHostBuilder specified in the environment variable does not match any static method name returning an IWebHostBuilder";

        static ServiceStack.Logging.ILog Log { get; set; }

        // Helper method to properly combine the prefix with the suffix
        static string EnvironmentVariableFullName(string name) { return EnvironmentVariablePrefix+name; }

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

            // Determine the web host builder to use from an EnvironmentVariable
            var webHostBuilderName = Environment.GetEnvironmentVariable(EnvironmentVariableFullName(EnvironmentVariableWebHostBuilder))??WebHostBuilderStringDefault;

            // Set the WebHostBuilder instance from a static method selected based upon the name supplied by the environment variable
            IWebHostBuilder webHostBuilder;
            if (webHostBuilderName=="IntegratedIISInProcessWebHostBuilder") {
                // Create an IntegratedIISInProcess web host
                Log.Debug("in Program.Main: create webHostBuilder by calling static method CreateIntegratedIISInProcessWebHostBuilder");
                webHostBuilder=CreateIntegratedIISInProcessWebHostBuilder();
            } else if (webHostBuilderName=="KestrelAloneWebHostBuilder") {
                // Create an Kestrel web host builder
                Log.Debug("in Program.Main: create webHostBuilder by calling static method CreateKestrelAloneWebHostBuilder");
                webHostBuilder=CreateKestrelAloneWebHostBuilder();
            } else {
                throw new InvalidDataException(InvalidWebHostBuilderStaticMethodNameExceptionMessage);
            }

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

        // This Builder pattern creates a WebHostBuilder populated with instructions to build an Integrated IIS InProcess WebHost
        public static IWebHostBuilder CreateIntegratedIISInProcessWebHostBuilder() =>
            // The method CreateDefaultBuilder includes IISIntegration which IS desired
            WebHost.CreateDefaultBuilder()
                // Specify the class to use when starting the WebHost
                .UseStartup<Startup>()
                // Use hard-coded URLs for this demo to listen on
                .UseUrls(ListenOnURLs);

        // This Builder pattern creates a WebHostBuilder populated with instructions to build a Kestrel WebHost with no IIS integration
        public static IWebHostBuilder CreateKestrelAloneWebHostBuilder() =>
            // CreateDefaultBuilder includes IISIntegration which is NOT desired, so
            // The Kestrel WebHost must be manually configured into the WebHostBuilder
            new WebHostBuilder()
                .UseKestrel()
                // This (older) post has great info and examples on setting up the Kestrel options
                //https://github.com/aspnet/KestrelHttpServer/issues/1334
                // In V30P5, all SS interfaces return an error that "synchronous writes are disallowed", see following issue
                //  https://github.com/aspnet/AspNetCore/issues/8302
                // Woraround is to configure the default web server to AllowSynchronousIO=true
                // ToDo: see if this is fixed in a release after V30P5
                // Configure Kestrel
                .ConfigureKestrel((context, options) => {
                    options.AllowSynchronousIO=true;
                })
				.UseContentRoot(Directory.GetCurrentDirectory())
                // Specify the class to use when starting the WebHost
                .UseStartup<Startup>()
                // Use hard-coded URLs for this demo to listen on
                .UseUrls(ListenOnURLs);
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
