// Required for the injected HttpClient
using System.Net.Http;
using System.Threading.Tasks;
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
// Required for the logger/logging
using Microsoft.Extensions.Logging;
using Blazor.Extensions.Logging;
// Access the DTOs defined in a separate assembly, shared with the Console App
using CommonDTOs;
// To use the SS. HttpClient-based JsonHttpClient
using ServiceStack;
using System;

namespace GUI.Pages {

    public static class BlazorSSClient { }
    public class BasicRESTServicesCodeBehind : BlazorComponent {

        #region string constants
        // Eventually replace with localization
        public const string labelForDataToPost = "Data To Post";
        public const string labelForDataReceivedFromPost = "Data Received from last Post";
        public const string labelForPostDataDataButton = "Press to Post Data";
        public const string dumpPostDataRspDTOText = "Pretry Print the postDataRspDTO object via the ServiceStack.Text extension method Dump: ";
        public const string labelForComplexDataDemo2StringData = "ComplexData.StringData To Post";
        public const string labelForComplexDataDemo2DateTimeData = "ComplexData.DateTimeData To Post";
        public const string labelForComplexDataDemo2IntData = "ComplexData.IntData To Post";
        public const string labelForComplexDataDemo2DoubleData = "ComplexData.DoubleData To Post";
        public const string labelForComplexDataDemo2DecimalData = "ComplexData.DecimalData To Post";
        public const string dumpPostComplexDataReqDTOText = "Pretty Print the ComplexDataReqDTO object via the ServiceStack.Text extension method Dump:";
        public const string labelForComplexDataReceivedFromPost = "ComplexData received from last Post";
        public const string labelForComplexDataReceivedFromPostStringData = "String Data:";
        public const string labelForComplexDataReceivedFromPostDateTimeData = "DateTime (UTC) Data:";
        public const string rspComplexDataDemo2DumpText = "Pretty Print the ComplexDataDemo02 object via the ServiceStack.Text extension method Dump:";
        public const string rspComplexDataDictionaryDemo2DumpText = "Pretty Print the ComplexDataDictionaryDemo02 object via the ServiceStack.Text extension method Dump:";
        public const string labelForPostComplexDataAsStringButton = "Press to Post ComplexData As a String";
        public const string labelForPostComplexDataDictionaryAsStringButton = "Press to Post ComplexDataDictionary As a String";
        public const string labelForPostComplexDataDataAsObjectButton = "Press to Post ComplexData As an object";
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
        public ILogger<BasicRESTServicesCodeBehind> Logger {
            get;
            set;
        }

        #endregion

        #region Page Initialization Handler
        protected override async Task OnInitAsync() {
            Logger.LogDebug($"Starting OnInitAsync");
            //Logger.LogDebug($"Initializing IServiceClient");
            // Someday this will work 
            //IServiceClient client = new JsonHttpClient("http://localhost:21100");
            //Logger.LogDebug($"client is null: {client == null}");
            InitializationReqDTO initializationReqDTO = new InitializationReqDTO();

            Logger.LogDebug($"Calling PostJsonAsync<BaseServicesInitializationRspPayload>");
            IServiceClient client = new JsonHttpClient("http://localhost:21200");
            InitializationRspDTO InitializationRspDTO =
                await client.PostAsync<InitializationRspDTO>("/Initialization?format=json", initializationReqDTO);

            Logger.LogDebug($"in OnInitAsync, Returned from PostAsync<InitializationRspDTO>, InitializationRspDTO: {InitializationRspDTO}");
            reqComplexData = new ComplexData()
                {
                    // initialize the fields
                    StringData = "Set By OnInitAsync",
                    DateTimeData = DateTime.UtcNow,
                    TimeSpanData = default,
                    IntData = default,
                    DoubleData = default,
                    DecimalData = default

                };
//                Logger.LogDebug($"In OnInitAsync, complexDataToPost: {reqComplexData.Dump()}");
                Logger.LogDebug($"Leaving OnInitAsync");
            }
            //  Create a Property for the Response DTO
            public InitializationRspDTO InitializationRspDTO { get; set; }
        #endregion

