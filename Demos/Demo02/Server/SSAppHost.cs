// Use the common definitions of the data to pass between the GUI and the Server
using CommonDTOs;
// Define the Container being used when configuring the SSApp
using Funq;
using System;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Text;
// Required to serve the Blazor static files
using ServiceStack.VirtualPath;
// Added to support the use of Dictionary in this Demo 
using System.Collections.Generic;

namespace Server {

    public class SSAppHost : AppHostBase {
        static readonly ILog Log = LogManager.GetLogger(typeof(SSAppHost));

        public const string CouldNotCreateServiceStackVirtualFileMappingExceptionMessage = "Could not create ServiceStack Virtual File Mapping: ";

        public SSAppHost() : base("SSServer", typeof(SSAppHost).Assembly) {
            Log.Debug("Entering SSAppHost .ctor");
            Log.Debug("Leaving SSAppHost .ctor");
        }

        public override void Configure(Container container) {
            Log.Debug($"Entering SSAppHost.Configure method");

            // Blazor requires the delivery of static files ending in certain file suffixes.
            // SS disallows some of these by default, so here we tell SS to allow certain file suffixes
            this.Config.AllowFileExtensions.Add("dll");
            this.Config.AllowFileExtensions.Add("json");
            this.Config.AllowFileExtensions.Add("pdb");

            // change the default redirect path so that a request that does not match a route will redirect to /index.html
            this.Config.DefaultRedirectPath="/index.html";

            // Tell ServiceStack where to find the static files of the Blazor application.
            // In a real application, this can be quite complicated, if users are allowed to install the application in custom locations
            // The desired end result is to know where to find the Blazor App's static files.
            // This example is built inside of Visual Studio 2017, with both the ConsoleApp and the GUI projects sharing the same parent directory, the $SolutionDir.
            // When the ConsoleApp is run inside of VS by hitting F5 or shift-F5, VS builds the Console App in
            // $ProjectDir + "bin/<Config>/<framework>"
            // The GUI project has a Publish Profile for DebugProfile that publishes to the filesystem in a location relative to the GUI $ProjectDir
            //  at $ProjectDir/bin/<Config>/netstandard2.0/Publish/dist/GUI 

            // Assuming you build and run this example in VS 2017 set to Debug configuration targeting the .Net standard 2.0 framework
            var physicalRootPath = "../../../../GUI/bin/Debug/netstandard2.0/Publish/GUI/dist";

            // For this demo, set the virtual path to the empty string
            var virtualRootPath = "";

            // Map the Virtual root Dir to the physical path of the root of the GUI
            // Wrap in a try catch block in case the physicalRootPath does not exist
            try {
                this.AddVirtualFileSources
                    .Add(new FileSystemMapping(virtualRootPath, physicalRootPath));
            }
            catch (Exception e) {
                Log.Debug($"In SSAppHost.Configure, got an exception when attempting to create a new virtual to physical file system mapping: {e.Message}");
                throw new InvalidOperationException(CouldNotCreateServiceStackVirtualFileMappingExceptionMessage, e);
            }

            // Remove the ServieStack metadata feature, as it overrides the default behaviour expected when a request for a root resource arrives, and the root resource iis not found
            this.Config.EnableFeatures=Feature.All.Remove(Feature.Metadata);

            // Blazor requires CORS support, enable the ServiceStack CORS feature
            Plugins.Add(new CorsFeature(
               allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
               allowedOrigins: "*",
               allowCredentials: true,
               allowedHeaders: "content-type, Authorization, Accept"));

            Log.Debug("Leaving SSAppHost.Configure");
        }
    }

    // Create the Service that will handle the Initialization and PostData REST routes
    public class BaseServices : Service {
        static readonly ILog Log = LogManager.GetLogger(typeof(BaseServices));
        
        #region BaseServices Initialization
        public object Post(InitializationReqDTO request) {
            return new InitializationRspDTO { };
        }
        #endregion

        #region BaseServices PostData
        public object Post(PostDataReqDTO request) {
            Log.Debug("entering PostDataReqDTO Post");
            // simply echo back in the response whatever data came in the request
            return new PostDataRspDTO { StringDataObject=request.StringDataObject };
        }
        #endregion

