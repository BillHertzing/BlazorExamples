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
using ServiceStack.Text;

namespace Server {
    partial class Program {

        public const string ASPNETCOREEnvironmentVariablePrefix = "ASPNETCORE_";
        public const string CustomEnvironmentVariablePrefix = "BlazorDemos_";

        public const string WebHostBuilderToBuildConfigRootKey = "WebHostBuilderToBuild";
        public const string WebHostBuilderStringDefault = "KestrelAloneWebHostBuilder";

        public const string EnvironmentProduction = "Production"; // Environments.Production
        public const string EnvironmentDevelopment = "Development";
        public const string EnvironmentDefault = EnvironmentProduction;
        public const string EnvironmentConfigRootKey = "Environment";

        public const string genericHostSettingsFileName = "genericHostSettings";
        public const string hostSettingsFileNameSuffix = ".json";

        public const string URLSConfigRootKey = "urls";

        public const string InvalidWebHostBuilderStringExceptionMessage = "The WebHostBuilder string {0} specified in the environment variable does not match any member of the SupportedWebHostBuilders enumeration.";
        public const string InvalidWebHostBuilderToBuildExceptionMessage = "The WebHostBuilder enumeration argument specified {0} is not supported.";
        public const string InvalidSupportedEnvironmentStringExceptionMessage = "The Environment string {0} specified in the environment variable does not match any member of the SupportedEnvironments enumeration.";
        public const string InvalidSupportedEnvironmentExceptionMessage = "The Environment enumeration argument specified {0} is not supported.";


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

            // Create the ConfigurationBuilder for this genericHost. This creates an ordered chain of configuration providers. The first providers in the chain have the lowest priority, the last providers in the chain have a higher priority.
            var genericHostConfigurationBuilder = new ConfigurationBuilder()
                // Start with a "compiled-in defaults" for anything that is REQUIRED to be provided in configuration for Production
                .AddInMemoryCollection(GenericHostDefaultConfiguration.Production)
                // SetBasePath creates a Physical File provider, which will be used by the following methods
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(genericHostSettingsFileName, optional: true)
                .AddEnvironmentVariables(prefix: ASPNETCOREEnvironmentVariablePrefix)
                .AddEnvironmentVariables(prefix: CustomEnvironmentVariablePrefix)
                // Add commandline switch provider
                .AddCommandLine(args);

            // Create this program's ConfigurationRoot
            IConfigurationRoot genericHostConfigurationRoot = genericHostConfigurationBuilder.Build();

            // Validate the values of Environment and WebHostBuilderToBuild from the configuration are supported by this program
            var webHostBuilderName = genericHostConfigurationRoot.GetValue<string>(WebHostBuilderToBuildConfigRootKey, WebHostBuilderStringDefault);
            SupportedWebHostBuilders webHostBuilderToBuild;
            if (!Enum.TryParse<SupportedWebHostBuilders>(webHostBuilderName, out webHostBuilderToBuild)) {
                throw new InvalidDataException(String.Format(InvalidWebHostBuilderStringExceptionMessage,webHostBuilderName));
            }
            Log.Debug($"in Program.Main: webHostBuilderToBuild = {webHostBuilderToBuild.ToString()}");

            // Determine the environment (Debug, TestingUnit, TestingX, Staging, Production) to use from an EnvironmentVariable
            var envName = genericHostConfigurationRoot.GetValue<string>(EnvironmentConfigRootKey, EnvironmentDefault);
            switch (envName) {
                case EnvironmentDevelopment:
                case EnvironmentProduction:
                    break;
                default:
                    throw new NotImplementedException(String.Format(InvalidSupportedEnvironmentExceptionMessage,envName));
            }

            // Create the GenericHostBuilder instance based on the ConfigurationRoot
            Log.Debug("in Program.Main: create genericHostBuilder by calling static method CreateSpecificHostBuilder");
            IHostBuilder genericHostBuilder = CreateSpecificHostBuilder(args, genericHostConfigurationRoot);
            // Log.Debug($"in Program.Main: genericHostBuilder.Dump() = {genericHostBuilder.Dump()}");

            // Create the generic host genericHost
            Log.Debug("in Program.Main: create genericHost by calling .Build() on the genericHostBuilder");
            var genericHost = genericHostBuilder.Build();
            Log.Debug($"in Program.Main: genericHost.Dump() = {genericHost.Dump()}");