        #region Post Data Button OnClick Handler
        public async Task PostData() {
            Logger.LogDebug($"Starting PostData");
            // Create the payload for the Post
            PostDataReqDTO postDataReqDTO = new PostDataReqDTO() { StringDataObject = dataToPost };
            Logger.LogDebug($"In OnInitAsync, postDataReqDTO: {postDataReqDTO}");
            Logger.LogDebug($"in PostData, Calling PostAsync<PostDataRspDTO> with PostDataReqDTO: {postDataReqDTO}");
            IServiceClient client = new JsonHttpClient("http://localhost:21200");
            PostDataRspDTO postDataRspDTO =
              await client.PostAsync<PostDataRspDTO>("/PostData?format=json", postDataReqDTO);
            Logger.LogDebug($"Returned from PostAsync<PostDataRspDTO> with PostDataRspDTO: {postDataRspDTO}");
            dataReceivedFromPost = postDataRspDTO.StringDataObject;
            // dumpDataViaSS = postDataRspDTO.Dump();
            Logger.LogDebug($"Leaving PostData");
        }
        #endregion

         #region PostComplexData OnClick Handler
            public async Task PostComplexDataAsObject()
            {
                Logger.LogDebug($"Starting PostComplexData");
                // Create the payload for the Post. Validation tests on the data entered by the user are not being done, using default types if an input is null
                // Log what is in the page's demo3ComplexDataToPost object
//                Logger.LogDebug($"in PostComplexData: reqComplexData: {reqComplexData.Dump()}");
            // Create an instance of the request DTO class and populate its data property
            ReqComplexDataDemo3DTO reqComplexDataDemo3DTO = new ReqComplexDataDemo3DTO() { ComplexDataDemo3 = reqComplexData };
                // Log the DTO object
  //              Logger.LogDebug($"in PostComplexData: reqComplexDataDemo3DTO: {reqComplexDataDemo3DTO.Dump()}");
            // pass that object to the PostJsonAsync<string> and await its return 
            // There are no try/catch blocks for error handling in Demo03
            IServiceClient client = new JsonHttpClient("http://localhost:21200");
            RspComplexDataDemo3DTO rspComplexDataDemo3DTO =
              await client.PostAsync<RspComplexDataDemo3DTO>("/PostComplexData?format=json", reqComplexDataDemo3DTO);
                // start by dumping it out for inspection
//                Logger.LogDebug($"in PostComplexData: rspComplexDataDemo3AsStringDTO: {rspComplexDataDemo3DTO.Dump()}");
            // Extract just the ComplexData object
            rspComplexData = rspComplexDataDemo3DTO.ComplexDataDemo3;
            // Dump the complex object to a human-readable string (JSON) and set the page's field to display this information on the page
 //           rspComplexDataDump = rspComplexData.Dump();
                Logger.LogDebug($"in PostComplexData: rspComplexDataDump: {rspComplexDataDump}");
                Logger.LogDebug($"Leaving PostComplexData");
            }
            #endregion

