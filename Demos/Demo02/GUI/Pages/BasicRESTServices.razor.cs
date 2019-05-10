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

        #region StringConstants
        #region StringConstants:Demo1
        // Eventually replace with localization
        public const string textForDemo1Area = "The Demo01 functions are repeated here to ensure they continue to work";
        public const string labelForDataToPost = "Data To Post";
        public const string labelForDataReceivedFromPost = "Data Received from last Post";
        public const string labelForPostDataDataButton = "Press to Post Data";
        public const string rspSimpleDataRspDTODumpText = "String representation of simple string object via the ServiceStack.Text extension method .Dump(): ";
        #endregion
        #region StringConstants:Demo1
        public const string textForDemo2Area = "The Demo02 functions show how the ServiceStack .Dump extension can be used to serialize a complex object instance to a string";
        public const string labelForComplexDataStringData = "ComplexData.StringData";
        public const string labelForComplexDataDateTimeData = "ComplexData.DateTimeData";
        public const string labelForComplexDataIntData = "ComplexData.IntData";
        public const string labelForComplexDataDoubleData = "ComplexData.DoubleData";
        public const string labelForComplexDataDecimalData = "ComplexData.DecimalData";
        public const string complexDataDumpText = "String representation of ComplexData object via the ServiceStack.Text extension method .Dump():";
        public const string reqComplexDataDumpedText = "reqComplexData.Dump():";
        public const string reqComplexDataAsStringDumpedText = "reqComplexDataAsString.Dump():";
        public const string reqComplexDataAsStringReqDTODumpedText = "reqComplexDataAsStringReqDTO.Dump():";
        public const string rspComplexDataAsStringRspDTODumpedText = "rspComplexDataAsStringRspDTO.Dump():";
        public const string rspComplexDataAsStringDumpedText = "rspComplexDataAsString.Dump():";
        public const string rspComplexDataDumpedText = "rspComplexData.Dump():";
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
            #region Demo1 Initialization Code
            InitializationReqDTO initializationReqDTO = new InitializationReqDTO();
            var uriBuilder = new UriBuilder("http://localhost:21200/Initialization");
            Logger.LogTrace($"Calling PostJsonAsync<BaseServicesInitializationRspPayload>");
            InitializationRspDTO = await HttpClient.PostJsonAsync<InitializationRspDTO>(uriBuilder.Uri.ToString(),
                                                                                                                 initializationReqDTO);
            Logger.LogTrace($"Returned from PostJsonAsync<InitializationRspDTO>, InitializationRspDTO: {InitializationRspDTO}");
            #endregion
            #region Demo2 Initialization Code
            // create the instance of the ComplexData to be posted
            ComplexDataP=new ComplexData() {
                // initialize the fields
                StringData="Set By OnInitAsync",
                // DateTimeData = DateTime.UtcNow,
                // TimeSpanData=default,
                IntData=default,
                DoubleData=default,
                DecimalData=default

            };
            // Create a Dumped string of the ComplexDataP object
            ComplexDataDumped=ComplexDataP.Dump();
            // Create a ComplexDataDictionary, initialize it with a new dictionary
            ComplexDataDictionaryP = new ComplexDataDictionary(new System.Collections.Generic.Dictionary<string, ComplexData>()) { };
            // Add a Key:Value pair to the empty dictionary, the value is the  ComplexDataP object
            ComplexDataDictionaryP.ComplexDataDict["firstkey"]=ComplexDataP;
            // Create a Dumped string of the ComplexDataDictionary object
            ComplexDataDictionaryDumped=ComplexDataDictionaryP.Dump();
            #endregion

            Logger.LogTrace($"Leaving OnInitAsync");
        }
        //  Create a Property for the Response DTO
        public InitializationRspDTO InitializationRspDTO { get; set; }
        #endregion

        #region Demo01
        #region Post Data Button OnClick Handler
        public async Task PostSimpleData() {
            Logger.LogTrace($"Starting PostSimpleData");
            // Create the payload for the Post
            PostDataReqDTO postDataReqDTO = new PostDataReqDTO() { StringDataObject = DataToPost };
            Logger.LogTrace($"Calling PostJsonAsync<PostDataReqDTO> with PostDataReqDTO.StringDataObject = {DataToPost}");
            PostDataRspDTO postDataRspDTO =
              await HttpClient.PostJsonAsync<PostDataRspDTO>(new UriBuilder("http://localhost:21200/PostSimpleData").Uri.ToString(), postDataReqDTO);
            Logger.LogTrace($"Returned from PostJsonAsync<PostDataRspDTO> with PostDataRspDTO.StringDataObject: {postDataRspDTO.StringDataObject}");
            RspSimpleDataRspDTODumped=postDataRspDTO.Dump();
            StringDataObjectReceivedFromPost= postDataRspDTO.StringDataObject;
            Logger.LogTrace($"Leaving PostSimpleData");
        }
        #endregion
        #endregion
        #region Demo02
        #region PostComplexDataAsString OnClick Handler
        public async Task PostComplexDataAsString() {
            Logger.LogTrace($"Starting PostComplexDataAsString");
            // Create the payload for the Post. 
            //  copy the ComplexDataP property into a local variable
            //  Validation tests on the data entered by the user are not being done, using default types if an input is null

            var reqComplexData = ComplexDataP;
            // Log the reqComplexData object
            Logger.LogDebug($"in PostComplexDataAsString: reqComplexData.Dump: {reqComplexData.Dump()}");
            // Display the ComplexData object in the GUI as a string using Dump()
            ReqComplexDataDumped=reqComplexData.Dump();

            // The HttpClient instance used below came from the DI  (IoC) container. Serialization is at the mercy of whatever Json-serializer the PostJsonAsync is using.
            // To ensure the ServiceStack JSON serializer and deserializer is used, 
            //    use SS serializers to serialize to a string, 
            //    then pass that string to the HttpClient's PostJsonAsync method 

            // Serialize the ComplexData instance object using ServiceStack 
            string reqComplexDataAsString = ServiceStack.Text.JsonSerializer.SerializeToString<ComplexData>(ComplexDataP);
            // Log the reqComplexDataAsString object
            Logger.LogDebug($"in PostComplexDataAsString: ComplexDataAsString: {reqComplexDataAsString.Dump()}");
            // Display the ComplexData object in the GUI as a string using Dump()
            ReqComplexDataAsStringDumped=reqComplexDataAsString.Dump();

            // Create an instance of the request DTO class and populate its data property
            ComplexDataAsStringReqDTO complexDataAsStringReqDTO = new ComplexDataAsStringReqDTO(reqComplexDataAsString);
            // Log the DTO object
            Logger.LogDebug($"in PostComplexDataAsString: complexDataAsStringReqDTO.Dump(): {complexDataAsStringReqDTO.Dump()}");
            // Display the DTO object in the GUI as a string using Dump()
            ReqComplexDataAsStringReqDTODumped= complexDataAsStringReqDTO.Dump();

            // pass that object to the PostJsonAsync<string> and await its return 
            // There are no try/catch blocks for error handling 
            Logger.LogDebug($"in PostComplexDataAsString: calling HttpClient.PostJsonAsync<ComplexDataAsStringRspDTO>");
            var complexDataAsStringRspDTO = await HttpClient.PostJsonAsync<ComplexDataAsStringRspDTO>(new UriBuilder("http://localhost:21200/PostComplexDataAsString").Uri.ToString(), complexDataAsStringReqDTO);

            // ToDo: better understaanding of the await in this specific useage. Is it 'assured" that the next line won't execute until a response is received?
            // Log the DTO object received
            Logger.LogDebug($"in PostComplexDataAsString: complexDataAsStringRspDTO.Dump(): {complexDataAsStringRspDTO.Dump()}");
            // Display the received DTO object in the GUI as a string using Dump()
            RspComplexDataAsStringRspDTODumped=complexDataAsStringRspDTO.Dump();

            // Extract the string object from the payload
            var rspComplexDataAsString = complexDataAsStringRspDTO.ComplexDataAsString;
            // Log that string
            Logger.LogDebug($"in PostComplexDataAsString: rspComplexDataAsString: {rspComplexDataAsString.Dump()}");
            // Display the string object in the GUI as a string using Dump()
            RspComplexDataAsStringDumped=rspComplexDataAsString.Dump();
            Logger.LogDebug($"1");
            // Deserialize the response from a string to a ComplexData using the ServiceStack Json-deserializer
            ComplexData rspComplexData = ServiceStack.Text.JsonSerializer.DeserializeFromString<ComplexData>(rspComplexDataAsString);
            Logger.LogDebug($"2");
            // Log the rspComplexData using Dump()
            Logger.LogDebug($"in PostComplexDataAsString: rspComplexData.Dump(): {rspComplexData.Dump()}");
            // Display the rspComplexData in the GUI as a string using Dump()
            RspComplexDataDumped= rspComplexData.Dump();

            // Copy the rspComplexData to the GUI ComplexDataP object, replacing the current with the new
            ComplexDataP=rspComplexData;
            // update the ComplexDataDumped in the GUI as a string using Dump()
            ComplexDataDumped=ComplexDataP.Dump();

            Logger.LogDebug($"Leaving PostComplexDataAsString");
        }
        #endregion

        #region PostComplexDataDictionaryAsString OnClick Handler
        public async Task PostComplexDataDictionaryAsString()
        {
            Logger.LogTrace($"Starting PostComplexDataDictionaryAsString");
            // Create the payload for the Post. 
            //  copy the ComplexDataDictionaryP property into a local variable
            var reqComplexDataDictionary = ComplexDataDictionaryP;
            // Log the reqComplexDataDictionary object
            Logger.LogDebug($"in PostComplexDataDictionaryAsString: reqComplexDataDictionary.Dump: {reqComplexDataDictionary.Dump()}");
            // Display the ComplexDataDictionary object in the GUI as a string using Dump()
            ReqComplexDataDictionaryDumped=reqComplexDataDictionary.Dump();

            // The HttpClient instance used below came from the DI  (IoC) container. Serialization is at the mercy of whatever Json-serializer the PostJsonAsync is using.
            // To ensure the ServiceStack JSON serializer and deserializer is used, 
            //    use SS serializers to serialize to a string, 
            //    then pass that string to the HttpClient's PostJsonAsync method 

            // Serialize the ComplexData instance object using ServiceStack 
            string reqComplexDataDictionaryAsString = ServiceStack.Text.JsonSerializer.SerializeToString<ComplexDataDictionary>(reqComplexDataDictionary);
            // Log the reqComplexDictionaryDataAsString object
            Logger.LogDebug($"in PostComplexDataDictionaryAsString: ComplexDataDictionaryAsString: {reqComplexDataDictionaryAsString.Dump()}");
            // Display the reqComplexDictionaryDataAsString  object in the GUI as a string using Dump()
            ReqComplexDataDictionaryAsStringDumped=reqComplexDataDictionaryAsString.Dump();

            // Create an instance of the request DTO class and populate its data property
            ComplexDataDictionaryAsStringReqDTO complexDataDictionaryAsStringReqDTO = new ComplexDataDictionaryAsStringReqDTO(reqComplexDataDictionaryAsString);
            // Log the DTO object
            Logger.LogDebug($"in PostComplexDataDictionaryAsString: complexDataDictionaryAsStringReqDTO.Dump(): {complexDataDictionaryAsStringReqDTO.Dump()}");
            // Display the DTO object in the GUI as a string using Dump()
            ReqComplexDataDictionaryAsStringReqDTODumped=complexDataDictionaryAsStringReqDTO.Dump();

            // pass that string to the PostAsync and await its return 
            // There are no try/catch blocks for error handling in Demo02
            var complexDataDictionaryAsStringRspDTO = await HttpClient.PostJsonAsync<ComplexDataDictionaryAsStringRspDTO> (new UriBuilder("http://localhost:21200/PostComplexDataDictionaryAsString").Uri.ToString(), complexDataDictionaryAsStringReqDTO);

            // Log the DTO object received
            Logger.LogDebug($"in PostComplexDataDictionaryAsString: complexDataDictionaryAsStringRspDTO.Dump(): {complexDataDictionaryAsStringRspDTO.Dump()}");
            // Display the received DTO object in the GUI as a string using Dump()
            RspComplexDataDictionaryAsStringRspDTODumped=complexDataDictionaryAsStringRspDTO.Dump();

            // Extract the string object from the payload
            var rspComplexDataDictionaryAsString = complexDataDictionaryAsStringRspDTO.ComplexDataDictionaryAsString;
            // Log that string
            Logger.LogDebug($"in PostComplexDataDictionaryAsString: rspComplexDataDictionaryAsString: {rspComplexDataDictionaryAsString.Dump()}");
            // Display the string object in the GUI as a string using Dump()
            RspComplexDataDictionaryAsStringDumped=rspComplexDataDictionaryAsString.Dump();

            // Deserialize the response from a string to a ComplexData using the ServiceStack Json-deserializer
            ComplexDataDictionary rspComplexDataDictionary = ServiceStack.Text.JsonSerializer.DeserializeFromString<ComplexDataDictionary>(rspComplexDataDictionaryAsString);
            // Log the rspComplexDataDictionary using Dump()
            Logger.LogDebug($"in PostComplexDataDictionaryAsString: rspComplexDataDictionary.Dump(): {rspComplexDataDictionary.Dump()}");
            // Display the rspComplexDataDictionary in the GUI as a string using Dump()
            RspComplexDataDictionaryDumped=rspComplexDataDictionary.Dump();

            // Copy the rspComplexDataDictionary to the GUI ComplexDataDictionaryP object, replacing the current with the new
            ComplexDataDictionaryP=rspComplexDataDictionary;
            // update the ComplexDataDictionaryDumped in the GUI as a string using Dump()
            ComplexDataDictionaryDumped=ComplexDataDictionaryP.Dump();

            Logger.LogTrace($"Leaving PostComplexDataDictionaryAsString");
        }
        #endregion
        #endregion
        #region Properties

        #region Properties:Demo1
        public string DataToPost { get; set; }
        public string StringDataObjectReceivedFromPost { get; set; }
        public string RspSimpleDataRspDTODumped { get; set; }
        #endregion

        #region Properties:Demo2
        #region Properties:Demo2:ComplexData
        public ComplexData ComplexDataP { get; set; }
        public string ComplexDataDumped { get; set; }
        public string ComplexDataAsStringDumped { get; set; }
        public string ReqComplexDataDumped { get; set; }
        public string ReqComplexDataAsStringDumped { get; set; }
        public string ReqComplexDataAsStringReqDTODumped { get; set; }
        public string RspComplexDataAsStringRspDTODumped { get; set; }
        public string RspComplexDataAsStringDumped { get; set; }
        public string RspComplexDataDumped { get; set; }
        #endregion
        #region Properties:Demo2:ComplexDataDictionary
        public ComplexDataDictionary ComplexDataDictionaryP { get; set; }
        public string ComplexDataDictionaryDumped { get; set; }
        public string ComplexDataAsStringDictionaryDumped { get; set; }
        public string  ReqComplexDataDictionaryDumped { get; set; }
        public string ReqComplexDataDictionaryAsStringDumped { get; set; }
        public string ReqComplexDataDictionaryAsStringReqDTODumped { get; set; }
        public string RspComplexDataDictionaryAsStringRspDTODumped { get; set; }
        public string RspComplexDataDictionaryAsStringDumped { get; set; }
        public string RspComplexDataDictionaryDumped { get; set; }
        #endregion
        #endregion

        #endregion
    }
}
