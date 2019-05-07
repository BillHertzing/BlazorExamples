using ServiceStack;
using System;
using System.Collections.Generic;

namespace CommonDTOs {
    #region Demo01
    #region InitializationReqDTO, InitializationRspDTO classes and Initialization route
    [Route("/Initialization")]
    public class InitializationReqDTO : IReturn<InitializationRspDTO> {
        public InitializationReqDTO() { }
    }
    public class InitializationRspDTO {
        public InitializationRspDTO() { }
    }
    #endregion

    #region PostDataReqDTO, PostDataRspDTO classes and PostData route
    [Route("/PostData")]
    public class PostDataReqDTO : IReturn<PostDataRspDTO> {
        public PostDataReqDTO() : this(string.Empty) { }
        public PostDataReqDTO(string stringDataObject) {
            StringDataObject=stringDataObject;
        }
        public string StringDataObject { get; set; }
    }
    public class PostDataRspDTO {
        public PostDataRspDTO() : this(string.Empty) { }

        public PostDataRspDTO(string stringDataObject) {
            StringDataObject=stringDataObject;
        }
        public string StringDataObject { get; set; }
    }
    #endregion
    #endregion

    #region Demo02 
    #region ComplexData and ComplexDataDictionary classes
    public class ComplexData {
        public ComplexData() : this(string.Empty, DateTime.UtcNow, new TimeSpan(), 0, 0, decimal.Zero) { }

        public ComplexData(string stringData, DateTime dateTimeData, TimeSpan timeSpanData, int intData, double doubleData, decimal decimalData) {
            StringData=stringData;
            DateTimeData=dateTimeData;
            TimeSpanData=timeSpanData;
            IntData=intData;
            DoubleData=doubleData;
            DecimalData=decimalData;
        }

        public string StringData { get; set; }
        public DateTime DateTimeData { get; set; }
        public TimeSpan TimeSpanData { get; set; }
        public int IntData { get; set; }
        public double DoubleData { get; set; }
        public decimal DecimalData { get; set; }
    }

    public class ComplexDataDictionary {
        public ComplexDataDictionary() :this(new Dictionary<string, ComplexData>()) {
        }

        public ComplexDataDictionary(Dictionary<string, ComplexData> complexDataDict) {
            ComplexDataDict=complexDataDict;
        }

        public Dictionary<string, ComplexData> ComplexDataDict { get; set; }
    }
    #endregion

    #region PostComplexDataAsString Clases and route attribute
    [Route("/PostComplexDataAsString")]
    public class ReqComplexDataAsStringDTO : IReturn<RspComplexDataAsStringDTO> {
        public string ComplexDataAsString { get; set; }
    }
    public class RspComplexDataAsStringDTO {
        public string ComplexDataAsString { get; set; }
    }
    #endregion

    #region PostComplexDataDictionary DTO classes and route attribute
    [Route("/PostComplexDataDictionaryAsString")]
    public class ReqComplexDataDictionaryAsStringDTO : IReturn<RspComplexDataDictionaryAsStringDTO> {
        public string ComplexDataDictionaryAsString { get; set; }
    }
    public class RspComplexDataDictionaryAsStringDTO {
        public string ComplexDataDictionaryAsString { get; set; }
    }
    #endregion
    #endregion
}
