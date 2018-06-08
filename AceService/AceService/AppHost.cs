using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Ace.AceService.BaseServicesInterface;
using Ace.AceService.GUIServices.Plugin;
using Funq;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Logging;

namespace Ace.AceService {
    //VS.NET Template Info: https://servicestack.net/vs-templates/EmptyWindowService
    public class AppHost : AppSelfHostBase {
        static readonly ILog Log = LogManager.GetLogger(typeof(AppHost));
    List<Task> longRunningTaskList;
    Dictionary<string, Timer> timers;

    /// Base constructor requires a Name and Assembly where the basic service implementation is located
    public AppHost() : base("AceService", typeof(BaseServices).Assembly) {
        Log.Debug("Entering AppHost Ctor");
      Log.Debug("Leaving AppHost Ctor");
    }

    void LongRunningTasksCheckTimer_Elapsed(object sender, ElapsedEventArgs e) {
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
      try {
          Log.Debug("In AppHost.Stop method, trying to resolve the timers dictionary");
        timers = container.TryResolve(typeof(Dictionary<string, Timer>)) as Dictionary<string, Timer>;
      } catch(Exception ex) {
          Log.Debug($"In AppHost.Stop method, resolving the timers dictionary threw exception message: {ex.Message}");
        throw;
      }

      foreach(var t in timers.Values) {
          t.Stop(); t.Dispose();
      }
      // call the ServiceStack AppSelfHostBase Stop method
      Log.Debug("Entering the ServiceStack AppSelfHostBase Stop Method");
      base.Stop();
      Log.Debug("Leaving AppHost Stop Method");
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
      // Add (Superseding any previous values) environment values that match any keys
      // ToDo: Add (Superseding any previous values) command line variable values that match any keys
      var appSettingsBuilder = new MultiAppSettingsBuilder()
          .AddDictionarySettings(DefaultConfiguration.GetIt())
          .AddAppSettings()
          .AddTextFile("./AceService.config")
          .AddEnvironmentalVariables();
      // Build the base services AppSettings
      AppSettings = appSettingsBuilder.Build();
      // ToDo: Validate the final set of settings
      // ToDo: Validate config file location, Logs file location

      // ToDo: support AppSettings to control the enable/disable of Postman
      // Enable Postman integration
      Plugins.Add(new PostmanFeature());

      // ToDo: support AppSettings to control the enable/disable of CorsFeature
      // Enable CORS support
      Plugins.Add(new CorsFeature(
         allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
         allowedOrigins: "*",
         allowCredentials: true,
         allowedHeaders: "content-type, Authorization, Accept"));

      // ToDo: support AppSettings to control the enable/disable of Metadata Feature
      this.Config.EnableFeatures = Feature.All.Remove(Feature.Metadata);

      // Turn debug mode for the ACEAgent depending if running in debug mode or release mode
#if Debug
      this.Config.DebugMode = true;
#else
      this.Config.DebugMode = false;
#endif

      // Create the basic services observable data structures based on the configuration settings

      // Add a dictionary of timers and a list to hold "long-running tasks" to the IoC container
      #region create a dictionary of timers and register it in the IoC container
      timers = new Dictionary<string, Timer>();
      container.Register<Dictionary<string, Timer>>(c => timers);
      #endregion create a dictionary of timers and register it in the IoC container
      #region create a list of tasks that is intended to hold "long running" tasks and register the list in the IoC container
      longRunningTaskList = new List<Task>();
      container.Register<List<Task>>(c => longRunningTaskList);
      #endregion create a list of tasks that is intended to hold "long running" tasks and register the list in the IoC container

      // Add a timer to check the status of long running tasks, and attach a callback to the timer
      #region create longRunningTasksCheckTimer, connect callback, and store in container's timers
      Log.Debug("In AppHost.Configure method, creating longRunningTasksCheckTimer");
      var longRunningTasksCheckTimer = new Timer(1000);
      longRunningTasksCheckTimer.AutoReset = true;
      longRunningTasksCheckTimer.Elapsed += new ElapsedEventHandler(LongRunningTasksCheckTimer_Elapsed);
      timers.Add("longRunningTasksCheckTimer", longRunningTasksCheckTimer);
      #endregion create longRunningTasksCheckTimer, connect callback, and store in container's timers

      // ToDo: Get the list of plugins to install from the configuration settings, currently hardcoded to load just the GUIServices
      // Create the list of PlugIns to load
      var plugInList = new List<IPlugin>() { new GUIServicesPlugin() };



      // Load each plugin here. Note that plugins may add AppSettings specific to its needs
      foreach (var pl in plugInList) {
          Plugins.Add(pl);
      }

      // ToDo: See Issue #8
      // ToDo place a static, deep-copy of the current application'instance of the configuration settings as the first object in the application's configuration settings history list.

      // start all the timers
      Log.Debug("In AppHost.Configure method, starting all timers");
      longRunningTasksCheckTimer.Start();
      Log.Debug("Leaving AppHost.Configure");
    }
    }
}
