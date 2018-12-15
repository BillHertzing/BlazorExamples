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
    public class ReqComplexDataAsStringDemo2DTO : IReturn<RspComplexDataAsStringDemo2DTO>
    {
        public string ComplexDataAsStringDemo2 { get; set; }
    }
    public class RspComplexDataAsStringDemo2DTO
    {
        public string ComplexDataAsStringDemo2 { get; set; }
    }
    #endregion

    #region Demo02 PostComplexDataDictionary DTO classes and route attribute
    [Route("/PostComplexDataDictionaryAsString")]
    public class ReqComplexDataDictionaryAsStringDemo2DTO : IReturn<RspComplexDataDictionaryAsStringDemo2DTO>
    {
        public string ComplexDataDictionaryAsStringDemo2 { get; set; }
    }
    public class RspComplexDataDictionaryAsStringDemo2DTO
    {
        public string ComplexDataDictionaryAsStringDemo2 { get; set; }
    }
    #endregion

	#region Demo03 PostComplexDataClases and route attribute
    [Route("/PostComplexData")]
    public class ReqComplexDataDemo3DTO : IReturn<RspComplexDataDemo3DTO>
    {
        public ComplexData ComplexDataDemo3 { get; set; }
    }
    public class RspComplexDataDemo3DTO
    {
        public ComplexData ComplexDataDemo3 { get; set; }
    }
    #endregion

    #region Demo03" PostComplexDataClases and route attribute
    [Route("/PostComplexDataDictionary")]
    public class ReqComplexDataDictionaryDemo3DTO : IReturn<RspComplexDataDictionaryDemo3DTO>
    {
        public ComplexDataDictionary ComplexDataDictionaryDemo3 { get; set; }
    }
    public class RspComplexDataDictionaryDemo3DTO
    {
        public ComplexDataDictionary ComplexDataDictionaryDemo3 { get; set; }
    }
    #endregion
}
