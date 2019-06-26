using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

using Serilog;
//Required for Serilog.SelfLog
using Serilog.Debugging;

using ServiceStack;
using ServiceStack.Text;

using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;

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
        public const string webHostSettingsFileName = "webHostSettings";
        public const string hostSettingsFileNameSuffix = ".json";

        public const string URLSConfigRootKey = "urls";

        public const string InvalidWebHostBuilderStringExceptionMessage = "The WebHostBuilder string {0} specified in the environment variable does not match any member of the SupportedWebHostBuilders enumeration.";
        public const string InvalidWebHostBuilderToBuildExceptionMessage = "The WebHostBuilder enumeration argument specified {0} is not supported.";
        public const string InvalidSupportedEnvironmentStringExceptionMessage = "The Environment string {0} specified in the environment variable does not match any member of the SupportedEnvironments enumeration.";
        public const string InvalidSupportedEnvironmentExceptionMessage = "The Environment enumeration argument specified {0} is not supported.";
        public const string InvalidCircularEnvironmentExceptionMessage = "The Environment \"Production\" should not be reached here";
        public const string InvalidRedeclarationOfEnvironmentExceptionMessage = "The initial Environment from the initialConfigurationRoot is {0}, and after reconfiguration, the Environment is {1}. this is a mis-match, and indicates a problem with the environment-specific configuration providers. Environment-specific configuration providers are not allowed to change the environment";


        public static async Task Main(string[] args) {

            // ETW Logging 
            DemoETWProvider.Log.Information("Entering Program.Main Information message");

            // ToDo: Create default production logger configuration prior to having a ConfigurationRoot
            Log.Debug("Entering Program.Main");

            // determine where this program's entry point's executing assembly resides
            //   then change the working directory to the location where the assembly (and configuration files) are installed to.
            var loadedFromDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Directory.SetCurrentDirectory(loadedFromDir);

            // Create the initialConfigurationBuilder for this genericHost. This creates an ordered chain of configuration providers. The first providers in the chain have the lowest priority, the last providers in the chain have a higher priority.
            //  Initial configuration does not take Environment into account. 
            var initialGenericHostConfigurationBuilder = new ConfigurationBuilder()
                // Start with a "compiled-in defaults" for anything that is REQUIRED to be provided in configuration for Production
                .AddInMemoryCollection(genericHostConfigurationCompileTimeProduction)
                // SetBasePath creates a Physical File provider, which will be used by the following methods
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(genericHostSettingsFileName +hostSettingsFileNameSuffix, optional: true)
                .AddEnvironmentVariables(prefix: ASPNETCOREEnvironmentVariablePrefix)
                .AddEnvironmentVariables(prefix: CustomEnvironmentVariablePrefix)
                .AddCommandLine(args);

            // Create this program's initial ConfigurationRoot
            var initialGenericHostConfigurationRoot = initialGenericHostConfigurationBuilder.Build();

            // Determine the environment (Debug, TestingUnit, TestingX, QA, QA1, QA2, ..., Staging, Production) to use from the initialGenericHostConfigurationRoot
            var initialEnvName = initialGenericHostConfigurationRoot.GetValue<string>(EnvironmentConfigRootKey, EnvironmentDefault);
            Log.Debug("in Program.Main: initialEnvName from initialGenericHostConfigurationRoot = {InitialEnvName}", initialEnvName);

            // declare the final ConfigurationRoot for this genericHost, and set it to the initialGenericHostConfigurationRoot
            IConfigurationRoot genericHostConfigurationRoot = initialGenericHostConfigurationRoot;

            // If the initialGenericHostConfigurationRoot specifies the Environment is production, then the final genericHostConfigurationRoot is correect 
            //   but if not, build a 2nd genericHostConfigurationBuilder and .Build it to create the genericHostConfigurationRoot

            // Validate the environment provided is one this progam understands how to use, and create the final genericHostConfigurationRoot
            // The first switch statement in the following block also provides validation the the initialEnvName is one that thisprogram understands how to use
            if (initialEnvName!=EnvironmentProduction) {
                // Recreate the ConfigurationBuilder for this genericHost, this time including environment-specific configuration providers.
                IConfigurationBuilder genericHostConfigurationBuilder = new ConfigurationBuilder()
                    // Start with a "compiled-in defaults" for anything that is REQUIRED to be provided in configuration for Production
                    .AddInMemoryCollection(genericHostConfigurationCompileTimeProduction);
                // Add environment-specific "compiled-in defaults"
                switch (initialEnvName) {
                    case EnvironmentDevelopment:
                        genericHostConfigurationBuilder.AddInMemoryCollection(genericHostConfigurationCompileTimeDevelopment);
                        break;
                    case EnvironmentProduction:
                        throw new InvalidOperationException(String.Format(InvalidCircularEnvironmentExceptionMessage, initialEnvName));
                    default:
                        throw new NotImplementedException(String.Format(InvalidSupportedEnvironmentExceptionMessage, initialEnvName));
                }
                // SetBasePath creates a Physical File provider, which will be used by the following methods that read files
                genericHostConfigurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(genericHostSettingsFileName + hostSettingsFileNameSuffix, optional: true);
                // Add environment-specific settings file
                switch (initialEnvName) {
                    case EnvironmentDevelopment:
                        genericHostConfigurationBuilder.AddJsonFile(genericHostSettingsFileName + "." +initialEnvName+ hostSettingsFileNameSuffix, optional: true);
                        break;
                    case EnvironmentProduction:
                        throw new InvalidOperationException(String.Format(InvalidCircularEnvironmentExceptionMessage, initialEnvName));
                    default:
                        throw new NotImplementedException(String.Format(InvalidSupportedEnvironmentExceptionMessage, initialEnvName));
                }
                genericHostConfigurationBuilder
                    .AddEnvironmentVariables(prefix: ASPNETCOREEnvironmentVariablePrefix)
                    .AddEnvironmentVariables(prefix: CustomEnvironmentVariablePrefix)
                    .AddCommandLine(args);
                // Set the final genericHostConfigurationRoot to the .Build() results
                genericHostConfigurationRoot=genericHostConfigurationBuilder.Build();
            }

            // Validate that the current Environment matches the Environment from the initialConfigurationRoot
            var envName = genericHostConfigurationRoot.GetValue<string>(EnvironmentConfigRootKey, EnvironmentDefault);
            //Log.Debug($"in Program.Main: envName from genericHostConfigurationRoot = {envName}");
            if (initialEnvName != envName) {
                throw new InvalidOperationException(String.Format(InvalidRedeclarationOfEnvironmentExceptionMessage, initialEnvName, envName));
            }

            // Validate the value of WebHostBuilderToBuild from the genericHostConfigurationRoot is one that is supported by this program
            var webHostBuilderName = genericHostConfigurationRoot.GetValue<string>(WebHostBuilderToBuildConfigRootKey, WebHostBuilderStringDefault);
            SupportedWebHostBuilders webHostBuilderToBuild;
            if (!Enum.TryParse<SupportedWebHostBuilders>(webHostBuilderName, out webHostBuilderToBuild)) {
                throw new InvalidDataException(String.Format(InvalidWebHostBuilderStringExceptionMessage, webHostBuilderName));
            }
            Log.Debug("in Program.Main: webHostBuilderToBuild = {V}", webHostBuilderToBuild.ToString());

            // Setup the Microsoft.Logging.Extensions Logging
            // One of what seems to me to be a limitation, is, the configuration needs to exist before logging can be read from it, so, 
            //    the whole process of getting the environment above, has to be done without the loggers. That seems... wrong?

            // Serilog is the logging provider I picked to provide a logging solution more robust than NLog/
            //  MLE is anacroynm for Microsoft.Logging.Extensions
            //  Serilog.ILogger MLELog;

            // Enable Serilog's internal debug logging. Note that internal logging will not write to any user-defined sinks
            //  https://github.com/serilog/serilog-sinks-file/blob/dev/example/Sample/Program.cs
            SelfLog.Enable(Console.Out);
            // Another example is at https://stackify.com/serilog-tutorial-net-logging/
            //  This brings in the System.Diagnostics.Debug namespace and writes the SelfLog there
            // Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            // The Serilog.Log is a static entry to the Serilog logging provider
            // Create a Serilog logger based on the ConfigurationRoot and assign it to the static Serilog.Log object



            // Configure logging based on the information in ConfigurationRoot
            // Example of setting up Serilogger in configuration
            Serilog.Core.Logger x = new LoggerConfiguration().ReadFrom.Configuration(genericHostConfigurationRoot).CreateLogger();
            
             // Example of setting up Serilogger in code, instead of configuration
            Serilog.Core.Logger y = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                //.Enrich.WithHttpRequestId()
                //.Enrich.WithUserName()
                //.WithExceptionDetails()
                .WriteTo.Seq(serverUrl: "http://localhost:5341")
                //.WriteTo.Udp(remoteAddress:IPAddress.Loopback, remotePort:9999, formatter:default) // I could not get it to write to Sentinel
                .WriteTo.Console(theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code)
                .WriteTo.Debug()
                .WriteTo.File(path:"Logs/Demo.Serilog.{Date}.log", fileSizeLimitBytes:1024, outputTemplate:"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}", rollingInterval:RollingInterval.Day, rollOnFileSizeLimit:true, retainedFileCountLimit:31)
                .CreateLogger();
            
            // Select which CoreLogger to use
            Serilog.Log.Logger=x;

            // Insert a breakpoint here, add a watch for Serilog.Log.Logger, then drilldown on the Non-Public members to find the hierarchy of sinks, 
            //   and validate the logging sinks that are present are the same as what is defined, either in code or in the ConfigurationRoot, are present
            DemoETWProvider.Log.Information("in Program.Main ETW Information message Environment= { @envName}");
          //  DemoETWProvider.Log.Critical("in Program.Main ETW Critical messagetest");

            Log.Debug("Environment = {@envName}", envName);
            Log.Debug("webHostBuilderToBuild = {@webHostBuilderToBuild}", webHostBuilderToBuild);

            // Create the GenericHostBuilder instance based on the ConfigurationRoot
            Log.Debug("in Program.Main: create genericHostBuilder by calling static method CreateSpecificHostBuilder");
            IHostBuilder genericHostBuilder = CreateSpecificHostBuilder(args, genericHostConfigurationRoot);
            Log.Debug("in Program.Main: genericHostBuilder.Dump() = {V}", genericHostBuilder.Dump());

            // Create the generic host genericHost
            Log.Debug("in Program.Main: create genericHost by calling .Build() on the genericHostBuilder");
            var genericHost = genericHostBuilder.Build();
            Log.Debug("in Program.Main: genericHost.Dump() = {V}", genericHost.Dump());


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
            Log.Debug("in Program.Main: genericHost.IConfiguration.Dump() = {V}", genericHost.Services.GetService<IConfigurationRoot>().Dump());
            IConfigurationRoot xIConfigurationRoot = genericHost.Services.GetService<IConfigurationRoot>();
            Log.Debug("in Program.Main: genericHost.Configuration.Dump() = {V}", genericHost.Services.GetService<ConfigurationRoot>().Dump());
            IConfigurationRoot xConfigurationRoot = genericHost.Services.GetService<ConfigurationRoot>();
            //  Inspect the the InternalHost injected service; this is the WebHost. Then inspect it's configuration provider.
            //  On my system, it is number 21 (the last one)
            //  Expanding the non-public members in the debugger, at the bottom of the heap are the things that wrap up a webhost, things like 
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
            Log.Debug("in Program.Main: genericHost created, starting RunAsync, listening on {V} and awaiting it", genericHostConfigurationRoot.GetValue<string>(URLSConfigRootKey));
            await genericHost.RunAsync();
            Log.Debug("in Program.Main: genericHost.RunAsync called at {Now}, listening on {V}", DateTime.Now, genericHostConfigurationRoot.GetValue<string>(URLSConfigRootKey));

            // Hold the Console window open as we await the webHost
            Console.WriteLine("press any key to close the generic hosting environment that is running as a ConsoleApp");
            Console.ReadKey();
            Log.Debug("in Program.Main: Leaving Program.Main");

            Log.CloseAndFlush();
        }


        #region genericHostBuilder creation / configuration
        // This Builder pattern creates a GenericHostBuilder populated by a specific web host as specified by a paramter
        public static IHostBuilder CreateSpecificHostBuilder(string[] args, IConfigurationRoot genericHostConfigurationRoot) {
            var hb = new HostBuilder()
            // The Generic Host Configuration. 
            .ConfigureHostConfiguration(configHostBuilder => {
                // Start with a "compiled-in defaults" for anything that is required to be provided in configuration for Production
                configHostBuilder.AddInMemoryCollection(genericHostConfigurationCompileTimeProduction);
                // SetBasePath creates a Physical File provider, which will be used by the two following methods
                configHostBuilder.SetBasePath(Directory.GetCurrentDirectory());
                configHostBuilder.AddJsonFile(genericHostSettingsFileName+hostSettingsFileNameSuffix, optional: true);
                configHostBuilder.AddEnvironmentVariables(prefix: ASPNETCOREEnvironmentVariablePrefix);
                configHostBuilder.AddEnvironmentVariables(prefix: CustomEnvironmentVariablePrefix);
                // ToDo: get all (resolved) commandline args from genericHostConfigurationRoot.  Note the following does not include the command line switchMappings
                if (args!=null) {
                    configHostBuilder.AddCommandLine(args);
                }
            })

            // The genericHost loggers
            .ConfigureLogging((genericHostBuilderContext, loggingBuilder) => {
                // clear default loggingBuilder providers
                loggingBuilder.ClearProviders();
                // Read the Logging section of the ConfigurationRoot
                loggingBuilder.AddConfiguration(genericHostBuilderContext.Configuration.GetSection("Logging"));
                // Always provide these loggers regardless of Environment or WebHostBuilderToBuild
                var env = genericHostBuilderContext.Configuration.GetValue<string>(EnvironmentConfigRootKey);
                // use different logging providers based on both Environment and WebHostBuilderToBuild
                switch (env) {
                    case EnvironmentDevelopment:
                        // This is where many developer conveniences are configured for Development environment
                        // In the Development environment, Add Console and Debug Log providers (both are .Net Core provided loggers) 
                        loggingBuilder.AddConsole();
                        loggingBuilder.AddDebug();
                        loggingBuilder.AddSerilog();
                        break;
                    case EnvironmentProduction:
                        loggingBuilder.AddSerilog();
                        break;
                    default:
                        throw new NotImplementedException(String.Format(InvalidSupportedEnvironmentExceptionMessage, env));
                }
                
                //loggingBuilder.AddEventLog();
                //loggingBuilder.AddEventSourceLogger();
                //loggingBuilder.AddTraceSource(sourceSwitchName);
            })

            // the WebHost configuration
            .ConfigureAppConfiguration((genericHostBuilderContext, configWebHostBuilder) => {
                // Start with a "compiled-in defaults" for anything that is required to be provided in configuration  
                configWebHostBuilder.AddInMemoryCollection(webHostConfigurationCompileTimeProduction);
                // Add additional required configuration variables to be provided in configuration for other environments
                string env = genericHostBuilderContext.Configuration.GetValue<string>(EnvironmentConfigRootKey);
                switch (env) {
                    case EnvironmentDevelopment:
                        // This is where many developer conveniences are configured for Development environment
                        // In the Development environment, modify the WebHostBuilder's settings to use the detailed error pages, and to capture startup errors
                        configWebHostBuilder.AddInMemoryCollection(webHostConfigurationCompileTimeDevelopment);
                        break;
                    case EnvironmentProduction:
                        break;
                    default:
                        throw new NotImplementedException(String.Format(InvalidSupportedEnvironmentExceptionMessage, env));
                }
                // webHost configuration can see the global configuration, and will default to using the Physical File provider present in the GenericWebHost'scofiguration
                configWebHostBuilder.AddJsonFile(webHostSettingsFileName, optional: true);
                configWebHostBuilder.AddJsonFile(
                    // ToDo: validate `genericHostBuilderContext.HostingEnvironment.EnvironmentName` has the same value as `env.ToString()`
                    $"webHostSettingsFileName.{genericHostBuilderContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                configWebHostBuilder.AddEnvironmentVariables(prefix: ASPNETCOREEnvironmentVariablePrefix);
                configWebHostBuilder.AddEnvironmentVariables(prefix: CustomEnvironmentVariablePrefix);
                // ToDo: get all (resolved) commandline args from genericHostBuilderContext.Configuration
                configWebHostBuilder.AddCommandLine(args);
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
                // Woraround is to configure the default web server to AllowSynchronousIO=true
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
                // Configure WebHost Logging to use Serilog
                webHostBuilder.UseSerilog();
                // Specify the class to use when starting the WebHost
                webHostBuilder.UseStartup<HostedWebServerStartup>();

                // Prior demos configured the URLs to ListenTo at this step; now its a configuration setting  "urls"
                //  We no longer have to explicitly tell the Demo where to pickup the urls
                //  In fact, there are a number of Environment Variable name patterns that the Environment Variables Configuration Provider 
                //  will pickup by default, as documented 
                //   https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.0#environment-variables-configuration-provider
            });
            return hb;
        }
        #endregion

    }

    // This is the HostedWebServer's Startup class. It is added to the GenericHost's WebHostBuilder's Startup property
    //  An instance is created when the GenericHostBuilder .Builds() the genericHost
    //  and just after creation, this instance's ConfigureServices method is called
    //  After creation
    public class HostedWebServerStartup {
        
        public IConfiguration Configuration { get; }

        // This class gets created by the runtime when .Build is called on the webHostBuilder. The .ctor populates this class' Configuration property .
        public HostedWebServerStartup(IConfiguration configuration) {
            DemoETWProvider.Log.Information("<");
            Log.Debug("in HostedWebServerStartup.ctor; populating the HostedWebServerStartup.Configuration property by Constructior Injection");
            Configuration=configuration;
            Log.Debug("in HostedWebServerStartup.ctor; Configuration.Dump() = {V}", Configuration.Dump());
            DemoETWProvider.Log.Information(">");
        }

        // This method gets called by the runtime after the HostedWebServerStartup.ctor completes.
        //    Use this method to add services to the hostedWebServer container.
        public void ConfigureServices(IServiceCollection services) {
            DemoETWProvider.Log.Information("<");
            // Todo: Logging, environment, configuration, cancellation token?
            Log.Debug("in HostedWebServerStartup.ConfigureServices: no service(s) have been injected in this Demo");
            //Log.Debug($"in HostedWebServerStartup.ConfigureServices; services.Dump() = {services.Dump()}");
            //Log.Debug($"in HostedWebServerStartup.ConfigureServices; Configuration.Dump() = {Configuration.Dump()}");
            DemoETWProvider.Log.Information(">");
        }

        // This method gets called by the runtime after HostedWebServerStartup.ConfigureServices completes. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            DemoETWProvider.Log.Information("<");
            // Looking at env.Dump(), both EnvironmentName and ApplicationName are present
            // the ContentRootPathProvider is a PhysicalFileProvider
            // the ContentRootPath is the current directory, as expected (by default, as we have never set it explicitly)
            //  ToDo: see demoxx, which covers ContentRoot and WebRoot in more detail
            // Interestinlgly (bug in P5?) the WebRootFileProvider key is present, but the value is NullFileProvider
            //  We have not set it explicitly. Perhaps the fallback is ContentRoot and its file provider
            Log.Debug("in HostedWebServerStartup.Configure; env.Dump() = {V}", env.Dump());
            // Looking at the app.Dump()
            // there is an ApplicationServices of type ServiceLookup.ServiceProviderEngineScope, to hold injected services
            // There is a ServerFeatures property, an instance of the type HTTP.Features.FeaturesCollection
            // There is a Properties property, with an instance of a Dictionary<string, object>
            Log.Debug("in HostedWebServerStartup.Configure; app.Dump() = {V}", app.Dump());
            // Dumping the Configuration and comparing it to the .ctor and ConfigureServices, it remains unchanged.
            Log.Debug("in HostedWebServerStartup.Configure; Configuration.Dump() = {V}", Configuration.Dump());
            // Create the ServiceStack middleware instance
            Log.Debug("in HostedWebServerStartup.Configure: Create the SSApp");
            var sSAppHost = new SSAppHost() {
                // Todo: Logging, environment, configuration, cancellation token?
                AppSettings=new NetCoreAppSettings(Configuration) // Use the configuration injected when this instance of Startup was created

            };
            Log.Debug("in HostedWebServerStartup.Configure: SSApp creation is finished");
            // This adds the ServiceStack middleware to the HostedWebServer
            Log.Debug("in HostedWebServerStartup.Configure: add the ServiceStack middleware to the HostedWebServer");
            app.UseServiceStack(sSAppHost);
            Log.Debug("in HostedWebServerStartup.Configure: Provide the terminal middleware handler delegate to the IApplicationBuilder via .Run");
            // The supplied lambda becomes the final handler in the HTTP pipeline
            app.Run(async (context) => {
                Log.Debug("Entering the last HTTP Pipeline handler (returns 404)");
                // ToDo: Respond with contents of index.html? From virtual path ?
                context.Response.StatusCode=404;
                await Task.FromResult(0);
                Log.Debug("leaving the last HTTP Pipeline handler (returns 404)");
            });
            DemoETWProvider.Log.Information(">");
        }
    }
}
