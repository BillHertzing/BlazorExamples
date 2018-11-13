// Required for the injected HttpClient
using System.Net.Http;
using System.Threading.Tasks;
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
// Required for the logger/logging
using Microsoft.Extensions.Logging;
using Blazor.Extensions.Logging;

// Access the DTOs defined in a separate assembly, shared with teh Console App
using CommonDTOs;

// Someday be able to use the ServiceStack TextUtils and HTTPClient and/or c# client here
//using ServiceStack;

namespace GUI.Pages {
    public class BasicRESTServicesCodeBehind : BlazorComponent {
    #region Page Initialization Handler
        protected override async Task OnInitAsync() {
            Logger.LogDebug($"Starting OnInitAsync");
            //Logger.LogDebug($"Initializing IServiceClient");
            // Someday this will work 
            //IServiceClient client = new JsonHttpClient("http://localhost:21100");
            //Logger.LogDebug($"client is null: {client == null}");
            InitializationReqDTO initializationReqDTO = new InitializationReqDTO();

            Logger.LogDebug($"Calling PostJsonAsync<BaseServicesInitializationRspPayload>");
            InitializationRspDTO = await HttpClient.PostJsonAsync<InitializationRspDTO>("Initialization",
                                                                                                             initializationReqDTO);
            Logger.LogDebug($"Returned from PostJsonAsync<InitializationRspDTO>, InitializationRspDTO = {InitializationRspDTO}");
            Logger.LogDebug($"Leaving OnInitAsync");
        }
        //  Create a Property for the Response DTO
        public InitializationRspDTO InitializationRspDTO { get; set; }
    #endregion

    #region Post Data Button OnClick Handler
    public async Task PostData() {
        Logger.LogDebug($"Starting PostData");
        // Create the payload for the Post
        PostDataReqDTO postDataReqDTO = new PostDataReqDTO { StringDataObject = dataToPost };
        Logger.LogDebug($"Calling PostJsonAsync<PostDataReqDTO> with PostDataReqDTO.StringDataObject = {dataToPost}");
        PostDataRspDTO postDataRspDTO =
          await HttpClient.PostJsonAsync<PostDataRspDTO>("/PostData?format=json", postDataReqDTO);
        Logger.LogDebug($"Returned from PostJsonAsync<PostDataRspDTO> with PostDataRspDTO.StringDataObject = {postDataRspDTO.StringDataObject}");
        dataReceivedFromPost = postDataRspDTO.StringDataObject;
        Logger.LogDebug($"Leaving PostData");
    }
    #endregion

    #region string constants
      // Eventually replace with localization
        public const string labelForDataToPost = "Data To Post";
        public const string labelForDataReceivedFromPost = "Data Received from last Post";
        public const string labelForPostDataDataButton = "Press to Post Data";

    #endregion

    #region DI container Auto-wired properties
    // This syntax adds to the class a Property that accesses the DI container, and retrieves the instance having the specified type from the DI container.
    // Access the builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type registered in the DI container
    [Inject]
    HttpClient HttpClient
    {
        get;
        set;
    }

    // Access the Logging extensions registered in the DI container
    [Inject]
    public ILogger<BasicRESTServicesCodeBehind> Logger
    {
        get;
        set;
    }

    #endregion

    #region public fields
      public string dataToPost;
      public string dataReceivedFromPost;
    #endregion
    }
}
