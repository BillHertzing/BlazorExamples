using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ServiceStack.Logging;
// Ths example uses NLog for the logger, be sure it is installed on your system, or, change the logging framework
using ServiceStack.Logging.NLogger;

// This demo is intended to show SS running as a  static file server and REST micro-service server
//  as a DotNet Core 3.0 (preview 4) Console App

namespace ConsoleApp {
    class Program {
        static ILog Log { get; set; }

        static void Main(string[] args) {

            // To ensure every class uses the same Global Logger, set the LogManager's LogFactory before initializing the ServiceStack's AppHost
            LogManager.LogFactory = new NLogFactory();
            // Create a logger instance for this class
            Log = LogManager.GetLogger(typeof(Program));
            Log.Debug("Entering Program.Main");

            // Change the working dir to the location where the Exe and configuration files are installed to.
            var loadedFromDir =
              Path
              .GetDirectoryName(Assembly
              .GetExecutingAssembly()
                .Location);
            Log.Debug($"loadedFromDir is {loadedFromDir}");
            Directory.SetCurrentDirectory(loadedFromDir);

            // set the port on which this ServiceStack application will listen.
            var listeningOn = "http://localhost:21200/";

            // Create the Host via a HostBuilder, and run it
            CreateHostBuilder(args).Build().Run();

            //var appHost = new AppHost()
            //  .Init()
            // .Start(listeningOn);

            Log.Debug($"AppHost Created at {DateTime.Now}, listening on {listeningOn}");

            Console.WriteLine("press any key to close the ServiceStack app");
            Console.ReadKey();
            Log.Debug("Leaving Program.Main");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args).
        );

    }
}
