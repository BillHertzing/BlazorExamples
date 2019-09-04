using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;


namespace Server {
    partial class Program {

        public const string ListenOnURLsKestrelAloneDevelopment = "http://localhost:20600/";
        public const string ListenOnURLsIntegratedIISInProcessDevelopment = "http://localhost:20600/";
        public const string ListenOnURLsKestrelAloneProduction = "http://localhost:20600/";
        public const string ListenOnURLsIntegratedIISInProcessProduction = "http://localhost:20600/";

        public const string EnvironmentVariablePrefix = "BlazorDemos";
        public const string EnvironmentVariableWebHostBuilder = "WebHostBuilder";
        public const string WebHostBuilderStringDefault = "CreateIntegratedIISInProcessWebHostBuilder";
        public const string EnvironmentVariableEnvironment = "Environment";
        public const string EnvironmentStringDefault = "Production";

        public const string InvalidWebHostBuilderStringExceptionMessage = "The WebHostBuilder string {0} specified in the environment variable does not match any member of the SupportedWebHostBuilders enumeration.";
        public const string InvalidWebHostBuilderToBuildExceptionMessage = "The WebHostBuilder enumeration argument specified {0} is not supported.";
        public const string InvalidSupportedEnvironmentStringExceptionMessage = "The Environment string {0} specified in the environment variable does not match any member of the SupportedEnvironments enumeration.";

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
            var webHostBuilderName = Environment.GetEnvironmentVariable(EnvironmentVariableFullName(EnvironmentVariableWebHostBuilder))??WebHostBuilderStringDefault;
            SupportedWebHostBuilders webHostBuilderToBuild;
            if (!Enum.TryParse<SupportedWebHostBuilders>(webHostBuilderName, out webHostBuilderToBuild)) {
                throw new InvalidDataException(String.Format(InvalidWebHostBuilderStringExceptionMessage,webHostBuilderName));
            }
            Log.Debug($"in Program.Main: webHostBuilderToBuild = {webHostBuilderToBuild.ToString()}");

            // Create the GenericHostBuilder instance based on the webHostBuilderToBuild string supplied by the environment variable
            Log.Debug("in Program.Main: create genericHostBuilder by calling static method CreateGenericHostHostingSpecificWebHostBuilder");
            IHostBuilder genericHostBuilder = CreateSpecificGenericHostBuilder(args, webHostBuilderToBuild);

            // Determine the environment (Debug, TestingUnit, TestingX, Staging, Production) to use from an EnvironmentVariable
            var env = Environment.GetEnvironmentVariable(EnvironmentVariableFullName(EnvironmentVariableEnvironment))??EnvironmentStringDefault;
            Log.Debug($"in Program.Main: env is {env}");

            // Modify the genericHostBuilder according to the Environment and the WebHostBuilderToBuild
            // Use string constants for teh URLS toListenOn for the four combinations of Environment and WebHostBuilderToBuild
            if (env==Environments.Development) {
                // This is where many developer conveniences are configured for Development environment
                // In the Development environment, modify the WebHostBuilder's settings to use the detailed error pages, and to capture startup errors
                genericHostBuilder.ConfigureWebHost(webHostBuilder =>
                webHostBuilder.CaptureStartupErrors(true)
                    .UseSetting("detailedErrors", "true")
                );
                if (webHostBuilderToBuild==SupportedWebHostBuilders.KestrelAloneWebHostBuilder) {
                    genericHostBuilder.ConfigureWebHost(webHostBuilder =>
                        webHostBuilder.UseUrls(ListenOnURLsKestrelAloneDevelopment)
                    );
                } else if (webHostBuilderToBuild==SupportedWebHostBuilders.IntegratedIISInProcessWebHostBuilder) {
                    genericHostBuilder.ConfigureWebHost(webHostBuilder =>
                        webHostBuilder.UseUrls(ListenOnURLsIntegratedIISInProcessDevelopment)
                    );

                } else {
                    throw new InvalidEnumArgumentException(String.Format(InvalidWebHostBuilderToBuildExceptionMessage,webHostBuilderToBuild.ToString()));
                }
            } else if (env==Environments.Production) {
                if (webHostBuilderToBuild==SupportedWebHostBuilders.KestrelAloneWebHostBuilder) {
                    genericHostBuilder.ConfigureWebHost(webHostBuilder =>
                        webHostBuilder.UseUrls(ListenOnURLsKestrelAloneProduction)
                    );
                } else if (webHostBuilderToBuild==SupportedWebHostBuilders.IntegratedIISInProcessWebHostBuilder) {
                    genericHostBuilder.ConfigureWebHost(webHostBuilder =>
                        webHostBuilder.UseUrls(ListenOnURLsIntegratedIISInProcessProduction)
                    );
                } else {
                    throw new InvalidEnumArgumentException(String.Format(InvalidWebHostBuilderToBuildExceptionMessage,webHostBuilderToBuild.ToString()));
                }
            } else {
                throw new NotImplementedException(String.Format(InvalidSupportedEnvironmentStringExceptionMessage,env));
            }

            // Create the generic host genericHost
            Log.Debug("in Program.Main: create genericHost by calling .Build() on the genericHostBuilder");
            var genericHost = genericHostBuilder.Build();

            // Start the genericHost
            Log.Debug("in Program.Main: genericHost created, starting RunAsync and awaiting it");
            await genericHost.RunAsync();
            Log.Debug($"in Program.Main: genericHost.RunAsync called at {DateTime.Now}");

            // Hold the Console window open as we await the webHost
            Console.WriteLine("press any key to close the generic hosting environment that is running as a ConsoleApp");
            Console.ReadKey();
            Log.Debug("in Program.Main: Leaving Program.Main");
        }

        // This Builder pattern creates a GenericHostBuilder populated by a specific web host as specified by a paramter
        public static IHostBuilder CreateSpecificGenericHostBuilder(string[] args, SupportedWebHostBuilders webHostBuilderToBuild) {
            var hb = new HostBuilder();
            hb.ConfigureWebHostDefaults(webHostBuilder => {
                switch (webHostBuilderToBuild) {
                    case SupportedWebHostBuilders.KestrelAloneWebHostBuilder:
                        webHostBuilder.UseKestrel();
                        break;
                    case SupportedWebHostBuilders.IntegratedIISInProcessWebHostBuilder:
                        webHostBuilder.UseIISIntegration();
                        break;
                    default:
                        throw new InvalidEnumArgumentException(InvalidWebHostBuilderToBuildExceptionMessage);
                }
                // This (older) post has great info and examples on setting up the Kestrel options
                //https://github.com/aspnet/KestrelHttpServer/issues/1334
                // In V30P5, all SS interfaces return an error that "synchronous writes are disallowed", see following issue
                //  https://github.com/aspnet/AspNetCore/issues/8302
                // Woraround is to configure the default web server to AllowSynchronousIO=true
                // ToDo: see if this is fixed in a release after V30P5
                // Configure Kestrel
                webHostBuilder.ConfigureKestrel((context, options) => {
                    options.AllowSynchronousIO=true;
                });
                // Specify the class to use when starting the WebHost
                webHostBuilder.UseStartup<Startup>();
            });
            return hb;
        }
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

