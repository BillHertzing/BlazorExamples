using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.Extensions.DependencyInjection;
// Both are required for the logger/logging
using Microsoft.Extensions.Logging;
using Blazor.Extensions.Logging;

namespace AceGUI
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(services =>
            {
              // Add Blazor.Extensions.Logging.BrowserConsoleLogger; taken from the Blazor.Extensions.Logging NuGet package home page https://www.nuget.org/packages/Blazor.Extensions.Logging/# on 6/12/2018
              services.AddLogging(builder => builder
                  .AddBrowserConsole() // Register the logger with the ILoggerBuilder
                  .SetMinimumLevel(LogLevel.Debug) // Set the minimum log level to Information
              );
            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
