using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server {
    public partial class Program {

        // Create an enumeration for the kinds of WebHostBuilders this program knows how to support
        public enum SupportedWebHostBuilders {
            IntegratedIISInProcessWebHostBuilder,
            KestrelAloneWebHostBuilder
        }

        // Create an enumeration for the kinds of environments this program knows how to support
        public enum SupportedEnvironments {
            Development,
            Production
        }
    }
}
