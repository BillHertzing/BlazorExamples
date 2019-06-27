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
using System.Collections.Generic;

using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using GenericHostExtensions;
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

        public const string ConsoleAppConfigRootKey = "Console";

        // Extend the CommandLine Configuration Provider with these switch mappings
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.0#switch-mappings
        public static readonly Dictionary<string, string> switchMappings =
            new Dictionary<string, string>
            {
                { "-Console", ConsoleAppConfigRootKey },
                { "-C", ConsoleAppConfigRootKey }
            };

        public static async Task Main(string[] args) {

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
                .AddInMemoryCollection(GenericHostDefaultConfiguration.Production)
                // SetBasePath creates a Physical File provider, which will be used by the following methods
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(genericHostSettingsFileName +hostSettingsFileNameSuffix, optional: true)
                .AddEnvironmentVariables(prefix: ASPNETCOREEnvironmentVariablePrefix)
                .AddEnvironmentVariables(prefix: CustomEnvironmentVariablePrefix)
                // Add commandline switch provider and map -console to --console:false
                .AddCommandLine(args, switchMappings);

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
                    .AddInMemoryCollection(GenericHostDefaultConfiguration.Production)
                    // SetBasePath creates a Physical File provider, which will be used by the following methods that read files
                    .SetBasePath(Directory.GetCurrentDirectory())
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
            Log.Debug("in Program.Main: webHostBuilderToBuild = {@webHostBuilderToBuild}", webHostBuilderToBuild.ToString());

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
            using (x) {

                // Insert a breakpoint here, add a watch for Serilog.Log.Logger, then drilldown on the Non-Public members to find the hierarchy of sinks, 
                //   and validate the logging sinks that are present are the same as what is defined, either in code or in the ConfigurationRoot, are present

                Log.Debug("Environment = {@envName}", envName);
                Log.Debug("webHostBuilderToBuild = {@webHostBuilderToBuild}", webHostBuilderToBuild);

                // During Development, the genericHost runs as a ConsoleHost. In production it runs as a service (Windows) or daemon (Linux)
                // Before creating the genericHostBuilder, we need to know if the program should be running as a console host or as a service
                // There are two conditions for this: 
                //  if a debugger is attached at this point in the program's execution
                //  if command line args contains -console true or -c true
                //  previously we added a switchMappings to the CommandlineArgs configuration provider, so, we can just get the value of console from the ConfigurationRoot
                bool isConsoleHost = Debugger.IsAttached||genericHostConfigurationRoot.GetValue(ConsoleAppConfigRootKey, false);
                //  A class that helps determine if running under Windows or Linux
                RuntimeKind runtimeKind = new RuntimeKind(isConsoleHost);
                Log.Debug("in Program.Main: runtimeKind = {@runtimeKind}", runtimeKind);

                // Introduce a Cancellation token source. This is a all-method cancellation token source, that can be used to signal the genericHost regardless of having it configured with a ConsoleApplication lifetime or a Service lifetime
                CancellationTokenSource genericHostCancellationTokenSource = new CancellationTokenSource();
                // and its token
                CancellationToken genericHostCancellationToken = genericHostCancellationTokenSource.Token;

                // Build and run the generic host, either as a ConsoleApp, or as a service/daemon
                // Attribution to https://www.stevejgordon.co.uk/running-net-core-generic-host-applications-as-a-windows-service
                // Attribution to https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/windows-service?view=aspnetcore-3.0

                // Create the GenericHostBuilder instance based on the ConfigurationRoot
                Log.Debug("in Program.Main: create genericHostBuilder by calling static method CreateSpecificHostBuilder");
                IHostBuilder genericHostBuilder = CreateSpecificHostBuilder(args, genericHostConfigurationRoot);
                Log.Debug("in Program.Main: genericHostBuilder = {@genericHostBuilder}", genericHostBuilder);

                Log.Debug("in Program.Main: IsConsoleApplication = {IsConsoleApplication}", runtimeKind.IsConsoleApplication);
                if (!runtimeKind.IsConsoleApplication) {
                    Log.Debug("in Program.Main: extend genericHostBuilder by calling extension method .ConfigureServices and adding to the DI Container a singleton instance of ServerAsService of type IHostLifetime");
                    genericHostBuilder.ConfigureServices((hostContext, services) => services.AddSingleton<IHostLifetime, ServerAsService>());
                    // genericHostBuilder.UseWindowsService(); // hmm latest doc indicates this should be part of P5, but appearently, it is not...
                } else {
                    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.0#runconsoleasync
                    // RunConsoleAsync on the builder is an extension that enables console support, builds and starts the host, and waits for Ctrl+C/SIGINT or SIGTERM to shut down.
                    //  Examining the source code, it configres the .UseConsoleLifetime() method on the builder
                    // https://github.com/aspnet/Hosting/blob/master/src/Microsoft.Extensions.Hosting/HostingHostBuilderExtensions.cs 
                    Log.Debug("in Program.Main: extend genericHostBuilder by calling the extension method UseConsoleLifetime");
                    // ToDo: Replace with the other .ctor and Action<ConsoleLifetimeOptions> lambda that will support selection between the two states of the boolean `SuppressStatusMessages` (from Configuration?)
                    genericHostBuilder.UseConsoleLifetime();
                }

                // Create the generic host genericHost 
                // the RunConsoleAsync extension method on the GenericHostBuilder also builds the genericHost at this point
                // Asking the genericHostBuilder to Build the genericHost causes the following
                //  The HostedWebServerStartup.ctor runs to completetion
                //  The HostedWebServerStartup.ConfigureServices method runs to completetion
                //  The ServerAsService .ctor runs to completion
                Log.Debug("in Program.Main: create genericHost by calling .Build() on the genericHostBuilder");
                var genericHost = genericHostBuilder.Build();
                Log.Debug("in Program.Main: genericHost.Dump() = {@genericHost}", genericHost);

                // Run it Async
                //  the RunConsoleAsync extension method on the GenericHostBuilder also calls RunAsync on the genericHost at this point
                // https://github.com/aspnet/Hosting/blob/master/src/Microsoft.Extensions.Hosting/HostingHostBuilderExtensions.cs 
                Log.Debug("in Program.Main: genericHost created, starting RunAsync, listening on {@urls} and awaiting it", genericHostConfigurationRoot.GetValue<string>(URLSConfigRootKey));
                // Calling RunAsync causes the following:
                //  The ServerAsService.WaitForStartAsync method runs to completetion
                //  The ServerAsService.Run method starts
                //  The ServerAsService.Run method calls Run(this), expecting it to block until the service is stopped or an exception bubbles up 
                await genericHost.RunAsync(genericHostCancellationToken);

                // The IHostLifetime instance methods take over now
                Log.Debug("in Program.Main: Leaving Program.Main");
            }
        }


        #region genericHostBuilder creation / configuration
        // This Builder pattern creates a GenericHostBuilder populated by a specific web host as specified by a paramter
        public static IHostBuilder CreateSpecificHostBuilder(string[] args, IConfigurationRoot genericHostConfigurationRoot) {
            var hb = new HostBuilder()
            // The Generic Host Configuration. 
            .ConfigureHostConfiguration(configHostBuilder => {
                // Start with a "compiled-in defaults" for anything that is required to be provided in configuration for Production
                configHostBuilder.AddInMemoryCollection(GenericHostDefaultConfiguration.Production);
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
                configWebHostBuilder.AddInMemoryCollection(WebHostDefaultConfiguration.Production);
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
                // Specify the URLS the webHost will Listen on, based on information from the ConfiguratioRoot
                webHostBuilder.UseUrls(genericHostConfigurationRoot.GetValue<string>(URLSConfigRootKey));
                // Prior demos configured the URLs to hardcoded ListenTo at this step; now its a configuration setting  "urls"
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
            Log.Debug("entering HostedWebServerStartup.ctor");
            Log.Debug("in HostedWebServerStartup.ctor; populating the HostedWebServerStartup.Configuration property by Constructior Injection");
            Configuration=configuration;
            //Log.Debug($"in HostedWebServerStartup.ctor; Configuration.Dump() = {Configuration.Dump()}");
            Log.Debug("leaving HostedWebServerStartup.ctor");
        }

        // This method gets called by the runtime after the HostedWebServerStartup.ctor completes.
        //    Use this method to add services to the hostedWebServer container.
        public void ConfigureServices(IServiceCollection services) {
            Log.Debug("Entering HostedWebServerStartup.ConfigureServices");
            // Todo: Logging, environment, configuration, cancellation token?
            Log.Debug("in HostedWebServerStartup.ConfigureServices: no service(s) have been injected in this Demo");
            //Log.Debug($"in HostedWebServerStartup.ConfigureServices; services.Dump() = {services.Dump()}");
            //Log.Debug($"in HostedWebServerStartup.ConfigureServices; Configuration.Dump() = {Configuration.Dump()}");
            Log.Debug("Leaving HostedWebServerStartup.ConfigureServices");
        }

        // This method gets called by the runtime after HostedWebServerStartup.ConfigureServices completes. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            Log.Debug("Entering HostedWebServerStartup.Configure");
            Log.Debug("in HostedWebServerStartup.Configure; env.Dump() = {V}", env.Dump());
            Log.Debug("in HostedWebServerStartup.Configure; app.Dump() = {V}", app.Dump());
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

            Log.Debug("Leaving HostedWebServerStartup.Configure");

        }
    }

    // Attribution to https://dejanstojanovic.net/aspnet/2018/june/clean-service-stop-on-linux-with-net-core-21/
    // Attribution to https://www.stevejgordon.co.uk/running-net-core-generic-host-applications-as-a-windows-service
    // Attribution to https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.0
    // Attribution to  https://github.com/aspnet/Hosting/blob/2a98db6a73512b8e36f55a1e6678461c34f4cc4d/samples/GenericHostSample/ServiceBaseLifetime.cs
    // This class imnplements the service injected into the genericHost.  
    // This class implements the methods needed for IHostLifetime (WaitForStartAsync and StopAsync) to be a ConsoleLifetime (IHostLifetime derives from ConsoleLifetime)
    // This class derives from ServiceBase and implements the methods needed for IHostedService (StartAsync and StopAsync) 
    public class ServerAsService : ServiceBase, IHostLifetime, IHostedService {
        IHostApplicationLifetime HostApplicationLifetime;
        ILogger<ServerAsService> logger;
        IHostEnvironment HostEnvironment;
        IConfiguration HostConfiguration;
        CancellationToken CancellationToken;

        // the _delayStart TaskCompletionSource exposes the task, its state and its results, accepts a cancellation request, and supports exception handling 
        private readonly TaskCompletionSource<object> _delayStart = new TaskCompletionSource<object>();

        // This creates an instance of a ServiceBase with the methods needed to control the GenericHost's Lifetime events
        //   the Ctor loads the instance's properties with corresponding instances from the GenericHost when ServerAsService is created by the GenericHost
        //  This class wil be registered as a Service in the GenericHosts Services collection
        public ServerAsService(
            IConfiguration hostConfiguration,
            IHostEnvironment hostEnvironment,
            ILogger<ServerAsService> logger,
            IHostApplicationLifetime hostApplicationLifetime) {
            this.logger=logger??throw new ArgumentNullException(nameof(logger));
            this.logger.LogInformation("Injected logger starting ServerAsService .ctor");
            Log.Debug("Entering ServerAsService .ctor");
            HostApplicationLifetime=hostApplicationLifetime??throw new ArgumentNullException(nameof(hostApplicationLifetime));
            HostEnvironment=hostEnvironment??throw new ArgumentNullException(nameof(hostEnvironment));
            HostConfiguration=hostConfiguration??throw new ArgumentNullException(nameof(hostConfiguration));
            this.logger.LogInformation("leaving ServerAsService .ctor");
            Log.Debug("leaving ServerAsService .ctor");
        }

        #region IHostLifetime and IHostedService Interfaces implementation
        // Used in IHostLifetime interface
        // Not called in ConsoleWindow mode
        // Hosting.Abstractions' StartAsync method calls this method at the start of StartAsync(CancellationToken)
        public Task WaitForStartAsync(CancellationToken cancellationToken) {
            this.logger.LogInformation("ServerAsService.WaitForStartAsync method called.");
            Log.Debug("Entering Program.ServerAsService.WaitForStartAsync method ");
            // Store away the CancellationToken passed as an argument
            CancellationToken=cancellationToken;
            // Register on that cancellationToken an Action that will call TrySetCanceled method on the _delayStart task.
            // This lets the cancellationToken passed into this method  signal to the genericHost an overall request for cancellation 
            CancellationToken.Register(() => _delayStart.TrySetCanceled());

            new Thread(Run).Start(); // Otherwise this would block and prevent IHost.StartAsync from finishing.
            Log.Debug("Leaving Program.ServerAsService.WaitForStartAsync method ");
            this.logger.LogDebug("Leaving Program.ServerAsService.WaitForStartAsync method ");
            return _delayStart.Task;
        }

        // Used in IHostedService interface
        // in ConsoleWindow (debug) mode, this is called after Program.HostedWebServerStartup.Configue completes.
        public Task StartAsync(CancellationToken cancellationToken) {
            this.logger.LogInformation("ServerAsService.StartAsync method called.");
            Log.Debug("Entering Program.ServerAsService.StartAsync method ");
            // Store away the CancellationToken passed as an argument
            CancellationToken=cancellationToken;
            // Register on that cancellationToken an Action that will call TrySetCanceled method on the _delayStart task.
            // This lets the cancellationToken passed into this method  signal to the genericHost an overall request for cancellation 
            CancellationToken.Register(() => _delayStart.TrySetCanceled());
            // Register the methods defined in this class with the three CancellationToken properties found on the IHostApplicationLifetime instance passed to this class in it's .ctor
            HostApplicationLifetime.ApplicationStarted.Register(OnStarted);
            HostApplicationLifetime.ApplicationStopping.Register(OnStopping);
            HostApplicationLifetime.ApplicationStopped.Register(OnStopped);
            Log.Debug("Leaving Program.ServerAsService.StartAsync method ");
            this.logger.LogInformation("Leaving Program.ServerAsService.StartAsync method ");
            return Task.CompletedTask;
        }

        // StopAsync isused in both IHostedService and IHostLifetime interfaces
        // This IS called when the user closes the ConsoleWindow with the windows topright pane "x (close)" icon
        // This IS called when the user hits ctrl-C in the console window
        //  After Ctrl-C and after this method exits, the debugger
        //    shows an unhandled exception: System.OperationCanceledException: 'The operation was canceled.'

        public Task StopAsync(CancellationToken cancellationToken) {
            this.logger.LogInformation("<ServerAsService.StopAsync");
            Log.Debug("Entering Program.ServerAsService.StopAsync method ");
            Log.Debug("in Program.ServerAsService.StopAsync: Calling the ServiceBase.Stop() method ");
            Stop();
            Log.Debug("Leaving Program.ServerAsService.StopAsync method ");
            this.logger.LogDebug(">{@ Method}", "Program.ServerAsService.StopAsync");
            return Task.CompletedTask;
        }
        #endregion

        #region ServiceBase object overriden methods
        // Called by base.Run when the service is ready to start.
        //  base is the ServiceBase this ServerAsSevice descends from.
        //  only 
        protected override void OnStart(string[] args) {
            this.logger.LogInformation("ServerAsService.OnStart method called.");
            Log.Debug("Entering Program.ServerAsService.OnStart method ");
            _delayStart.TrySetResult(null);
            Log.Debug("Leaving Program.ServerAsService.OnStart method ");
            base.OnStart(args);
        }

        // Called by base.Stop. This may be called multiple times by service Stop, ApplicationStopping, and StopAsync.
        // That's OK because StopApplication uses a CancellationTokenSource and prevents any recursion.
        // This IS called when user hits ctrl-C in ConsoleWindow
        //  This IS NOT called when user closes the startup auto browser
        // ToDo:investigate BrowserLink, and multiple browsers effect onthis call
        protected override void OnStop() {
            this.logger.LogInformation("ServerAsService.OnStop method called.");
            Log.Debug("Entering Program.ServerAsService.OnStop method ");
            HostApplicationLifetime.StopApplication();
            base.OnStop();
            Log.Debug("Leaving Program.ServerAsService.OnStop method ");
        }

        // All the other ServiceBase's virtual methods could be overriden here as well, to log them
        #endregion

        #region Event Handlers registered with the HostApplicationLifetime events
        // Registered as a handler with the HostApplicationLifetime.ApplicationStarted event
        // HostApplicationLifetime instance is passed to the ServerAsService ctor
        private void OnStarted() {
            this.logger.LogInformation("OnStarted event handler called.");
            Log.Debug("Entering Program.ServerAsService.OnStarted method from event handler ");

            // Post-startup code goes here  
            Log.Debug("Leaving Program.ServerAsService.OnStarted method from event handler ");
        }

        // Registered as a handler with the HostApplicationLifetime.Application event
        // HostApplicationLifetime instance is passed to the ServerAsService ctor
        // This is NOT called if the ConsoleWindows ends when the connected browser (browser opened by LaunchSettings when starting with debugger)
        //  is closed
        // This IS called if the user hits ctrl-C in the ConsoleWindow
        private void OnStopping() {
            this.logger.LogInformation("OnStopping method called.");
            Log.Debug("Entering Program.ServerAsService.OnStopping method from event handler ");

            // On-stopping code goes here  
            Log.Debug("Leaving Program.ServerAsService.OnStopping method from event handler ");
        }

        // Registered as a handler with the HostApplicationLifetime.ApplicationStarted event
        // HostApplicationLifetime instance is passed to the ServerAsService ctor
        private void OnStopped() {
            this.logger.LogInformation("OnStopped method called.");
            Log.Debug("Entering Program.ServerAsService.OnStopped method from event handler ");

            // Post-stopped code goes here  
            Log.Debug("Leaving Program.ServerAsService.OnStopped method from event handler ");
        }
        #endregion

        // Run method with no arguments
        private void Run() {
            this.logger.LogInformation("Injected logger Run method called.");
            Log.Debug("Entering Program.ServerAsService.Run");
            Log.Debug("In Program.ServerAsService.Run");
            try {
                Log.Debug("in Program.ServerAsService.Run, calling Run(this) on the ServiceBase class, expecting it to block until service stoppped");
                Run(this); // This blocks until the service is stopped.
                Log.Debug("in Program.ServerAsService.Run, returned from Run(this), something has unblocked it");
                _delayStart.TrySetException(new InvalidOperationException("Stopped without starting"));
            }
            catch (Exception ex) {
                Log.Debug(ex,"in Program.ServerAsService.Run, Run(this) threw exception = {Message}", ex.Message);
                _delayStart.TrySetException(ex);
            }
            Log.Debug("Leaving Program.ServerAsService.Run");
        }
    }

}

