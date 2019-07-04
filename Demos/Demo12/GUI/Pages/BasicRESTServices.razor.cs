// Required for the injected HttpClient
using System.Net.Http;
using System.Threading.Tasks;
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components;
// Required for the logger/logging
using Microsoft.Extensions.Logging;

// Access the DTOs defined in a separate assembly, shared with the Console App
using CommonDTOs;
using System;

namespace GUI.Pages
{
    public partial class BasicRESTServicesCodeBehind : ComponentBase
    {

        #region string constants
        #region stringconstants:Demo1
        // Eventually replace with localization
        public const string textForDemo1Area = "The Demo01 functions are repeated here to ensure they continue to work";
        public const string labelForDataToPost = "Data To Post";
        public const string labelForDataReceivedFromPost = "Data Received from last Post";
        public const string labelForPostDataDataButton = "Press to Post Data";
        #endregion
        #endregion

        #region DI container Auto-wired properties
        // This syntax adds to the class a Property that accesses the DI container, and retrieves the instance having the specified type from the DI container.
        // Access the builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type registered in the DI container
        [Inject]
        HttpClient HttpClient {
            get;
            set;
        }

        // Access the Logging extensions registered in the DI container
        [Inject]
        public ILogger<BasicRESTServicesCodeBehind> Logger { get; set; }

        #endregion

        #region Page Initialization Handler
        protected override async Task OnInitAsync()
        {
            Logger.LogTrace($"Starting OnInitAsync");
            ////Logger.LogDebug($"Initializing IServiceClient");
            // Someday this will work 
            //IServiceClient client = new JsonHttpClient("http://localhost:21100");
            ////Logger.LogDebug($"client is null: {client == null}");
            InitializationReqDTO initializationReqDTO = new InitializationReqDTO();
             
            // Issue in Preview 4 requires the "route" to be a complete URL
            var uriBuilder = new UriBuilder("http://localhost:21200/Initialization");
            Logger.LogTrace($"Calling PostJsonAsync<BaseServicesInitializationRspPayload>");
            InitializationRspDTO = await HttpClient.PostJsonAsync<InitializationRspDTO>(uriBuilder.Uri.ToString(),
                                                                                                             initializationReqDTO);
            Logger.LogTrace($"Returned from PostJsonAsync<InitializationRspDTO>, InitializationRspDTO = {InitializationRspDTO}");
            Logger.LogTrace($"Leaving OnInitAsync");
        }
        //  Create a Property for the Response DTO
        public InitializationRspDTO InitializationRspDTO { get; set; }
        #endregion

        #region Post Data Button OnClick Handler
        public async Task PostData() {
            Logger.LogTrace($"Starting PostData");
            // Create the payload for the Post
            PostDataReqDTO postDataReqDTO = new PostDataReqDTO() { StringDataObject = DataToPost };
           Logger.LogTrace($"Calling PostJsonAsync<PostDataReqDTO> with PostDataReqDTO.StringDataObject = {postDataReqDTO.StringDataObject}");
            PostDataRspDTO postDataRspDTO =
                await HttpClient.PostJsonAsync<PostDataRspDTO>(new UriBuilder("http://localhost:21200/PostData").Uri.ToString(), postDataReqDTO);
            Logger.LogTrace($"Returned from PostJsonAsync<PostDataRspDTO> with PostDataRspDTO.StringDataObject = {postDataRspDTO.StringDataObject}");
            DataReceivedFromPost = postDataRspDTO.StringDataObject;
            Logger.LogTrace($"Leaving PostData");
        }
        #endregion

        #region Properties
        public string DataToPost { get; set; }
        public string DataReceivedFromPost { get; set; }
        #endregion
    }
}
