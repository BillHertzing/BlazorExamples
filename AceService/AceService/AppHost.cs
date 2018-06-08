using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Ace.AceService.BaseServicesInterface;
using Ace.AceService.GUIServices.Interfaces;
using Funq;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Common;
using ServiceStack.Logging;

namespace Ace.AceService {
    //VS.NET Template Info: https://servicestack.net/vs-templates/EmptyWindowService
    public class AppHost : AppSelfHostBase {
        static readonly ILog Log = LogManager.GetLogger(typeof(AppHost));
        Dictionary<string, Timer> timers;
        List<Task> longRunningTaskList;
        /// Base constructor requires a Name and Assembly where the basic service implementation is located
        public AppHost() : base("AceService", typeof(BaseServices).Assembly) {
            Log.Debug("Entering AppHost Ctor");
            Log.Debug("Leaving AppHost Ctor");
        }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        public override void Configure(Container container) {
            // inject the concrete logging implementation
            Log.Debug($"Entering AppHost.Configure method, container is {container.ToString()}");

      // populate the configuration settings
      // The location should be part of the container IOC injection, (hopefully observable and creating a stream that can be subscribed to by an authorized  client of this service )

      // Start with the AceService.BaseService builtin (compile-time) configuration settings
      // Add the AceService.BaseService builtin (compile-time) configuration settings
      // Add (Superseding any previous values) the App.config file (AKA AceAgent.exe.config after building and at runtime)
      // Add (Superseding any previous values) the optional configuration file for BaseService configuration settings AKA AceService.config
      // ToDo: Add (Superseding any previous values) environment values that match any keys
      var appSettingsBuilder = new MultiAppSettingsBuilder()
                .AddDictionarySettings(DefaultConfiguration.GetIt())
                .AddAppSettings()
                .AddTextFile("./AceService.config");

      // Enable Postman integration
      Plugins.Add(new PostmanFeature());

            // Enable CORS support
            Plugins.Add(new CorsFeature(
               allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
              allowedOrigins: "*",
              allowCredentials: true,
              allowedHeaders: "content-type, Authorization, Accept"));

            // ToDo: Get the list of plugins to install from the configuration settings
            // Create the list of PlugIns to load
            var plugInList = new List<IPlugin>() { new Ace.AceService.GUIServices.Plugin.GUIServicesPlugin() };

            // ToDo: Add configuration setting specific to any plugin prior to loading the PlugIn
            // ToDo: Superseded by an optional configuration file that contains 'recently used' configuration settings
            // Superseded by Environment variables
            appSettingsBuilder.AddEnvironmentalVariables();
      // ToDo Superseded by command line variables
      // ToDo: Validate the final set of settings
      // ToDo:Validate config file location, Logs file location
      // ToDo:Build the final AppSettings
      AppSettings = appSettingsBuilder.Build();

            // Create the basic services observable data structures based on the configuration settings

            // Add data structures and timers to handle the list of LongRunningTasks
            timers = new Dictionary<string, Timer>();
            container.Register<Dictionary<string, Timer>>(c => timers);
            longRunningTaskList = new List<Task>();
            container.Register<List<Task>>(c => longRunningTaskList);

            // Add a timer to check  the status of long running tasks, and attach a callback to the timer
            #region create longRunningTasksCheckTimer, connect callback, and store in container's timers
            Log.Debug("In AppHost.Configure method, creating longRunningTasksCheckTimer");
            var longRunningTasksCheckTimer = new Timer(1000);
            longRunningTasksCheckTimer.AutoReset = true;
            longRunningTasksCheckTimer.Elapsed += new ElapsedEventHandler(LongRunningTasksCheckTimer_Elapsed);
            timers.Add("longRunningTasksCheckTimer", longRunningTasksCheckTimer);
      #endregion create longRunningTasksCheckTimer, connect callback, and store in container's timers

      // Load each plugin here
      foreach (var pl in plugInList)
      {
        Plugins.Add(pl);
      }

      // ToDo: See Issue #8
      // ToDo: (no issue yet) turnoff automatic call to browser
      // disable the metadata page
      this.Config.EnableFeatures = Feature.All.Remove(Feature.Metadata);

      // Turn debug mode for the ACEAgent depending if running in debug mode or release mode
#if Debug
            this.Config.DebugMode = true;
      
#else
      this.Config.DebugMode = false; 
      #endif

            // ToDo place a static, deep-copy of the current application'instance of the configuration settings as the first object in the application's configuration settings history list.

      // start all the timers
      Log.Debug("In AppHost.Configure method, starting all timers");
            longRunningTasksCheckTimer.Start();
      Log.Debug("Leaving AppHost.Configure");
        }

    /// <summary>
    /// Shut down the Web Service
    /// </summary>
    public override void Stop() {
            Log.Debug("Entering AppHost Stop Method");
            var container = base.Container;

            // It is possible that the Stop method is called during service startup, because the service could be failing because of a problem during startup,
            //  so need to check that each of container's disposable items actually exist before disposing of them

            // Stop and dispose of all timers
            Dictionary<string, Timer> timers;
            try
            {
                Log.Debug("In AppHost.Stop method, trying to resolve the timers dictionary");
                timers = container.TryResolve(typeof(Dictionary<string, Timer>)) as Dictionary<string, Timer>;
            }
            catch (Exception ex)
            {
                Log.Debug($"In AppHost.Stop method, resolving the timers dictionary threw exception message: {ex.Message}");
                throw;
            }

            foreach (var t in timers.Values) { t.Stop(); t.Dispose(); }
            // call the ServiceStack AppSelfHostBase Stop method
            Log.Debug("Entering the ServiceStack AppSelfHostBase Stop Method");
            base.Stop();
            Log.Debug("Leaving AppHost Stop Method");
        }
                
        void LongRunningTasksCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Log.Debug("Entering the appHost.LongRunningTasksCheckTimer_Elapsed Method");
            var container = base.Container;
            Dictionary<string, Timer> timers = container.TryResolve(typeof(Dictionary<string, Timer>)) as Dictionary<string, Timer>;
            timers["longRunningTasksCheckTimer"].Stop();
            //Log.Debug("checking for existence of any longRunningTasks");
            List<Task> longRunningTaskList = container.TryResolve(typeof(List<Task>)) as List<Task>;
            //Log.Debug($"There are {longRunningTaskList.Count} tasks in the longRunningTaskList");
            timers["longRunningTasksCheckTimer"].Start();
            //Log.Debug("Leaving the appHost.LongRunningTasksCheckTimer_Elapsed Method");
        }
    }
}
