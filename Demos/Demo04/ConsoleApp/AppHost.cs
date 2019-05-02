using System;
using Funq;
using ServiceStack;
using ServiceStack.Logging;
// Required to serve the Blazor static files
using ServiceStack.VirtualPath;
using CommonDTOs;
using System.Collections.Generic;
using System.Linq;
// Use the Dump() from ServiceStack.text
using ServiceStack.Text;
namespace ConsoleApp {
    //VS.NET Template Info: https://servicestack.net/vs-templates/EmptyWindowService
    public class AppHost : AppSelfHostBase {
        static readonly ILog Log = LogManager.GetLogger(typeof(AppHost));

        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// </summary>
        public AppHost() : base("ConsoleApp", typeof(AppHost).Assembly) {
            Log.Debug("Entering ConsoleApp Ctor");
            Log.Debug("Leaving ConsoleApp Ctor");
        }


        public override void Stop() {
            Log.Debug("Entering AppHost Stop Method");
            // If this ServiceStack application creates objects that implement IDisposable, they need to be disposed of here
            // This sample does not have any objects to dispose, but this override provides logging  when the Stop method is called
            // call the ServiceStack AppSelfHostBase Stop method
            Log.Debug("Entering the ServiceStack AppSelfHostBase Stop Method");
            base.Stop();
            Log.Debug("Leaving AppHost Stop Method");
        }

        public override void Configure(Container container) {

            Log.Debug($"Entering AppHost.Configure method");

            // Blazor requires the delivery of static files ending in certain file suffixes.
            // SS disallows some of these by default, so here we tell SS to allow certain file suffixes
            this.Config.AllowFileExtensions.Add("dll");
            this.Config.AllowFileExtensions.Add("json");
            this.Config.AllowFileExtensions.Add("pdb");

            // // change the default redirect path so that a request that does not match a route will redirect to /index.html
            //this.Config.DefaultRedirectPath = "/index.html";

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
                Log.Debug($"In AppHost.Configure, got an exception when attempting to create a new virtual to physical file system mapping: {e.Message}");
                throw new Exception("Could not create ServiceStack Virtual File Mapping: ", e);
            }

            this.Config.EnableFeatures = Feature.All.Remove(Feature.Metadata);

            // Blazor requires CORS support, enable the ServiceStack feature
            Plugins.Add(new CorsFeature(
               allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
               allowedOrigins: "*",
               allowCredentials: true,
               allowedHeaders: "content-type, Authorization, Accept"));

            Log.Debug("Leaving AppHost.Configure");
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
        public object Post(PostDataReqDTO request)
    {
            Log.Debug("entering PostDataReqDTO Post");
            // simply echo back in the response whatever data came in the request
            return new PostDataRspDTO { StringDataObject = request.StringDataObject };
    }
        #endregion

        #region PostComplexData
        public object Post(ComplexDataReqDTO request)
        {
            Log.Debug("entering PostComplexData Post");
            Log.Debug($"in PostComplexData Post; request: {request.Dump()}");
            // do some simple processing on the request data, then send both the original and the modified complex data objects back in the response.
            ComplexData reqComplexData = request.ComplexData;
            Log.Debug($"in PostComplexData Post; reqComplexData.Dump(): {reqComplexData.Dump()}");
            // Create a response ComplexData
            var now = DateTime.UtcNow;
            ComplexData rspComplexData = new ComplexData(reqComplexData.StringData+"... Right Back At Ya",  now, now-reqComplexData.DateTimeData,   reqComplexData.IntData+1, reqComplexData.DoubleData*2, reqComplexData.DecimalData/10);
                
            Log.Debug($"in PostComplexData Post; rspComplexData.Dump(): {rspComplexData.Dump()}");
            // Create a responseDTO object
            ComplexDataRspDTO complexDataRspDTO = new ComplexDataRspDTO(rspComplexData);
            Log.Debug($"in PostComplexData Post; complexDataRspDTO.Dump() = {complexDataRspDTO.Dump()}");
            Log.Debug("leaving PostComplexData Post");
            return complexDataRspDTO;
        }
        #endregion
        #region PostComplexDataAsDictionary
        public object Post(ComplexDataDictionaryReqDTO request) {
            Log.Debug("entering PostComplexDataAsDictionary Post");
            Log.Debug($"in PostComplexDataAsDictionary Post; request.Dump() = {request.Dump()}");
            // do some simple processing on the request data, then send both the original and the modified complex data objects back in the response.
            // For demo, expect to get a dictionary with just one key:value pair, key is "firstKey"
            ComplexDataDictionary complexDataDictionary = request.ComplexDataDictionary;
            Log.Debug($"in PostComplexDataAsDictionary Post; complexDataDictionary.Dump() = {complexDataDictionary.Dump()}");
            ComplexData complexDataFromReqFirstKey = complexDataDictionary.ComplexDataDict["firstKey"];
            // Create a response ComplexData
            var now = DateTime.UtcNow;
            ComplexData complexDataForRsp = new ComplexData(complexDataFromReqFirstKey.StringData+"... Right Back At Ya", now, now-complexDataFromReqFirstKey.DateTimeData, complexDataFromReqFirstKey.IntData+1, complexDataFromReqFirstKey.DoubleData*2, complexDataFromReqFirstKey.DecimalData/10);
            Log.Debug($"in PostComplexDataAsDictionary Post; complexDataForRsp.Dump(): {complexDataForRsp.Dump()}");
            ComplexDataDictionary rspComplexDataDict = new ComplexDataDictionary(new Dictionary<string, ComplexData>());
            rspComplexDataDict.ComplexDataDict.Add("ComplexObjectReceived", complexDataFromReqFirstKey);
            rspComplexDataDict.ComplexDataDict.Add("ComplexObjectReturned", complexDataForRsp);
            Log.Debug($"in PostComplexDataAsDictionary Post; rspComplexDataDict.Dump: {rspComplexDataDict.Dump()}");
            // Create a responseDTO object
            ComplexDataDictionaryRspDTO complexDataDictionaryRspDTO = new ComplexDataDictionaryRspDTO() { ComplexDataDictionary=rspComplexDataDict };
            Log.Debug($"in PostComplexDataAsDictionary Post; complexDataDictionaryRspDTO.Dump(): {complexDataDictionaryRspDTO.Dump()}");
            Log.Debug("leaving PostComplexDataAsDictionary Post");
            return complexDataDictionaryRspDTO;
        }
        #endregion
    }
}
