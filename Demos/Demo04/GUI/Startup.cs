using Blazor.Extensions.Logging;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GUI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Blazor.Extensions.Logging.BrowserConsoleLogger; taken from the Blazor.Extensions.Logging NuGet package home page https://www.nuget.org/packages/Blazor.Extensions.Logging/# on 6/12/2018
            services.AddLogging(builder => builder
                .AddBrowserConsole() // Register the logger with the ILoggerBuilder
                                     // Setting minimum LogLevel to trace enables detailed tracing
                                     // Setting minimum LogLevel to Debug disables detailed tracing, b ut will show LogDebug logging calls
                .SetMinimumLevel(LogLevel.Debug) // Set the minimum log level to Debug
            );
            // Inject ServiceStack client in place of normal HttpClient
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}

