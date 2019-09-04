using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;
using ServiceStack.Text;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.ServiceProcess;
using System.Linq;
using System.Runtime.InteropServices;
using GenericHostExtensions;
using System.ComponentModel;
using System.Collections.Generic;


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

        public const string ConsoleAppConfigRootKey = "Console";

        // Extend the CommandLine Configuration Provider with these switch mappings
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.0#switch-mappings
        public static readonly Dictionary<string, string> switchMappings =
            new Dictionary<string, string>
            {
                { "-Console", ConsoleAppConfigRootKey },
                { "-C", ConsoleAppConfigRootKey }
            };

        // ServiceStack Logging
        static ServiceStack.Logging.ILog Log { get; set; }

        // Helper method to properly combine the prefix with the suffix
        // static string EnvironmentVariableFullName(string name) { return EnvironmentVariablePrefix+name; }

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
                .AddInMemoryCollection(genericHostConfigurationCompileTimeProduction)
                // SetBasePath creates a Physical File provider, which will be used by the following methods
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(genericHostSettingsFileName, optional: true)
                .AddEnvironmentVariables(prefix: ASPNETCOREEnvironmentVariablePrefix)
                .AddEnvironmentVariables(prefix: CustomEnvironmentVariablePrefix)
                // Add commandline switch provider and map -console to --console:false
                .AddCommandLine(args, switchMappings);

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
                    throw new InvalidEnumArgumentException(String.Format(InvalidSupportedEnvironmentExceptionMessage,envName));
            }

            // During Development, the genericHost runs as a ConsoleHost. In production it runs as a service (Windows) or daemon (Linux)
            // Before creating the genericHostBuilder, we need to know if the program should be running as a console host or as a service
            // There are two conditions for this: 
            //  if a debugger is attached at this point in the program's execution
            //  if command line args contains -console true or -c true
            //  previously we added a switchMappings to the CommandlineArgs configuration provider, so, we can just get the value of console from the ConfigurationRoot
            bool isConsoleHost = Debugger.IsAttached||genericHostConfigurationRoot.GetValue(ConsoleAppConfigRootKey, false);
            //  A class that helps determine if running under Windows or Linux
            RuntimeKind runtimeKind = new RuntimeKind(isConsoleHost);

            // Introduce a Cancellation token source. This is a all-method cancellation token source, that can be used to signal the genericHost regardless of having it configured with a ConsoleApplication lifetime or a Service lifetime
            CancellationTokenSource genericHostCancellationTokenSource = new CancellationTokenSource();
            // and its token
            CancellationToken genericHostCancellationToken = genericHostCancellationTokenSource.Token;

            // Build and run the generic host, either as a ConsoleApp, or as a service/daemon
            // Attribution to https://www.stevejgordon.co.uk/running-net-core-generic-host-applications-as-a-windows-service

            // Create the GenericHostBuilder instance based on the ConfigurationRoot
            Log.Debug("in Program.Main: create genericHostBuilder by calling static method CreateGenericHostHostingSpecificWebHostBuilder");
            IHostBuilder genericHostBuilder = CreateSpecificHostBuilder(args, genericHostConfigurationRoot);
            Log.Debug($"in Program.Main: genericHostBuilder.Dump() = {genericHostBuilder.Dump()}");

            if (!runtimeKind.IsConsoleApplication) {
                genericHostBuilder.ConfigureServices((hostContext, services) => services.AddSingleton<IHostLifetime, ServerAsService>());
            } else {
                // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.0#runconsoleasync
                // RunConsoleAsync on the builder is an extension that  enables console support, builds and starts the host, and waits for Ctrl+C/SIGINT or SIGTERM to shut down.
                //  Examining the source code, it configres the .UseConsoleLifetime() method on the builder
                // https://github.com/aspnet/Hosting/blob/master/src/Microsoft.Extensions.Hosting/HostingHostBuilderExtensions.cs 
                genericHostBuilder.UseConsoleLifetime();
            }

            // Create the generic host genericHost 
            // the RunConsoleAsync also builds the genericHost at this point
            Log.Debug("in Program.Main: create genericHost by calling .Build() on the genericHostBuilder");
            var genericHost = genericHostBuilder.Build();
            Log.Debug($"in Program.Main: genericHost.Dump() = {genericHost.Dump()}");

            // Run it Async
            //  Examining the source code, RunConsoleAsync calls .RunAsync() on the genericHost
            // https://github.com/aspnet/Hosting/blob/master/src/Microsoft.Extensions.Hosting/HostingHostBuilderExtensions.cs 
            Log.Debug($"genericHost.RunAsServiceAsync called at {DateTime.Now}, listening on {genericHostConfigurationRoot.GetValue<string>(URLSConfigRootKey)}");
            await genericHost.RunAsync(genericHostCancellationToken);

            // The IHostLifetime instancemethods take over now
            Log.Debug("in Program.Main: Leaving Program.Main");
        }


        #region genericHostBuilder creation / configuration
        // This Builder pattern creates a GenericHostBuilder populated by a specific web host as specified by a paramter
            public static IHostBuilder CreateSpecificHostBuilder(string[] args, IConfigurationRoot genericHostConfigurationRoot) {
                var hb = new HostBuilder()

                    // The Generic Host Configuration. 
                    .ConfigureHostConfiguration(configHost => {
                    // Start with a "compiled-in defaults" for anything that is required to be provided in configuration for Production
                    configHost.AddInMemoryCollection(genericHostConfigurationCompileTimeProduction);
                    // SetBasePath creates a Physical File provider, which will be used by the two following methods
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                        configHost.AddJsonFile(genericHostSettingsFileName+hostSettingsFileNameSuffix, optional: true);
                        configHost.AddEnvironmentVariables(prefix: ASPNETCOREEnvironmentVariablePrefix);
                    // ToDo: get all (resolved) commandline args from genericHostConfigurationRoot
                    configHost.AddCommandLine(args);
                    })

                    // the WebHost configuration
                    .ConfigureAppConfiguration((hostContext, configApp) => {
                    // Start with a "compiled-in defaults" for anything that is required to be provided in configuration  
                    configApp.AddInMemoryCollection(webHostConfigurationCompileTimeProduction);
                    // Add additional required configuration variables to be provided in configuration for other environments
                    string env = genericHostConfigurationRoot.GetValue<string>(EnvironmentConfigRootKey);
                        switch (env) {
                            case EnvironmentDevelopment:
                            // This is where many developer conveniences are configured for Development environment
                            // In the Development environment, modify the WebHostBuilder's settings to use the detailed error pages, and to capture startup errors
                            configApp.AddInMemoryCollection(webHostConfigurationCompileTimeDevelopment);
                                break;
                            case EnvironmentProduction:
                                break;
                            default:
                                throw new NotImplementedException(String.Format(InvalidSupportedEnvironmentExceptionMessage,env));
                        }
                    // webHost configuration can see the global configuration, and will default to using the Physical File provider present in the GenericWebHost'scofiguration
                    configApp.AddJsonFile(webHostSettingsFileName, optional: true);
                        configApp.AddJsonFile(
                        // ToDo: validate `hostContext.HostingEnvironment.EnvironmentName` has the same value as `env.ToString()`
                        $"webHostSettingsFileName.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                        configApp.AddEnvironmentVariables(prefix: CustomEnvironmentVariablePrefix);
                    // ToDo: get all (resolved) commandline args from genericHostConfigurationRoot
                    configApp.AddCommandLine(args);
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
            #endregion

        }

        // This is the HostedWebServer's Startup class. It is added to the GenericHost's Services collection
        //  An instance is created when GenericHost executes it's RunConsoleAsync method
        //  After creation
        public class Startup {
        static readonly ILog Log = LogManager.GetLogger(typeof(Startup));
        public IConfiguration Configuration { get; }

        public IRuntimeKind IsCA { get; }

        //  This constructor is called when GenericHost executes it's Run method
        public Startup(IConfiguration configuration) {
            Log.Debug("entering Program.Startup.ctor");
            Configuration=configuration;
            Log.Debug("leaving Program.Startup.ctor");
        }

        // This method gets called by the runtime after the Startup.ctor completes.
        //    Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            Log.Debug("Entering Program.Startup.ConfigureServices");
            // Todo: Logging, environment, configuration, cancellation token?
            Log.Debug("Leaving Program.Startup.ConfigureServices");
        }

        // This method gets called by the runtime after Startup.ConfigureServices completes. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            Log.Debug("Entering Program.Startup.Configure");
            Log.Debug($"in Program.Startup.ctor RuntimeKind.RuntimeKind = {IsCA.IsConsoleApplication}");
            // Create the ServiceStack middleware instance
            var sSAppHost = new SSAppHost() {
                // Todo: Logging, environment, configuration, cancellation token?
                AppSettings=new NetCoreAppSettings(Configuration) // Use **appsettings.json** and config sources

            };

            // This adds the ServiceStack middleware to the HostedWebServer
            Log.Debug("in Program.Startup.Configure: add the ServiceStack middleware to the HostedWebServer");
            app.UseServiceStack(sSAppHost);

            // The supplied lambda becomes the final handler in the HTTP pipeline
            app.Run(async (context) => {
                Log.Debug("Entering the last HTTP Pipeline handler (returns 404)");
                // ToDo: Respond with contents of index.html? From virtual path ?
                context.Response.StatusCode=404;
                await Task.FromResult(0);
                Log.Debug("leaving the last HTTP Pipeline handler (returns 404)");
            });

            Log.Debug("Leaving Program.Startup.Configure");

        }
    }

    // Attribution to https://dejanstojanovic.net/aspnet/2018/june/clean-service-stop-on-linux-with-net-core-21/
    // Attribution to https://www.stevejgordon.co.uk/running-net-core-generic-host-applications-as-a-windows-service
    // Attribution to https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.0
    // Attribution to  https://github.com/aspnet/Hosting/blob/2a98db6a73512b8e36f55a1e6678461c34f4cc4d/samples/GenericHostSample/ServiceBaseLifetime.cs
    // This is the class hosted within the Generic Host.This class
    public class ServerAsService : ServiceBase, IHostLifetime, IHostedService {
        IHostApplicationLifetime HostApplicationLifetime;
        ILogger<ServerAsService> logger;
        IHostEnvironment HostEnvironment;
        IConfiguration HostConfiguration;
        CancellationToken CancellationToken;
        static ServiceStack.Logging.ILog Log { get; set; } = LogManager.GetLogger(typeof(ServerAsService));

        private readonly TaskCompletionSource<object> _delayStart = new TaskCompletionSource<object>();

        // This creates an instance of the HostedWebServer
        //   the Ctor loads the instance's properties with corresponding instances from the GenericHost
        public ServerAsService(
            IConfiguration hostConfiguration,
            IHostEnvironment hostEnvironment,
            ILogger<ServerAsService> logger,
            IHostApplicationLifetime hostApplicationLifetime) {
            this.logger=logger??throw new ArgumentNullException(nameof(logger));
            this.logger.LogInformation("starting ServerAsService .ctor");
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
        public Task WaitForStartAsync(CancellationToken cancellationToken) {
            this.logger.LogInformation("ServerAsService.WaitForStartAsync method called.");
            Log.Debug("Entering Program.ServerAsService.WaitForStartAsync method ");
            CancellationToken=cancellationToken; // ToDo: better understanding
            CancellationToken.Register(() => _delayStart.TrySetCanceled());

            //HostApplicationLifetime.ApplicationStopping.Register(Stop);

            //HostApplicationLifetime.ApplicationStarted.Register(OnStarted);
            //HostApplicationLifetime.ApplicationStopping.Register(OnStopping);
            //HostApplicationLifetime.ApplicationStopped.Register(OnStopped);

            new Thread(Run).Start(); // Otherwise this would block and prevent IHost.StartAsync from finishing.
            Log.Debug("Leaving Program.ServerAsService.WaitForStartAsync method ");
            return _delayStart.Task;
        }

        // Used in IHostedService interface
        // in ConsoleWindow (debug) mode, this is called after Program.Startup.Configue completes.
        public Task StartAsync(CancellationToken cancellationToken) {
            this.logger.LogInformation("ServerAsService.StartAsync method called.");
            Log.Debug("Entering Program.ServerAsService.StartAsync method ");
            CancellationToken=cancellationToken; // ToDo: better understanding
            CancellationToken.Register(() => _delayStart.TrySetCanceled());
            // Register the methods defined in this class with the HostApplicationLifetime instance passed to this class in it's .ctor
            HostApplicationLifetime.ApplicationStarted.Register(OnStarted);
            HostApplicationLifetime.ApplicationStopping.Register(OnStopping);
            HostApplicationLifetime.ApplicationStopped.Register(OnStopped);
            Log.Debug("Leaving Program.ServerAsService.StartAsync method ");
            return Task.CompletedTask;
        }

        // StopAsync isused in both IHostedService and IHostLifetime interfaces
        // This IS called when the user closes the ConsoleWindow with the windows topright pane "x (close)" icon
        // This IS called when the user hits ctrl-C in the console window
        //  After Ctrl-C and after this method exits, the debugger
        //    shows an unhandled exception: System.OperationCanceledException: 'The operation was canceled.'

        public Task StopAsync(CancellationToken cancellationToken) {
            this.logger.LogInformation("ServerAsService.StopAsync method called.");
            Log.Debug("Entering Program.ServerAsService.StopAsync method ");
            Log.Debug("in Program.ServerAsService.StopAsync: Calling the ServiceBase.Stop() method ");
            Stop();
            Log.Debug("Leaving Program.ServerAsService.StopAsync method ");
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
            this.logger.LogInformation("Run method called.");
            Log.Debug("Entering Program.ServerAsService.Run");
            try {
                Run(this); // This blocks until the service is stopped.
                _delayStart.TrySetException(new InvalidOperationException("Stopped without starting"));
            }
            catch (Exception ex) {
                _delayStart.TrySetException(ex);
            }
            Log.Debug("Leaving Program.ServerAsService.Run");
        }
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
                                .ConfigureLogging((hostContext, hostLoggingBuilder) => {
                                    hostLoggingBuilder.AddConsole();
                                    hostLoggingBuilder.AddDebug();
                                })
               ;
           if (isService) {
               await builder.RunAsServiceAsync();
           } else {
               await builder.RunConsoleAsync();
           }

   }

*/
