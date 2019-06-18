using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;

namespace Server {
    class Program {

        public const string ListenOnURLs = "http://localhost:20500/";

        public const string EnvironmentVariablePrefix = "BlazorDemos";
        public const string EnvironmentVariableWebHostBuilder = "WebHostBuilder";
        public const string WebHostBuilderDefault = "CreateIntegratedIISInProcessWebHostBuilder";
        public const string EnvironmentVariableEnvironment = "Environment";
        public const string EnvironmentDefault = "Production";

        public const string InvalidWebHostBuilderStaticMethodNameExceptionMessage = "The WebHostBuilder specified in the environment variable does not match any static method name returning an IWebHostBuilder";
        public const string InvalidEnvironmentExceptionMessage = "The Environment specified in the environment variable does not match known environment in this program";

        static ServiceStack.Logging.ILog Log { get; set; }

        // Helper method to properly combine the prefix with the suffix
        static string EnvironmentVariableFullName(string name) { return EnvironmentVariablePrefix+"_"+name; }

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
            var webHostBuilderName = Environment.GetEnvironmentVariable(EnvironmentVariableFullName(EnvironmentVariableWebHostBuilder))??WebHostBuilderDefault;

            // ToDo: find clever (fast) way to express "Go through the list of static methods returning IHostBuilder", if a (string) cast of the method name matches the WebHostBuilder environment variable string, select the method with the matching name, log that fact,, and call it here...
            // Set the GenericHostBuilder instance based on the name supplied by the environment variable
            IHostBuilder genericHostBuilder;
            if (webHostBuilderName=="IntegratedIISInProcessWebHostBuilder") {
                // Create an IntegratedIISInProcess generic host builder
                Log.Debug("in Program.Main: create genericHostBuilder by calling static method CreateGenericHostHostingIntegratedIISInProcessWebHostBuilder");
                genericHostBuilder=CreateGenericHostHostingIntegratedIISInProcessWebHostBuilder(args);
            } else if (webHostBuilderName=="KestrelAloneWebHostBuilder") {
                // Create an Kestrel only generic host builder
                Log.Debug("in Program.Main: create genericHostBuilder by calling static method CreateGenericHostHostingKestrelAloneBuilder");
                genericHostBuilder=CreateGenericHostHostingKestrelAloneBuilder(args);
            } else {
                throw new InvalidDataException(InvalidWebHostBuilderStaticMethodNameExceptionMessage);
            }

            // Determine the environment (Debug, TestingUnit, TestingX, Staging, Production) to use from an EnvironmentVariable
            var env = Environment.GetEnvironmentVariable(EnvironmentVariableFullName(EnvironmentVariableEnvironment))??EnvironmentDefault;
            Log.Debug($"in Program.Main: env is {env}");

            // Modify the genericHostBuilder according to the environment
            // ToDo: investigate using an enumeration to support localization
            switch (env) {
                case "Development":
                    // This is where many developer conveniences are configured for Development environment
                    // In the Development environment, modify the WebHostBuilder's settings to use the detailed error pages, and to capture startup errors
                    genericHostBuilder.ConfigureWebHost(webHostBuilder =>
                    webHostBuilder.CaptureStartupErrors(true)
                        .UseSetting("detailedErrors", "true")
                    );
                    break;
                case "Production":
                    break;
                default:
                    throw new InvalidOperationException(InvalidEnvironmentExceptionMessage);
            }

            // Create the generic host genericHost
            Log.Debug("in Program.Main: create genericHost by calling .Build() on the genericHostBuilder");
            var genericHost = genericHostBuilder.Build();

            // Start the genericHost
            Log.Debug("in Program.Main: genericHost created, starting RunAsync and awaiting it");
            await genericHost.RunAsync();
            Log.Debug($"in Program.Main: genericHost.RunAsync called at {DateTime.Now}, listening on {ListenOnURLs}");

            // Hold the Console window open as we await the webHost
            Console.WriteLine("press any key to close the generic hosting environment that is running as a ConsoleApp");
            Console.ReadKey();
            Log.Debug("in Program.Main: Leaving Program.Main");
        }

        // This Builder pattern creates a GenericHostBuilder populated with Kestrel hosted within IISIntegration
        public static IHostBuilder CreateGenericHostHostingIntegratedIISInProcessWebHostBuilder(string[] args) =>
            new HostBuilder()
                .ConfigureWebHostDefaults(webHostBuilder => {
                    // The method UseIISIntegration instructs teh HostBuilder to ise IISIntegration which IS desired
                    webHostBuilder.UseIISIntegration()
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
                    // Specify the class to use when starting the WebHost
                    .UseStartup<Startup>()
                    // Use hard-coded URLs for this demo to listen on
                    .UseUrls(ListenOnURLs)
                    ;
                });

        // This Builder pattern creates a GenericHostBuilder populated with only Kestrel with no IIS integration
        public static IHostBuilder CreateGenericHostHostingKestrelAloneBuilder(string[] args) =>
            // CreateDefaultBuilder includes IISIntegration which is NOT desired, so
            // The Kestrel Web Server must be manually configured into the Generic Host
            new HostBuilder()
                .ConfigureWebHostDefaults(webHostBuilder => {
                    // specify Kestrel as the WebHost
                    webHostBuilder.UseKestrel()
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
                    // Specify the class to use when starting the WebHost
                    .UseStartup<Startup>()
                    // Use hard-coded URLs for this demo to listen on
                    .UseUrls(ListenOnURLs)
                    ;
                });

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
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