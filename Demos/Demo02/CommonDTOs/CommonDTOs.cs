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
    public class ComplexDataDemo2
    {
        public string StringData { get; set; }
        public DateTime DateTimeData { get; set; }
        public TimeSpan TimeSpanData { get; set; }
        public int IntData { get; set; }
        public double DoubleData { get; set; }
        public decimal DecimalData { get; set; }
    }

    #region PostComplexDataAsString DTO classes and route attribute
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

    public class ComplexDataDictionaryDemo2
    {
        public Dictionary<string, ComplexDataDemo2> ComplexDataDictDemo2 { get; set; }
    }
    #region PostComplexDataDictionary DTO classes and route attribute
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
}
