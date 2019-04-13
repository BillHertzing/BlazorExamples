// Required for the injected HttpClient
using System.Net.Http;
using System.Threading.Tasks;
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components;
// Required for the logger/logging
using Microsoft.Extensions.Logging;
//using Blazor.Extensions.Logging;
// Access the DTOs defined in a separate assembly, shared with the Console App
using CommonDTOs;
// Use the Serializers and .Dump() extension from ServiceStack.text
using ServiceStack.Text;

namespace GUI.Pages {
    public class BasicRESTServicesCodeBehind : ComponentBase
    {

        #region string constants
        // Eventually replace with localization
        public const string labelForDataToPost = "Data To Post";
        public const string labelForDataReceivedFromPost = "Data Received from last Post";
        public const string labelForPostDataDataButton = "Press to Post Data";
        public const string rspPostDataDumpText = "Pretry Print the postDataRspDTO object via the ServiceStack.Text extension method Dump: ";
        #region Demo02
        public const string labelForComplexDataStringData = "ComplexData.StringData To Post";
        public const string labelForComplexDataDateTimeData = "ComplexData.DateTimeData To Post";
        public const string labelForComplexDataIntData = "ComplexData.IntData To Post";
        public const string labelForComplexDataDoubleData = "ComplexData.DoubleData To Post";
        public const string labelForComplexDataDecimalData = "ComplexData.DecimalData To Post";
        public const string reqComplexDataDumpText = "Pretty Print the reqComplexData object via the ServiceStack.Text extension method Dump:";
        public const string reqComplexDataAsStringDTODumpText = "Pretty Print the reqComplexDataAsStringDTO object via the ServiceStack.Text extension method Dump:";
        public const string labelForComplexDataReceivedFromPost = "ComplexData received from last Post";
        public const string labelForComplexDataReceivedFromPostStringData = "String Data:";
        public const string labelForComplexDataReceivedFromPostDateTimeData = "DateTime (UTC) Data:";
        public const string rspComplexDataAsStringDumpText = "Pretty Print the rspComplexData object via the ServiceStack.Text extension method Dump:";
        public const string rspComplexDataDictionaryAsStringDumpText = "Pretty Print the rspComplexDataDictionary object via the ServiceStack.Text extension method Dump:";
        public const string labelForPostComplexDataAsStringButton = "Press to Post ComplexData As a String";
        public const string labelForPostComplexDataDictionaryAsStringButton = "Press to Post ComplexDataDictionary As a String";
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
        //[Inject]
        //public ILogger<BasicRESTServicesCodeBehind> Logger { get; set; }


        #endregion

        #region Page Initialization Handler
        protected override async Task OnInitAsync() {
                //Logger.LogDebug($"Starting OnInitAsync");
                InitializationReqDTO initializationReqDTO = new InitializationReqDTO();
                //Logger.LogDebug($"Calling PostJsonAsync<BaseServicesInitializationRspPayload>");
                InitializationRspDTO = await HttpClient.PostJsonAsync<InitializationRspDTO>("Initialization",
                                                                                                                 initializationReqDTO);
                //Logger.LogDebug($"Returned from PostJsonAsync<InitializationRspDTO>, InitializationRspDTO: {InitializationRspDTO}");
                // create the instance of the ComplexData to be posted
            reqComplexData = new ComplexData()
                {
                    // initialize the fields
                    StringData = "Set By OnInitAsync",
//                    DateTimeData = DateTime.UtcNow,
                    TimeSpanData = default,
                    IntData = default,
                    DoubleData = default,
                    DecimalData = default

                };
                //Logger.LogDebug($"Leaving OnInitAsync");
            }
            //  Create a Property for the Response DTO
            public InitializationRspDTO InitializationRspDTO { get; set; }
        #endregion

