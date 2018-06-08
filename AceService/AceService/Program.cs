using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;
using Topshelf;

namespace Ace.AceService {
    partial class Program {
        public const string ServiceNameBase = "AceCommander";
        public const string ServiceDisplayNameBase = "AceCommander";
        public const string ServiceDescriptionBase = "AceCommander";
        public const string LifeCycleSuffix =
#if Debug
            "Dev";
#else
            "";
#endif

        public static ILog Log;

        static void Main(string[] args) {
            //To ensure every ServiceStack service uses the same Global Logger, set it before you initialize ServiceStack's AppHost,
            LogManager.LogFactory = new NLogFactory();
            Log = LogManager.GetLogger(typeof(Program));
            Log.Debug("Entering Program.Main");
            // When running as a service, the initial working dir is usually %WinDir%\System32, but the program (and configuration files) is probably installed to a different directory
            // Change the working dir to the location where the Exe and configuration files are installed to.
            var loadedFromDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Log.Debug($"loadedFromDir is {loadedFromDir}");
            System.IO.Directory.SetCurrentDirectory(loadedFromDir);
            // setup shutdown handlers
            // process machine resources
            // confirm parameters
            // setup event handlers
            // start main processing thread
            // Run TopShelf to run the wrapper class around the ServiceStack framework
            Log.Debug("Program.Main calling TopShelf HostFactory.Run");
            HostFactory.Run(x =>
            {
                //x.UseNLog();
                x.Service<TopShelfAroundServiceStackWrapper>();
                x.SetServiceName(ServiceNameBase + LifeCycleSuffix);
                x.SetDisplayName(ServiceDisplayNameBase + LifeCycleSuffix);
                x.SetDescription(ServiceDescriptionBase + LifeCycleSuffix);
                x.StartAutomatically();
                // replace RunAsLocalSystem with RunAs a named user/pwd if needed for different/higher permissions
                x.RunAsLocalSystem();
                //if (promptForCredentialsWhileInstall){
                //    x.RunAsFirstPrompt();
                //} else{
                x.RunAsLocalSystem();
                //}
                x.EnableShutdown();
                //ToDo Implement Squirrel or equivalent to AutoUpdate a service
                //x.AddCommandLineSwitch("squirrel", _ => { });
                //x.AddCommandLineDefinition("firstrun", _ => Environment.Exit(0));
                //x.AddCommandLineDefinition("obsolete", _ => Environment.Exit(0));
                //ToDo: better understand the purpose of withoverlapping
                //x.AddCommandLineDefinition("updated", version => { bool withOverlapping = false; x.UseHostBuilder((env, settings) => new UpdateHostBuilder(env, settings, version, withOverlapping)); });
                //x.AddCommandLineDefinition("install", version => { x.UseHostBuilder((env, settings) => new InstallAndStartHostBuilder(env, settings, version)); });
                //x.AddCommandLineDefinition("uninstall", _ => { x.UseHostBuilder((env, settings) => new StopAndUninstallHostBuilder(env, settings)); });

            });
            Log.Debug("Leaving Program.Main");

            //            // Run the service in a console window when built in Debug configuration, to allow for debugging during development
            //#if DEBUG
            //            Console.WriteLine("Running WinServiceAppHost in Console mode");

            //            try {
            //                appHost.Init();
            //                appHost.Start(appHost.AppSettings.Get<string>("Ace.AceService:ListeningOn"));
            //                Process.Start(appHost.AppSettings.Get<string>("Ace.AceService:ListeningOn"));
            //                Console.WriteLine("Press <CTRL>+C to stop.");
            //                Thread.Sleep(Timeout.Infinite);
            //            } catch(Exception ex) {
            //                Console.WriteLine($"ERROR: {ex.GetType().Name}: {ex.Message}");
            //                throw;
            //            } finally {
            //                appHost.Stop();
            //            }

            //            Console.WriteLine("WinServiceAppHost has finished");

            //#else
            //            //When in RELEASE mode it will run as a Windows Service with the code below

            //            ServiceBase[] ServicesToRun;
            //            ServicesToRun = new ServiceBase[]
            //            {
            //                new WinService(appHost, Configuration[$"AppConfiguration:ListeningOn"]) //appHost.AppSettings.Get<string>("Ace.AceService:ListeningOn")?
            //            };
            //            ServiceBase.Run(ServicesToRun);
            //#endif
            //
            //            Console.ReadLine();
        }
    }
}
