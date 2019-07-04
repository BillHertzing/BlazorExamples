using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server {
    static public class GenericHostDefaultConfiguration {
        // Create the minimal set of Configuration settings that the Generic Host needs to startup and run in production
        public static Dictionary<string, string> Production =
            new Dictionary<string, string> {
                {"from", "genericHostConfigurationCompileTimeProduction"},
                {"genericHostConfigurationCompileTimeProduction", "true"},
                {"Environment", Program.EnvironmentDevelopment},
                {"WebHostBuilderToBuild", Program.SupportedWebHostBuilders.KestrelAloneWebHostBuilder.ToString()},
            };
    }
    static public class WebHostDefaultConfiguration {
        // Create the minimal set of Configuration settings that the webhost selected by the generichost needs to startup and run in production
        public static Dictionary<string, string> Production =
            new Dictionary<string, string> {
            {"from", "webHostConfigurationCompileTimeDevelopment"},
            {"webHostConfigurationCompileTimeDevelopment", "true"},
            {"urls", "http://localhost:20810/"},
            {"PhysicalRootPath", "./GUI/GUI/dist"},
        };
    }
}
