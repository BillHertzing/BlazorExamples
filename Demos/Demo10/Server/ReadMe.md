# ReadMe for Demo10 Server
Documentation for the Server used in Demo10 can be found here: [Demo10 Server Documentation](Documentation/Details.html)

## Changes to code

## Changes to Program.Main
A static property for the Serilog logger is added to each class.
The Serilog Log.Logger is populatd with a Logger that is created at the beginning of Main, whose configuration is taken from the logging section of ConfigurationRoot.
The static method CreateSpecificHostBuilder adds .ConfigureLogging((genericHostBuilderContext, loggingBuilder) => {...}


## changes to Configuration

### changes to DefaultConfiguration.cs


### changes to appsettings.json and appsettings.{Environment}.json filesfiles
appsettings.json and appsettings.development.json both adds "Logging": {
    "LogLevel": {
		...
	}
  },

LogLevels differ between the production and development LogLevel sections; Development has a "lower" minimum level, picking up Debug and Information levels, with production picking up just Warning and above.

appsettings.json adds logging provider sections for both Serilog and Nlog. 
An absolute file location for production log files is declared
In both loggers, an Internal Error logging file is specified, sharing the same absolute file location.
In both loggers, an asynchronous target for ArchiveFile is declared.
ArchiveFile target has filename template patterns for the current day's file's filename and filename template patterns for the prior day's (rolled) file's filenames. Both Serilog and NLog identify themselves in the filename patterns specific to each log provider.
ArchiveFile target in production are rolled every day, and expire after 30 days

appsettings.development. json extends the logging provider sections for both Serilog and Nlog.
both add Console and Viewer (UDP) synchronous targets.
both modify the ArchiveFile targets as follows
An relative file location for development log files is declared, overriding the production file location. This applies to both the Internal Error log files as well as the program's  log files.

A second logger `SLLog` , for Serilog, is added in Program.main, and is configured to log ASPNetCore internal messages at the Information level

### Changes to project's dependencies
The following are added
Serilog - Serilog is a diagnostic logging library for .NET applications. 
Serilog.Settings.Configuration - A Serilog configuration provider that reads from Microsoft.Extensions.Configuration
Serilog.AspNetCore - This package routes ASP.NET Core log messages through Serilog, so you can get information about ASP.NET's internal operations logged to the same Serilog sinks as your application events.
Serilog.Sinks.File Writes Serilog events to one or more text files.
Serilog.Sinks.Async An asynchronous wrapper for other Serilog sinks.
Serilog.Sinks.Debug - A Serilog sink that writes log events to the debug output window.
Serilog.Sinks.Udp - A Serilog sink A Serilog sink sending UDP packages over the network.