        #region Post Data Button OnClick Handler
        public async Task PostData() {
            //Logger.LogDebug($"Starting PostData");
            // Create the payload for the Post
            PostDataReqDTO postDataReqDTO = new PostDataReqDTO() { StringDataObject = dataToPost };
            //Logger.LogDebug($"In OnInitAsync, postDataReqDTO: {postDataReqDTO}");
            //Logger.LogDebug($"Calling PostJsonAsync<PostDataReqDTO> with PostDataReqDTO.StringDataObject: {dataToPost}");
            PostDataRspDTO postDataRspDTO =
              await HttpClient.PostJsonAsync<PostDataRspDTO>("/PostData?format=json", postDataReqDTO);
            //Logger.LogDebug($"Returned from PostJsonAsync<PostDataRspDTO> with PostDataRspDTO.StringDataObject: {postDataRspDTO.StringDataObject}");
            dataReceivedFromPost = postDataRspDTO.StringDataObject;
            rspPostDataDump = postDataRspDTO.Dump();
            //Logger.LogDebug($"Leaving PostData");
        }
        #endregion
        #region Demo01
        #region PostComplexDataAsString OnClick Handler
        public async Task PostComplexDataAsString()
        {
            //Logger.LogDebug($"Starting PostComplexDataAsString");
            // Create the payload for the Post. Validation tests on the data entered by the user are not being done, using default types if an input is null
            // Log what is in the page's ComplexDataToPost object
            //Logger.LogDebug($"in PostComplexDataAsString: reqComplexData: {reqComplexData.Dump()}");
            // The HttpClient instance used below came from the DI  (IoC) container. Serialization is at the mercy of whatever Json-serializer the PostJsonAsync is using.
            // To ensure the ServiceStack JSON serializer and deserializer is used, we use SS serializers to serialize to a string, then pass that string to the HttpClient's PostJsonAsync method 
            // Serialize the ComplexData instance object using ServiceStack 
            string reqComplexDataAsString = ServiceStack.Text.JsonSerializer.SerializeToString<ComplexData>(reqComplexData);
            //Logger.LogDebug($"in PostComplexDataAsString: reqComplexDataAsString: {reqComplexDataAsString}");
            // Display the ComplexData object-as-string in the GUI
            reqComplexDataDump = reqComplexData.Dump();
            // Create an instance of the request DTO class and populate its data property
            ReqComplexDataAsStringDTO reqComplexDataAsStringDTO = new ReqComplexDataAsStringDTO() { ComplexDataAsString = reqComplexDataAsString };
            // Log the DTO object
            //Logger.LogDebug($"in PostComplexDataAsString: reqComplexDataAsStringDTO: {reqComplexDataAsStringDTO.Dump()}");
            // Display the DTO object in the GUI
            reqComplexDataAsStringDTODump = reqComplexDataAsStringDTO.Dump();
            // pass that object to the PostJsonAsync<string> and await its return 
            // There are no try/catch blocks for error handling in Demo02
            var rspComplexDataAsStringDTO = await HttpClient.PostJsonAsync<RspComplexDataAsStringDTO>("/PostComplexDataAsString?format=json", reqComplexDataAsStringDTO);
            // Don't make any assumptions about what the response was, start by dumping it out for inspection
            //Logger.LogDebug($"in PostComplexDataAsString: rspComplexDataAsStringDTO: {rspComplexDataAsStringDTO.Dump()}");
            // Print what we expect to be the payload
            string rspComplexDataAsString = rspComplexDataAsStringDTO.ComplexDataAsString;
            //Logger.LogDebug($"in PostComplexDataAsString: rspComplexDataAsString: {rspComplexDataAsString}");
            // Deserialize the response from a string to a PostComplexData using the ServiceStack Json-deserializer
            ComplexData rspComplexData = ServiceStack.Text.JsonSerializer.DeserializeFromString<ComplexData>(rspComplexDataAsString);
            // Dump the complex object to a human-readable string
            //Logger.LogDebug($"in PostComplexDataAsString: rspComplexData: {rspComplexData.Dump()}");
            // Set the page's field to display this information on the page
            rspComplexDataAsStringDump = rspComplexData.Dump();
            //Logger.LogDebug($"in PostComplexDataAsString rspComplexDataAsStringDump: {rspComplexDataAsStringDump}");
            //Logger.LogDebug($"Leaving PostComplexDataAsString");
        }
        #endregion