            // Single Stepping displays the .Dump internal `gets` as locals,
            //  making it possible to inspect the structures provided by the configuration provider 
            //  configured by the ConfigureHostConfiguration method call in the static genericHostBuilder method.
            // Set breakpoint here in debugging mode
            var xServices = genericHost.Services;
            // Inspect the Non-Public Members ->ResolvedServices. 
            //   Count is 21 when env is Development and Specific is (both), either / or. Specific WebHostBuilder has no effect on the number of services
            //   The 0th index is (for my development environment) is of type ConfigurationRoot
            //   Expand the Non-Public members -> ResolvedServices[0] Non-Public members value->Providers.
            //   This shows the chain of ConfigurationProviders. Their values show that they are the expected chain as defined in the static method CreateSpecificGenericHostBuilder
            //   Modify that structure (order and or content), and the chain displayed in the debugger will change as well, following suit (order and or content)
            //  DotNetCore V3.0P5 has an issue, the values for the webhost json file names are appearing in the genericHost configuration providers.
            Log.Debug($"in Program.Main: genericHost.IConfiguration.Dump() = { genericHost.Services.GetService<IConfigurationRoot>().Dump()}");
            IConfigurationRoot xIConfigurationRoot = genericHost.Services.GetService<IConfigurationRoot>();
            Log.Debug($"in Program.Main: genericHost.Configuration.Dump() = { genericHost.Services.GetService<ConfigurationRoot>().Dump()}");
            IConfigurationRoot xConfigurationRoot = genericHost.Services.GetService<ConfigurationRoot>();
            //  Inspect the the InternalHost injected service; this is the WebHost. Then inspect it's configuration provider.
            //  On my system, it is number 21 (the last one)
            //  Expanding the non-public members in the debugger, at the bottom of the heap are the things that wrap up a webhostm things like 
            // an implementation of an applicationLifetime (application (webserver) start and stopping events)
            // a hostLifetime implementation with EnvironmentName, ContentRootPath, the ApplicationName, and a ContentRootFileProvider (an implementation of a PhhysicalFileProvider)
            //  The hostLifetime implementation also contains Options. Up to this point in the demonstrations, all interaction has been through the debugger in Visual studio. 
            //  This starts a ConsoleApplication and runs the async Program.Main method inside the ConnsoleApplication. 
            //  The implementation of the InternalHost adds a ConsoleLifetimeOption, and that has only a single suboption "SuppressStatusMessages".
            // Soon the demos will have a different Option. and the applicationLifetime events will come into play.

            //  Note also that the debugger shows five elements in the Disposable collection. This includes the ConfigurationRoot, the LoggerFactory, the Internal.Host, and the Internal Options ConsoleLifetime value

            // it is much easier to log these structures from inside the running host methods
            // In this demo, the Startup class now logs these structures from inside 



            // Start the genericHost
            Log.Debug($"in Program.Main: genericHost created, starting RunAsync, listening on {genericHostConfigurationRoot.GetValue<string>(URLSConfigRootKey)} and awaiting it");
            await genericHost.RunAsync();
            Log.Debug($"in Program.Main: genericHost.RunAsync called at {DateTime.Now}, listening on {genericHostConfigurationRoot.GetValue<string>(URLSConfigRootKey)}");

            // Hold the Console window open as we await the webHost
            Console.WriteLine("press any key to close the generic hosting environment that is running as a ConsoleApp");
            Console.ReadKey();
            Log.Debug("in Program.Main: Leaving Program.Main");
        }

        // This Builder pattern creates a GenericHostBuilder populated by a specific web host as specified by a paramter
        public static IHostBuilder CreateSpecificHostBuilder(string[] args, IConfigurationRoot genericHostConfigurationRoot) {
            var hb = new HostBuilder()

                // The Generic Host Configuration. 
                .ConfigureHostConfiguration(configHost => {
                    // Start with a "compiled-in defaults" for anything that is required to be provided in configuration for Production
                    configHost.AddInMemoryCollection(GenericHostDefaultConfiguration.Production);
                    // SetBasePath creates a Physical File provider, which will be used by the two following methods
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile(genericHostSettingsFileName+hostSettingsFileNameSuffix, optional: true);
                    configHost.AddEnvironmentVariables(prefix: ASPNETCOREEnvironmentVariablePrefix);
                    // ToDo: get all (resolved) commandline args from genericHostConfigurationRoot
                    configHost.AddCommandLine(args);
                });

                // What kind of WebHostBuilder to build
                 SupportedWebHostBuilders webHostBuilderToBuild = genericHostConfigurationRoot.GetValue<SupportedWebHostBuilders>(WebHostBuilderToBuildConfigRootKey);
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
                    // Workaround is to configure the default web server to AllowSynchronousIO=true
                    // ToDo: see if this is fixed in a release after V30P5
                    // Configure Kestrel
                    webHostBuilder.ConfigureKestrel((context, options) => {
                        options.AllowSynchronousIO=true;
                    });

                    // Configuration of the WebHostBuilder based on Environment
                    string env = genericHostConfigurationRoot.GetValue<string>(EnvironmentConfigRootKey);
                    switch (env) {
                        case EnvironmentDevelopment:
                            // This is where many developer conveniences are configured for Development environment
                            // In the Development environment, modify the WebHostBuilder's settings to use the detailed error pages, and to capture startup errors
                            webHostBuilder.CaptureStartupErrors(true)
                               .UseSetting("detailedErrors", "true");
                            break;
                        case EnvironmentProduction:
                            break;
                        default:
                            throw new InvalidEnumArgumentException(String.Format(InvalidSupportedEnvironmentExceptionMessage, env));
                    }

                    // Specify the class to use when starting the WebHost
                    webHostBuilder.UseStartup<Startup>();

                    // Prior demos configured the URLs to ListenTo at this step; now its a configuration setting  "urls"
                    //  We no longer have to explicitly tell the Demo where to pickup the urls
                    //  In fact, there are a number of Environment Variable name patterns that the Environment Variables Configuration Provider 
                    //  will pickup by default, as documented 
                    //   https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.0#environment-variables-configuration-provider
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
