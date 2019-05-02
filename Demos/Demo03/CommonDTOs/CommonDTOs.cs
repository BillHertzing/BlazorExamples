using ServiceStack;
using System;
using System.Collections.Generic;

namespace CommonDTOs {
    #region Initialization PostData DTO classes and route attribute
    [Route("/Initialization")]
    public class InitializationReqDTO : IReturn<InitializationRspDTO> {
    }
    public class InitializationRspDTO {
    }
    #endregion

    #region PostData DTO classes and route attribute
    [Route("/PostData")]
    public class PostDataReqDTO : IReturn<PostDataRspDTO> {
        public string StringDataObject { get; set; }
    }
    public class PostDataRspDTO {
        public string StringDataObject { get; set; }
    }
    #endregion

    #region Demo02 ComplexData and ComplexDataDictionary types
    public class ComplexData {
        public string StringData { get; set; }
        public DateTime DateTimeData { get; set; }
        public TimeSpan TimeSpanData { get; set; }
        public int IntData { get; set; }
        public double DoubleData { get; set; }
        public decimal DecimalData { get; set; }
    }

    public class ComplexDataDictionary {
        public Dictionary<string, ComplexData> ComplexDataDict { get; set; }
    }
    #endregion

    #region Demo02 PostComplexDataAsString Clases and route attribute
    [Route("/PostComplexDataAsString")]
    public class ReqComplexDataAsStringDTO : IReturn<RspComplexDataAsStringDTO> {
        public string ComplexDataAsString { get; set; }
    }
    public class RspComplexDataAsStringDTO {
        public string ComplexDataAsString { get; set; }
    }
    #endregion

    #region Demo02 PostComplexDataDictionary DTO classes and route attribute
    [Route("/PostComplexDataDictionaryAsString")]
    public class ReqComplexDataDictionaryAsStringDTO : IReturn<RspComplexDataDictionaryAsStringDTO> {
        public string ComplexDataDictionaryAsString { get; set; }
    }
    public class RspComplexDataDictionaryAsStringDTO {
        public string ComplexDataDictionaryAsString { get; set; }
    }
    #endregion
}
