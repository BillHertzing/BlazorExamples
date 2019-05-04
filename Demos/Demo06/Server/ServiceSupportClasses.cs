
/* Hosting the web server inside a service; I cannot get it to work in V30P4
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using ServiceStack.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.ServiceProcess;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Hosting;

namespace Server {
    // Code from https://github.com/aspnet/Hosting/blob/2a98db6a73512b8e36f55a1e6678461c34f4cc4d/samples/GenericHostSample/ServiceBaseLifetime.cs

    public class ServiceBaseLifetime : ServiceBase, IHostLifetime {
        private readonly TaskCompletionSource<object> _delayStart = new TaskCompletionSource<object>();

        public ServiceBaseLifetime(IHostApplicationLifetime hostApplicationLifetime) {
            HostApplicationLifetime=hostApplicationLifetime??throw new ArgumentNullException(nameof(hostApplicationLifetime));
        }

        private IHostApplicationLifetime HostApplicationLifetime { get; }

        public Task WaitForStartAsync(CancellationToken cancellationToken) {
            cancellationToken.Register(() => _delayStart.TrySetCanceled());
            HostApplicationLifetime.ApplicationStopping.Register(Stop);

            new Thread(Run).Start(); // Otherwise this would block and prevent IHost.StartAsync from finishing.
            return _delayStart.Task;
        }

        private void Run() {
            try {
                Run(this); // This blocks until the service is stopped.
                _delayStart.TrySetException(new InvalidOperationException("Stopped without starting"));
            }
            catch (Exception ex) {
                _delayStart.TrySetException(ex);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            Stop();
            return Task.CompletedTask;
        }

        // Called by base.Run when the service is ready to start.
        protected override void OnStart(string[] args) {
            _delayStart.TrySetResult(null);
            base.OnStart(args);
        }

        // Called by base.Stop. This may be called multiple times by service Stop, ApplicationStopping, and StopAsync.
        // That's OK because StopApplication uses a CancellationTokenSource and prevents any recursion.
        protected override void OnStop() {
            HostApplicationLifetime.StopApplication();
            base.OnStop();
        }
    }

    // Code from https://github.com/aspnet/Hosting/blob/2a98db6a73512b8e36f55a1e6678461c34f4cc4d/samples/GenericHostSample/ServiceBaseLifetime.cs

    public static class ServiceBaseLifetimeHostExtensions {
        public static IHostBuilder UseServiceBaseLifetime(this IHostBuilder hostBuilder) {
            return hostBuilder.ConfigureServices((hostContext, services) => services.AddSingleton<IHostLifetime, ServiceBaseLifetime>());
        }

        public static Task RunAsServiceAsync(this IHostBuilder hostBuilder, CancellationToken cancellationToken = default) {
            return hostBuilder.UseServiceBaseLifetime().Build().RunAsync(cancellationToken);
        }
    }
}
*/