        #region PostComplexDataDictionary OnClick Handler
            //public async Task PostComplexDataDictionary()
            //{
            //    Logger.LogDebug($"Starting PostComplexDataDictionary");
            //    // Create the payload for the Post. Validation tests on the data entered by the user are not being done, using default types if an input is null
            //    // Log what is in the page's demo2ComplexDataToPost object
            //    Logger.LogDebug($"in PostComplexDataDictionary: reqComplexData: {reqComplexData.Dump()}");
            //    // Create a ComplexDataDictionary, initialize it with a new dictionary
            //    ComplexDataDictionary reqComplexDataDictionaryDemo2 = new ComplexDataDictionary() { ComplexDataDict = new System.Collections.Generic.Dictionary<string, ComplexData>() };
            //    // Log the instance
            //    Logger.LogDebug($"in PostComplexDataDictionary: reqComplexDataDictionaryDemo2: {reqComplexDataDictionaryDemo2.Dump()}");
            //    // Add a Key:Value pair to the empty dictionary, the value is the page's reqComplexData object
            //    reqComplexDataDictionaryDemo2.ComplexDataDict["firstkey"] = reqComplexData;
            //    // Log the instance now that it is fully populated
            //    Logger.LogDebug($"in PostComplexDPostComplexDataDictionaryataDictionary: reqComplexDataAsDictionaryDemo2: {reqComplexDataDictionaryDemo2.Dump()}");
            //    // The HttpClient instance used below came from the DI  (IoC) container. Serialization is at the mercy of whatever Json-serializer the PostJsonAsync is using.
            //    // To ensure the ServiceStack JSON serializer and deserializer is used, we use SS serializers to serialize to a string, then pass that string to the HttpClient's PostJsonAsync method 
            //    // Serialize the ComplexData instance object using ServiceStack 
            //    // Convert the ComplexDataDictionary to a string
            //    var reqComplexDataDictionaryDemo2AsString = ServiceStack.Text.JsonSerializer.SerializeToString<ComplexDataDictionary>(reqComplexDataDictionaryDemo2);
            //    Logger.LogDebug($"in PostComplexDataDictionary: reqComplexDataDictionaryDemo2AsString: {reqComplexDataDictionaryDemo2AsString}");
            //    // Create an an instance of the request DTO class and initialize it with the ComplexDataDictionary object we just created
            //    ReqComplexDataDictionaryAsStringDemo2DTO reqComplexDataDictionaryAsStringDemo2DTO = new ReqComplexDataDictionaryAsStringDemo2DTO() { ComplexDataDictionaryAsStringDemo2 = reqComplexDataDictionaryDemo2AsString };
            //    // Log the ReqComplexDataDictionaryAsStringDemo2DTO
            //    Logger.LogDebug($"in PostComplexDataDictionary: reqComplexDataDictionaryAsStringDemo2DTO: {reqComplexDataDictionaryAsStringDemo2DTO.Dump()}");
            //    // pass that string to the PostAsync and await its return 
            //    // There are no try/catch blocks for error handling in Demo02
            //    var rspComplexDataDictionaryAsStringDemo2DTO = await HttpClient.PostJsonAsync<RspComplexDataDictionaryAsStringDemo2DTO> ("/PostComplexDataDictionary?format=json", reqComplexDataDictionaryAsStringDemo2DTO);
            //    // Don't make any assumptions about what the response was, start by dumping it out for inspection
            //    Logger.LogDebug($"in PostComplexDataDictionary: rspComplexDataAsStringDemo2DTO: {rspComplexDataDictionaryAsStringDemo2DTO.Dump()}");
            //    string rspComplexDataAsDictionaryDemo2AsString = rspComplexDataDictionaryAsStringDemo2DTO.ComplexDataDictionaryAsStringDemo2;
            //    Logger.LogDebug($"in PostComplexDataDictionary: rspComplexDataAsStringDemo2DTO.ComplexDataAsStringDemo2: {rspComplexDataAsDictionaryDemo2AsString}");
            //    // Deserialize the response from a string to a PostComplexDataRspDTO using the ServiceStack Json-deserializer
            //    ComplexDataDictionary rspComplexDataDictionaryDemo2 = ServiceStack.Text.JsonSerializer.DeserializeFromString<ComplexDataDictionary>(rspComplexDataAsDictionaryDemo2AsString);
            //    // Dump the complex object to a human-readable string
            //    Logger.LogDebug($"in PostComplexDataDictionary: RspPostComplexData: {rspComplexDataDictionaryDemo2.Dump()}");
            //    // pretty print it to a string field, and also to the logger
            //    rspComplexDataDictionaryDemo2Dump = rspComplexDataDictionaryDemo2.Dump();
            //    Logger.LogDebug($"in PostComplexDataDictionary dumpComplexDataViaSS: {rspComplexDataDictionaryDemo2Dump}");
            //    Logger.LogDebug($"Leaving PostComplexDataDictionary");
            //}
            #endregion

        #region public fields

        #region Demo01 code
        public string dataToPost;
        public string dataReceivedFromPost;
        public string dumpDataViaSS;
        #endregion

        #region Demo02 and Demo03 code
        public ComplexData reqComplexData;
        public ComplexData rspComplexData;
        public string rspComplexDataDump;
        public string rspComplexDataDictionaryDump;
        #endregion

        #region Demo02 code

        public string dumpReqDemo2PostComplexData;
        public string rspComplexDataDemo2Dump;
        public string rspComplexDataDictionaryDemo2Dump;
        #endregion

        #region Demo03 code
        public string dumpReqDemo3PostComplexData;
        #endregion
        #endregion
    }
}
