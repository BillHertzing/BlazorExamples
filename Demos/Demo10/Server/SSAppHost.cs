// Use the common defintions of the data to pass between the GUI and the Server
using CommonDTOs;
// Define the Container being used when configuring the SSApp
using Funq;
using System;
using Serilog;
using ServiceStack;
using ServiceStack.Text;
using ServiceStack.VirtualPath;
using System.Collections.Generic;

namespace Server {

    public class SSAppHost : AppHostBase {

        public const string CouldNotCreateServiceStackVirtualFileMappingExceptionMessage = "Could not create ServiceStack Virtual File Mapping: ";

        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// </summary>
        public SSAppHost() : base("SSServer", typeof(SSAppHost).Assembly) {
            DemoETWProvider.Log.MethodBoundry("<");
            // Log.Debug("in SSAppHost .ctor, base.Configuration.Dump() = {V}", base.Configuration.Dump());
            Log.Debug("Leaving SSAppHost Ctor");
            DemoETWProvider.Log.MethodBoundry(">");
        }


        //public override void Stop() {
        //     DemoETWProvider.Log.MethodBoundry("<");
        //    // If this ServiceStack application creates objects that implement IDisposable, they need to be disposed of here
        //    // This sample does not have any objects to dispose, but this override provides logging  when the Stop method is called
        //    // call the ServiceStack AppSelfHostBase Stop method
        //    Log.Debug("Entering the ServiceStack AppSelfHostBase Stop Method");
        //    base.Stop();
        //    DemoETWProvider.Log.MethodBoundry(">");

        //}

        public override void Configure(Container container) {
            DemoETWProvider.Log.MethodBoundry("<");
            //Log.Debug($"in SSAppHost.Configure, base.Configuration.GetValue<String>(Program.URLSConfigRootKey).Dump() = {base.Configuration.GetValue<String>(Program.URLSConfigRootKey).Dump()}");

            // Blazor requires the delivery of static files ending in certain file suffixes.
            // SS disallows some of these by default, so here we tell SS to allow certain file suffixes
            this.Config.AllowFileExtensions.Add("dll");
            this.Config.AllowFileExtensions.Add("json");
            this.Config.AllowFileExtensions.Add("pdb");

            // // change the default redirect path so that a request that does not match a route will redirect to /index.html
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
                Log.Debug("In SSAppHost.Configure, got an exception when attempting to create a new virtual to physical file system mapping: {Message}", e.Message);
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

            DemoETWProvider.Log.MethodBoundry(">");
        }
    }

    // Create the Service that will handle the Initialization and PostData REST routes
    public class BaseServices : Service {
        #region BaseServices Initialization
        public object Post(InitializationReqDTO request) {
            DemoETWProvider.Log.MethodBoundry("<");
            // V30P4 has an error trying to serialize an empty response, so the InitializationRspDTO has been modified to return a string
            var rsp = new InitializationRspDTO();
            DemoETWProvider.Log.MethodBoundry(">");
            return rsp;
        }
        #endregion
        #region BaseServices PostData
        public object Post(PostDataReqDTO request) {
            DemoETWProvider.Log.MethodBoundry("<"); 
            // simply echo back in the response whatever data came in the request
            var postDataRspDTO = new PostDataRspDTO(request.StringDataObject);
            DemoETWProvider.Log.MethodBoundry(">");
            return postDataRspDTO;
        }
        #endregion
    }
}
