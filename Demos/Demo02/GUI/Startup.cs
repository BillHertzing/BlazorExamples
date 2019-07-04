using Blazor.Extensions.Logging;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
// Both are required for the logger/logging
using Microsoft.Extensions.Logging;
//using Blazor.Extensions.Logging;

namespace GUI {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            // Add Blazor.Extensions.Logging.BrowserConsoleLogger; taken from the Blazor.Extensions.Logging NuGet package home page https://www.nuget.org/packages/Blazor.Extensions.Logging/# on 6/12/2018
            services.AddLogging(builder => builder
                // Register the Blazor.Extensions.Logging logger with the Microsoft.Extensions.Logging.ILoggerBuilder
                .AddBrowserConsole()
                // Setting minimum LogLevel to trace enables detailed tracing
                // Setting minimum LogLevel to Debug disables detailed tracing, but will show LogDebug logging calls
                .SetMinimumLevel(LogLevel.Debug) // Set the minimum log level to Debug
            );
        }

        public void Configure(IComponentsApplicationBuilder app) {
            app.AddComponent<App>("app");
        }
    }
}

