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
// Use the Serializers and .Dump() extension from ServiceStack.text
using ServiceStack.Text;
using System;

namespace GUI.Pages {
    public class BasicRESTServicesCodeBehind : ComponentBase {

        #region string constants
        #region stringconstants:Demo1
        // Eventually replace with localization
        public const string textForDemo1Area = "The Demo01 functions are repeated here to ensure they continue to work";
        public const string labelForDataToPost = "Data To Post";
        public const string labelForDataReceivedFromPost = "Data Received from last Post";
        public const string labelForPostDataDataButton = "Press to Post Data";
        public const string rspPostDataDumpText = "Pretty Print the postDataRspDTO object via the ServiceStack.Text extension method .Dump(): ";
        #endregion
        #region Demo02
        public const string textForDemo2Area = "The Demo02 functions show how the ServiceStack .Dump extension can be used to serialize a complex object instance to a string";
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
        [Inject]
        public ILogger<BasicRESTServicesCodeBehind> Logger { get; set; }

        #endregion

        #region Page Initialization Handler
        protected override async Task OnInitAsync() {
            Logger.LogTrace($"Starting OnInitAsync");
            InitializationReqDTO initializationReqDTO = new InitializationReqDTO();
            var uriBuilder = new UriBuilder("http://localhost:21200/Initialization");
            Logger.LogTrace($"Calling PostJsonAsync<BaseServicesInitializationRspPayload>");
            InitializationRspDTO = await HttpClient.PostJsonAsync<InitializationRspDTO>(uriBuilder.Uri.ToString(),
                                                                                                                 initializationReqDTO);
            Logger.LogTrace($"Returned from PostJsonAsync<InitializationRspDTO>, InitializationRspDTO: {InitializationRspDTO}");
            // create the instance of the ComplexData to be posted
            reqComplexData=new ComplexData() {
                // initialize the fields
                StringData="Set By OnInitAsync",
                //                    DateTimeData = DateTime.UtcNow,
                TimeSpanData=default,
                IntData=default,
                DoubleData=default,
                DecimalData=default

            };
            Logger.LogTrace($"Leaving OnInitAsync");
        }
        //  Create a Property for the Response DTO
        public InitializationRspDTO InitializationRspDTO { get; set; }
        #endregion

        #region Demo01
        #region Post Data Button OnClick Handler
        public async Task PostData() {
            Logger.LogTrace($"Starting PostData");
            // Create the payload for the Post
            PostDataReqDTO postDataReqDTO = new PostDataReqDTO() { StringDataObject = DataToPost };
            Logger.LogTrace($"Calling PostJsonAsync<PostDataReqDTO> with PostDataReqDTO.StringDataObject = {DataToPost}");
            PostDataRspDTO postDataRspDTO =
              await HttpClient.PostJsonAsync<PostDataRspDTO>(new UriBuilder("http://localhost:21200/PostData").Uri.ToString(), postDataReqDTO);
            Logger.LogTrace($"Returned from PostJsonAsync<PostDataRspDTO> with PostDataRspDTO.StringDataObject: {postDataRspDTO.StringDataObject}");
            DataReceivedFromPost = postDataRspDTO.StringDataObject;
            rspPostDataDump = postDataRspDTO.Dump();
            Logger.LogTrace($"Leaving PostData");
        }
        #endregion
        #endregion

        #region Demo02
        #region PostComplexDataAsString OnClick Handler
        public async Task PostComplexDataAsString() {
            Logger.LogTrace($"Starting PostComplexDataAsString");
            // Create the payload for the Post. 
			//  Validation tests on the data entered by the user are not being done, using default types if an input is null
            // Log what is in the page's ComplexDataToPost object
            Logger.LogDebug($"in PostComplexDataAsString: reqComplexData.Dump: {reqComplexData.Dump()}");
			
            // The HttpClient instance used below came from the DI  (IoC) container. Serialization is at the mercy of whatever Json-serializer the PostJsonAsync is using.
            // To ensure the ServiceStack JSON serializer and deserializer is used, 
			//    use SS serializers to serialize to a string, 
			//    then pass that string to the HttpClient's PostJsonAsync method 
            // Serialize the ComplexData instance object using ServiceStack 
            string reqComplexDataAsString = ServiceStack.Text.JsonSerializer.SerializeToString<ComplexData>(reqComplexData);
            Logger.LogDebug($"in PostComplexDataAsString: reqComplexDataAsString: {reqComplexDataAsString}");
            // Display the ComplexData object-as-string in the GUI
            reqComplexDataDump=reqComplexData.Dump();
            // Create an instance of the request DTO class and populate its data property
            ComplexDataAsStringReqDTO reqComplexDataAsStringDTO = new ComplexDataAsStringReqDTO() { ComplexDataAsString=reqComplexDataAsString };
            // Log the DTO object
            Logger.LogDebug($"in PostComplexDataAsString: reqComplexDataAsStringDTO.Dump(): {reqComplexDataAsStringDTO.Dump()}");
            // Display the DTO object in the GUI
            reqComplexDataAsStringDTODump=reqComplexDataAsStringDTO.Dump();
            // pass that object to the PostJsonAsync<string> and await its return 
            // There are no try/catch blocks for error handling in Demo02
            var rspComplexDataAsStringDTO = await HttpClient.PostJsonAsync<ComplexDataAsStringRspDTO>(new UriBuilder("http://localhost:21200/PostComplexDataAsString").Uri.ToString(), reqComplexDataAsStringDTO);
            // Don't make any assumptions about what the response was, start by dumping it out for inspection
            Logger.LogDebug($"in PostComplexDataAsString: rspComplexDataAsStringDTO.Dump(): {rspComplexDataAsStringDTO.Dump()}");
            // Print what we expect to be the payload
            string rspComplexDataAsString = rspComplexDataAsStringDTO.ComplexDataAsString;
            Logger.LogDebug($"in PostComplexDataAsString: rspComplexDataAsString: {rspComplexDataAsString}");
            // Deserialize the response from a string to a PostComplexData using the ServiceStack Json-deserializer
            ComplexData rspComplexData = ServiceStack.Text.JsonSerializer.DeserializeFromString<ComplexData>(rspComplexDataAsString);
            // Dump the complex object to a human-readable string
            Logger.LogDebug($"in PostComplexDataAsString: rspComplexData.Dump(): {rspComplexData.Dump()}");
            // Set the page's field to display this information on the page
            rspComplexDataAsStringDump=rspComplexData.Dump();
            Logger.LogDebug($"in PostComplexDataAsString rspComplexDataAsStringDump: {rspComplexDataAsStringDump}");
            Logger.LogDebug($"Leaving PostComplexDataAsString");
        }
        #endregion

