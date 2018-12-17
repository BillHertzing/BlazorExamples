using ServiceStack;
using System;
using System.Collections.Generic;

namespace CommonDTOs
{
    #region Initialization PostData DTO classes and route attribute
    [Route("/Initialization")]
     public class InitializationReqDTO : IReturn<InitializationRspDTO>
    {
    }
    public class InitializationRspDTO
    {
    }
    #endregion

    #region PostData DTO classes and route attribute
    [Route("/PostData")]
  public class PostDataReqDTO : IReturn<PostDataRspDTO>
  {
    public string StringDataObject { get; set; }
  }
  public class PostDataRspDTO
  {
    public string StringDataObject { get; set; }
  }
    #endregion

    #region Demo02 and Demo03 ComplexData and ComplexDataDictionary types
    public class ComplexData
    {
        public string StringData { get; set; }
        public DateTime DateTimeData { get; set; }
        public TimeSpan TimeSpanData { get; set; }
        public int IntData { get; set; }
        public double DoubleData { get; set; }
        public decimal DecimalData { get; set; }
    }

    public class ComplexDataDictionary
    {
        public Dictionary<string, ComplexData> ComplexDataDict { get; set; }
    }
	#endregion

	#region Demo02 PostComplexDataAsString Clases and route attribute
    [Route("/PostComplexDataAsString")]
    public class ReqComplexDataAsStringDTO : IReturn<RspComplexDataAsStringDTO>
    {
        public string ComplexDataAsString { get; set; }
    }
    public class RspComplexDataAsStringDTO
    {
        public string ComplexDataAsString { get; set; }
    }
    #endregion

    #region Demo02 PostComplexDataDictionaryAsString DTO classes and route attribute
    [Route("/PostComplexDataDictionaryAsString")]
    public class ReqComplexDataDictionaryAsStringDTO : IReturn<RspComplexDataDictionaryAsStringDTO>
    {
        public string ComplexDataDictionaryAsString { get; set; }
    }
    public class RspComplexDataDictionaryAsStringDTO
    {
        public string ComplexDataDictionaryAsString { get; set; }
    }
    #endregion

    #region Demo03 PostComplexDataAsObject and PostComplexDataDictionary AsObject DTO Classes and route attribute
    [Route("/PostComplexDataAsObject")]
    public class ReqComplexDataAsObjectDTO : IReturn<RspComplexDataOsObjectDTO>
    {
        public ComplexData ComplexDataAsObject { get; set; }
    }
    public class RspComplexDataOsObjectDTO
    {
        public ComplexData ComplexDataAsObject { get; set; }
    }

    [Route("/PostComplexDataDictionaryAsObject")]
    public class ReqComplexDataDictionaryAsObjectDTO : IReturn<RspComplexDataDictionaryAsObjectDTO>
    {
        public ComplexDataDictionary ComplexDataDictionaryAsObject { get; set; }
    }
    public class RspComplexDataDictionaryAsObjectDTO
    {
        public ComplexDataDictionary ComplexDataDictionaryAsObject { get; set; }
    }
    #endregion
}
