
namespace Server {
    public partial class Program {

        // Create an enumeration for the kinds of WebHostBuilders this program knows how to support
        public enum SupportedWebHostBuilders {
            IntegratedIISInProcessWebHostBuilder,
            KestrelAloneWebHostBuilder
        }

    }
}