        #region PostComplexDataDictionaryAsString OnClick Handler
        public async Task PostComplexDataDictionaryAsString() {
            Logger.LogTrace($"Starting PostComplexDataDictionaryAsString");
            // Create the payload for the Post. Validation tests on the data entered by the user are not being done, using default types if an input is null
            // Log what is in the page's demo2ComplexDataToPost object
            Logger.LogDebug($"in PostComplexDataDictionaryAsString: reqComplexData.Dump(): {reqComplexData.Dump()}");
            // Create a ComplexDataDictionary, initialize it with a new dictionary
            ComplexDataDictionary reqComplexDataDictionary = new ComplexDataDictionary() { ComplexDataDict=new System.Collections.Generic.Dictionary<string, ComplexData>() };
            // Log the instance
            Logger.LogDebug($"in PostComplexDataDictionaryAsString: reqComplexDataDictionary.Dump(): {reqComplexDataDictionary.Dump()}");
            // Add a Key:Value pair to the empty dictionary, the value is the page's reqComplexData object
            reqComplexDataDictionary.ComplexDataDict["firstkey"]=reqComplexData;
            // Log the instance now that it is fully populated
            Logger.LogDebug($"in PostComplexDPostComplexDataDictionaryAsStringataDictionary: reqComplexDataAsDictionary.Dump(): {reqComplexDataDictionary.Dump()}");
            // The HttpClient instance used below came from the DI  (IoC) container. Serialization is at the mercy of whatever Json-serializer the PostJsonAsync is using.
            // To ensure the ServiceStack JSON serializer and deserializer is used, we use SS serializers to serialize to a string, then pass that string to the HttpClient's PostJsonAsync method 
            // Serialize the ComplexData instance object using ServiceStack 
            // Convert the ComplexDataDictionary to a string
            var reqComplexDataDictionaryAsString = ServiceStack.Text.JsonSerializer.SerializeToString<ComplexDataDictionary>(reqComplexDataDictionary);
            Logger.LogDebug($"in PostComplexDataDictionaryAsString: reqComplexDataDictionaryAsString: {reqComplexDataDictionaryAsString}");
            // Create an an instance of the request DTO class and initialize it with the ComplexDataDictionary object we just created
            ComplexDataDictionaryAsStringReqDTO reqComplexDataDictionaryAsStringDTO = new ComplexDataDictionaryAsStringReqDTO() { ComplexDataDictionaryAsString=reqComplexDataDictionaryAsString };
            // Log the ComplexDataDictionaryAsStringReqDTO
            Logger.LogDebug($"in PostComplexDataDictionaryAsString: reqComplexDataDictionaryAsStringDTO.Dump(): {reqComplexDataDictionaryAsStringDTO.Dump()}");
            // pass that string to the PostAsync and await its return 
            // There are no try/catch blocks for error handling in Demo02
            var rspComplexDataDictionaryAsStringDTO = await HttpClient.PostJsonAsync<ComplexDataDictionaryAsStringRspDTO>(new UriBuilder("http://localhost:21200/PostComplexDataDictionaryAsString").Uri.ToString(), reqComplexDataDictionaryAsStringDTO);
            // Don't make any assumptions about what the response was, start by dumping it out for inspection
            Logger.LogDebug($"in PostComplexDataDictionaryAsString: rspComplexDataAsStringDTO.Dump(): {rspComplexDataDictionaryAsStringDTO.Dump()}");
            string rspComplexDataAsDictionaryAsString = rspComplexDataDictionaryAsStringDTO.ComplexDataDictionaryAsString;
            Logger.LogDebug($"in PostComplexDataDictionaryAsString: rspComplexDataAsStringDTO.ComplexDataDictionaryAsString: {rspComplexDataAsDictionaryAsString}");
            // Deserialize the response from a string to a PostComplexDataRspDTO using the ServiceStack Json-deserializer
            ComplexDataDictionary rspComplexDataDictionary = ServiceStack.Text.JsonSerializer.DeserializeFromString<ComplexDataDictionary>(rspComplexDataAsDictionaryAsString);
            // Dump the complex object to a human-readable string
            Logger.LogDebug($"in PostComplexDataDictionaryAsString: rspComplexDataDictionary.Dump(): {rspComplexDataDictionary.Dump()}");
            // pretty print it to a string field, and also to the logger
            rspComplexDataDictionaryAsStringDump=rspComplexDataDictionary.Dump();
            Logger.LogDebug($"in PostComplexDataDictionaryAsString rspComplexDataDictionaryAsStringDump: {rspComplexDataDictionaryAsStringDump}");
            Logger.LogTrace($"Leaving PostComplexDataDictionaryAsString");
        }
        #endregion
        #endregion

        #region public fields

        #region Demo01 code
        public string DataToPost { get; set; }
        public string DataReceivedFromPost { get; set; }
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