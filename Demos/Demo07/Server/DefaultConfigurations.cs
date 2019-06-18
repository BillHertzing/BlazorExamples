using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server {
    partial class Program {

        // Create the minimal set of Configuration settings that the program needs to startup and run in production
        public static Dictionary<string, string> genericHostConfigurationCompileTimeProduction =
        new Dictionary<string, string>
        {
                {"from", "genericHostConfigurationCompileTimeProduction"},
                {"genericHostConfigurationCompileTimeProduction", "true"},
                {"Environment", Program.EnvironmentDevelopment},
                {"WebHostBuilderToBuild", SupportedWebHostBuilders.KestrelAloneWebHostBuilder.ToString()},
                {"urls", "http://localhost:20720/"},
        };
        // Create the additional/changed set of Configuration settings that the program needs to startup and run in development
        public static Dictionary<string, string> genericHostConfigurationCompileTimeDevelopment =
        new Dictionary<string, string>
        {
                {"from", "genericHostConfigurationCompileTimeDevelopment"},
                {"genericHostConfigurationCompileTimeDevelopment", "true"},
                {"Environment", Program.EnvironmentProduction},
                {"WebHostBuilderToBuild", SupportedWebHostBuilders.IntegratedIISInProcessWebHostBuilder.ToString()},
                {"urls", "http://localhost:20700/"},
        };
    }
}