        #region BaseServices PostComplexDataAsString
        public object Post(ComplexDataAsStringReqDTO request) {
            Log.Debug("entering PostComplexDataAsString Post");
            Log.Debug($"in PostComplexDataAsString Post; requestDumped: {request.Dump()}");
            // do some simple processing on the request data, then send both the original and the modified complex data objects back in the response.
            // For demo02, expect to get a string that has the ComplexData serialized into it.
            string reqComplexDataAsString = request.ComplexDataAsString;
            Log.Debug($"in PostComplexDataAsString Post; reqComplexDataAsStringDumped: {reqComplexDataAsString.Dump()}");
            // Deserialize the data from the request to a ComplexData using the ServiceStack Json-deserializer
            ComplexData reqComplexData = ServiceStack.Text.JsonSerializer.DeserializeFromString<ComplexData>(reqComplexDataAsString);
            Log.Debug($"in PostComplexDataAsString Post; reqComplexDataDumped: {reqComplexData.Dump()}");
            // Create a response ComplexData
            var now = DateTime.UtcNow;
            ComplexData rspComplexData = new ComplexData(reqComplexData.StringData+"... Right Back At Ya", DateTime.UtcNow, reqComplexData.IntData+1, reqComplexData.DoubleData*2, reqComplexData.DecimalData/10); // DateTimeData = now, TimeSpanData = now - reqComplexData.DateTimeData,

            Log.Debug($"in PostComplexDataAsString Post; rspComplexDataDumped: {rspComplexData.Dump()}");
            // Serialize it to a string using ServiceStack Serializer
            string rspComplexDataAsString = ServiceStack.Text.JsonSerializer.SerializeToString<ComplexData>(rspComplexData);
            Log.Debug($"in PostComplexDataAsString Post; rspComplexDataAsStringDumped: {rspComplexDataAsString.Dump()}");
            // Create a responseDTO object
            ComplexDataAsStringRspDTO rspComplexDataAsStringDTO = new ComplexDataAsStringRspDTO(rspComplexDataAsString);
            Log.Debug($"in PostComplexDataAsString Post; rspComplexDataAsStringDTODumped = {rspComplexDataAsStringDTO.Dump()}");
            Log.Debug("leaving PostComplexDataAsString Post");
            return rspComplexDataAsStringDTO;
        }
        #endregion

        #region BaseServices PostComplexDataAsDictionary
        public object Post(ComplexDataDictionaryAsStringReqDTO request) {
            Log.Debug("entering PostComplexDataDictionaryAsString Post");
            Log.Debug($"in PostComplexDataDictionaryAsString Post; requestDumped = {request.Dump()}");
            // do some simple processing on the request data, then send both the original and the modified complex data objects back in the response.
            // For demo02, expect to get a dictionary with just one key:value pair, key is "firstKey"
            string reqComplexDataDictionaryAsString = request.ComplexDataDictionaryAsString;
            ComplexDataDictionary reqComplexDataDictionary = ServiceStack.Text.JsonSerializer.DeserializeFromString<ComplexDataDictionary>(reqComplexDataDictionaryAsString);
            Dictionary<string, ComplexData> demo2ComplexDataDict = reqComplexDataDictionary.ComplexDataDict;
            // The complex data sent in the "firstKey"
            ComplexData reqComplexData = demo2ComplexDataDict["firstkey"];
            // Create a response ComplexData
            //var now = DateTime.UtcNow;
            ComplexData rspComplexData = new ComplexData(reqComplexData.StringData+"... Right Back At Ya", DateTime.UtcNow, reqComplexData.IntData+1, reqComplexData.DoubleData*2, reqComplexData.DecimalData/10); // DateTimeData = now, TimeSpanData = now - reqComplexData.DateTimeData
            Log.Debug($"in PostComplexDataDictionaryAsString Post; rspComplexData: {rspComplexData.Dump()}");
            ComplexDataDictionary rspComplexDataDict = new ComplexDataDictionary(new Dictionary<string, ComplexData>());
            rspComplexDataDict.ComplexDataDict.Add("ComplexObjectReceived", reqComplexData);
            rspComplexDataDict.ComplexDataDict.Add("ComplexObjectReturned", rspComplexData);
            Log.Debug($"in PostComplexDataDictionaryAsString Post; rspComplexDataDict: {rspComplexDataDict.Dump()}");
            // Serialize it into a string
            string rspComplexDataDictionaryAsString = ServiceStack.Text.JsonSerializer.SerializeToString<ComplexDataDictionary>(rspComplexDataDict);
            Log.Debug($"in PostComplexDataDictionaryAsString Post; rspComplexDataAsString: {rspComplexDataDictionaryAsString}");
            // Create a responseDTO object
            ComplexDataDictionaryAsStringRspDTO rspComplexDataDictionaryAsStringDTO = new ComplexDataDictionaryAsStringRspDTO() { ComplexDataDictionaryAsString=rspComplexDataDictionaryAsString };
            Log.Debug($"in PostComplexDataDictionaryAsString Post; rspComplexDataDictionaryAsStringDTO: {rspComplexDataDictionaryAsStringDTO.Dump()}");
            Log.Debug("leaving PostComplexDataDictionaryAsString Post");
            return rspComplexDataDictionaryAsStringDTO;
        }
        #endregion
    }
}
