using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server {
    partial class Program {

        // Create the minimal set of Configuration settings that the program needs to startup and run in a production environment
        public static Dictionary<string, string> genericHostConfigurationCompileTimeProduction =
        new Dictionary<string, string>
        {
            {"from", "genericHostConfigurationCompileTimeProduction"},
            {"genericHostConfigurationCompileTimeProduction", "true"},
            {"Environment", Program.EnvironmentProduction},
            {"WebHostBuilderToBuild", SupportedWebHostBuilders.KestrelAloneWebHostBuilder.ToString()},
            // {"urls", "http://localhost:21020/"}, // ToDo: investigate the impact of UseIIS on urls, and the location of UseIIS in the static generichostbuilder
        };
        // Create the additional/changed set of Configuration settings that the program needs to startup and run in a development environment
        public static Dictionary<string, string> genericHostConfigurationCompileTimeDevelopment =
        new Dictionary<string, string>
        {
            {"from", "genericHostConfigurationCompileTimeDevelopment"},
            {"genericHostConfigurationCompileTimeDevelopment", "true"},
            {"Environment", Program.EnvironmentDevelopment},
            {"WebHostBuilderToBuild", SupportedWebHostBuilders.IntegratedIISInProcessWebHostBuilder.ToString()},
            // {"urls", "http://localhost:21010/"}, // ToDo: investigate the impact of UseIIS on urls, and the location of UseIIS in the static generichostbuilder
        };
        // Create the minimal set of Configuration settings that the webhost selected by the generichost needs to startup and run in production
        public static Dictionary<string, string> webHostConfigurationCompileTimeProduction =
        new Dictionary<string, string>
        {
            {"from", "webHostConfigurationCompileTimeProduction"},
            {"webHostConfigurationCompileTimeProduction", "true"},
            {"PhysicalRootPath", "./GUI/dist"},
            {"urls", "http://localhost:21020/"},
    };
        // Create the minimal set of Configuration settings that the webhost selected by the generichost needs to startup and run in production
        public static Dictionary<string, string> webHostConfigurationCompileTimeDevelopment =
        new Dictionary<string, string>
        {
            {"from", "webHostConfigurationCompileTimeDevelopment"},
            {"webHostConfigurationCompileTimeDevelopment", "true"},
            {"urls", "http://localhost:21010/"},
            {"PhysicalRootPath", "../../../../GUI/bin/Debug/netstandard2.0/Publish/GUI/dist"},
        };
    }
}
