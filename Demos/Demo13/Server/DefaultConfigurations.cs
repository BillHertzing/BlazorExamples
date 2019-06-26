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
            {"WebHostBuilderToBuild", SupportedWebHostBuilders.KestrelAloneWebHostBuilder.ToString()},
            {"Environment", SupportedEnvironments.Production.ToString()},
    };
        // Create the additional/changed set of Configuration settings that the program needs to startup and run in development
        public static Dictionary<string, string> genericHostConfigurationCompileTimeDevelopment =
    new Dictionary<string, string>
    {
            {"from", "genericHostConfigurationCompileTimeDevelopment"},
            {"genericHostConfigurationCompileTimeDevelopment", "true"},
            {"WebHostBuilderToBuild", SupportedWebHostBuilders.IntegratedIISInProcessWebHostBuilder.ToString()},
            {"Environment", SupportedEnvironments.Development.ToString()},
    };
        // Create the minimal set of Configuration settings that the webhost selected by the generichost needs to startup and run in production
        public static Dictionary<string, string> webHostConfigurationCompileTimeProduction =
    new Dictionary<string, string>
    {
            {"from", "webHostConfigurationCompileTimeProduction"},
            {"webHostConfigurationCompileTimeProduction", "true"},
            {"urls", "http://localhost:21099/"},
    };
        // Create the minimal set of Configuration settings that the webhost selected by the generichost needs to startup and run in production
        public static Dictionary<string, string> webHostConfigurationCompileTimeDevelopment =
    new Dictionary<string, string>
    {
            {"from", "webHostConfigurationCompileTimeDevelopment"},
            {"webHostConfigurationCompileTimeDevelopment", "true"},
            {"urls", "http://localhost:21098/"},
    };

    }
}
