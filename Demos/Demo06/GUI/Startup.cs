using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
// Both are required for the logger/logging
using Microsoft.Extensions.Logging;
using Blazor.Extensions.Logging;
// Required for simple state in browser-local storage
using Blazored.LocalStorage;
// Required for full State service
using GUI.State;

namespace GUI {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            // Add Blazor.Extensions.Logging.BrowserConsoleLogger; taken from the Blazor.Extensions.Logging NuGet package home page https://www.nuget.org/packages/Blazor.Extensions.Logging/# on 6/12/2018
            services.AddLogging(builder => builder
                // Register the Blazor.Extensions.Logging logger with the Microsoft.Extensions.Logging.ILoggerBuilder
                .AddBrowserConsole()
                // Setting minimum LogLevel to Trace enables detailed tracing
                // Setting minimum LogLevel to Debug disables detailed tracing, but will show LogDebug logging calls
                .SetMinimumLevel(LogLevel.Debug) // Set the minimum log level to Debug
            );
            // Add a library that enables local storage on the browser
            //  https://github.com/Blazored/LocalStorage
            services.AddBlazoredLocalStorage();
            // Add State to the DI
            services.AddState();
        }

        public void Configure(IComponentsApplicationBuilder app) {
            app.AddComponent<App>("app");
        }
    }
}

