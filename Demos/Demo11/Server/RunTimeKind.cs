using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace GenericHostExtensions {

    // ToDo: Move into a separate assembly dedicated to extending GenericHost to make it easy to run as a Console or windows service or Linux Daemon
    public enum RuntimePlatformLifetime {
        WindowsConsoleApp,
        WindowsService,
        LinuxConsoleApp,
        LinuxDaemon,
        FreeBSDConsoleApp,
        FreeBSDDaemon,
        OSXConsoleApp,
        OSXDaemon
    }

    public interface IRuntimeKind {
        bool IsConsoleApplication { get; }
        bool IsFreeBSD { get; }
        bool IsLinux { get; }
        bool IsOSX { get; }
        bool IsWindows { get; }
        RuntimePlatformLifetime Kind { get; }
    }

    public class RuntimeKind : IRuntimeKind {
        const string UnknownRunTimeInformationOSPlatformExceptionMessage = "unknown RuntimeInformation OSPlatform : {0}";
        public RuntimeKind(bool isCA) {
            IsConsoleApplication=isCA;
            RuntimePlatformLifetime kind;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                kind=isCA ? RuntimePlatformLifetime.WindowsConsoleApp : RuntimePlatformLifetime.WindowsService;
                IsWindows=true;
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                kind=isCA ? RuntimePlatformLifetime.LinuxConsoleApp : RuntimePlatformLifetime.LinuxDaemon;
                IsLinux=true;
                //  } else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD)) {
                //     kind=isCA ? RuntimePlatformLifetime.FreeBSDConsoleApp : RuntimePlatformLifetime.FreeBSDDaemon;
                //     IsFreeBSD=true;
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                kind=isCA ? RuntimePlatformLifetime.OSXConsoleApp : RuntimePlatformLifetime.OSXDaemon;
                IsOSX=true;
            } else {
                // ToDo: replace exception message with a string constant
                throw new InvalidOperationException(String.Format(UnknownRunTimeInformationOSPlatformExceptionMessage, RuntimeInformation.FrameworkDescription));
            }
        }
        public RuntimePlatformLifetime Kind { get; private set; }
        public bool IsConsoleApplication { get; private set; } = false;
        public bool IsWindows { get; private set; } = false;
        public bool IsLinux { get; private set; } = false;
        public bool IsOSX { get; private set; } = false;
        public bool IsFreeBSD { get; private set; } = false;
    }
}