        #region PostComplexDataDictionaryAsString OnClick Handler
        public async Task PostComplexDataDictionaryAsString()
        {
            //Logger.LogDebug($"Starting PostComplexDataDictionaryAsString");
            // Create the payload for the Post. Validation tests on the data entered by the user are not being done, using default types if an input is null
            // Log what is in the page's demo2ComplexDataToPost object
            //Logger.LogDebug($"in PostComplexDataDictionaryAsString: reqComplexData: {reqComplexData.Dump()}");
            // Create a ComplexDataDictionary, initialize it with a new dictionary
            ComplexDataDictionary reqComplexDataDictionary = new ComplexDataDictionary() { ComplexDataDict = new System.Collections.Generic.Dictionary<string, ComplexData>() };
            // Log the instance
            //Logger.LogDebug($"in PostComplexDataDictionaryAsString: reqComplexDataDictionary: {reqComplexDataDictionary.Dump()}");
            // Add a Key:Value pair to the empty dictionary, the value is the page's reqComplexData object
            reqComplexDataDictionary.ComplexDataDict["firstkey"] = reqComplexData;
            // Log the instance now that it is fully populated
            //Logger.LogDebug($"in PostComplexDPostComplexDataDictionaryAsStringataDictionary: reqComplexDataAsDictionary: {reqComplexDataDictionary.Dump()}");
            // The HttpClient instance used below came from the DI  (IoC) container. Serialization is at the mercy of whatever Json-serializer the PostJsonAsync is using.
            // To ensure the ServiceStack JSON serializer and deserializer is used, we use SS serializers to serialize to a string, then pass that string to the HttpClient's PostJsonAsync method 
            // Serialize the ComplexData instance object using ServiceStack 
            // Convert the ComplexDataDictionary to a string
            var reqComplexDataDictionaryAsString = ServiceStack.Text.JsonSerializer.SerializeToString<ComplexDataDictionary>(reqComplexDataDictionary);
            //Logger.LogDebug($"in PostComplexDataDictionaryAsString: reqComplexDataDictionaryAsString: {reqComplexDataDictionaryAsString}");
            // Create an an instance of the request DTO class and initialize it with the ComplexDataDictionary object we just created
            ReqComplexDataDictionaryAsStringDTO reqComplexDataDictionaryAsStringDTO = new ReqComplexDataDictionaryAsStringDTO() { ComplexDataDictionaryAsString = reqComplexDataDictionaryAsString };
            // Log the ReqComplexDataDictionaryAsStringDTO
            //Logger.LogDebug($"in PostComplexDataDictionaryAsString: reqComplexDataDictionaryAsStringDTO: {reqComplexDataDictionaryAsStringDTO.Dump()}");
            // pass that string to the PostAsync and await its return 
            // There are no try/catch blocks for error handling in Demo02
            var rspComplexDataDictionaryAsStringDTO = await HttpClient.PostJsonAsync<RspComplexDataDictionaryAsStringDTO> ("/PostComplexDataDictionaryAsString?format=json", reqComplexDataDictionaryAsStringDTO);
            // Don't make any assumptions about what the response was, start by dumping it out for inspection
            //Logger.LogDebug($"in PostComplexDataDictionaryAsString: rspComplexDataAsStringDTO: {rspComplexDataDictionaryAsStringDTO.Dump()}");
            string rspComplexDataAsDictionaryAsString = rspComplexDataDictionaryAsStringDTO.ComplexDataDictionaryAsString;
            //Logger.LogDebug($"in PostComplexDataDictionaryAsString: rspComplexDataAsStringDTO.ComplexDataAsString: {rspComplexDataAsDictionaryAsString}");
            // Deserialize the response from a string to a PostComplexDataRspDTO using the ServiceStack Json-deserializer
            ComplexDataDictionary rspComplexDataDictionary = ServiceStack.Text.JsonSerializer.DeserializeFromString<ComplexDataDictionary>(rspComplexDataAsDictionaryAsString);
            // Dump the complex object to a human-readable string
            //Logger.LogDebug($"in PostComplexDataDictionaryAsString: RspPostComplexData: {rspComplexDataDictionary.Dump()}");
            // pretty print it to a string field, and also to the logger
            rspComplexDataDictionaryAsStringDump = rspComplexDataDictionary.Dump();
            //Logger.LogDebug($"in PostComplexDataDictionaryAsString dumpComplexDataViaSS: {rspComplexDataDictionaryAsStringDump}");
            //Logger.LogDebug($"Leaving PostComplexDataDictionaryAsString");
        }
        #endregion
        #endregion
        #region public fields

        #region Demo01 code
        public string dataToPost;
        public string dataReceivedFromPost;
        public string rspPostDataDump;
        #endregion

        #region Demo02 code
        public ComplexData reqComplexData;
        public string reqComplexDataDump;
        public string reqComplexDataAsStringDTODump;
        public ComplexData rspComplexData;
        public string rspComplexDataAsStringDump;
        public string rspComplexDataDictionaryAsStringDump;
        #endregion

        #endregion
    }
}
