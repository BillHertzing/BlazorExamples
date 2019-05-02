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

    #region ComplexData and ComplexDataDictionary types
    public class ComplexData
    {
        public ComplexData() :this(String.Empty, DateTime.Now, TimeSpan.Zero, 0, 0, Decimal.Zero) {}

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

    public class ComplexDataDictionary : IEquatable<ComplexDataDictionary> {
        public ComplexDataDictionary() :this(new Dictionary<string, ComplexData>()){
        }

        public ComplexDataDictionary(Dictionary<string, ComplexData> complexDataDict) {
            ComplexDataDict=complexDataDict;
        }

        public Dictionary<string, ComplexData> ComplexDataDict { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as ComplexDataDictionary);
        }

        public bool Equals(ComplexDataDictionary other) {
            return other!=null&&
                   EqualityComparer<Dictionary<string, ComplexData>>.Default.Equals(ComplexDataDict, other.ComplexDataDict);
        }

        public override int GetHashCode() {
            return -2127441625+EqualityComparer<Dictionary<string, ComplexData>>.Default.GetHashCode(ComplexDataDict);
        }

        public static bool operator ==(ComplexDataDictionary left, ComplexDataDictionary right) {
            return EqualityComparer<ComplexDataDictionary>.Default.Equals(left, right);
        }

        public static bool operator !=(ComplexDataDictionary left, ComplexDataDictionary right) {
            return !(left==right);
        }
    }
	#endregion


    #region PostComplexData and PostComplexDataDictionary DTO Classes and route attribute
    [Route("/PostComplexData")]
    public class ComplexDataReqDTO : IReturn<ComplexDataRspDTO>
    {
        public ComplexDataReqDTO() : this(new ComplexData()) {
        }

        public ComplexDataReqDTO(ComplexData complexData) {
            ComplexData=complexData;
        }

        public ComplexData ComplexData { get; set; }
    }
    public class ComplexDataRspDTO
    {
        public ComplexDataRspDTO() :this(new ComplexData()) {
        }

        public ComplexDataRspDTO(ComplexData complexData) {
            ComplexData=complexData;
        }

        public ComplexData ComplexData { get; set; }
    }

    [Route("/PostComplexDataDictionary")]
    public class ComplexDataDictionaryReqDTO : IReturn<ComplexDataDictionaryRspDTO>
    {
        public ComplexDataDictionaryReqDTO() {
        }

        public ComplexDataDictionaryReqDTO(ComplexDataDictionary complexDataDictionary) {
            ComplexDataDictionary=complexDataDictionary;
        }

        public ComplexDataDictionary ComplexDataDictionary { get; set; }
    }
    public class ComplexDataDictionaryRspDTO
    {
        public ComplexDataDictionaryRspDTO() :this (new ComplexDataDictionary()) {
        }

        public ComplexDataDictionaryRspDTO(ComplexDataDictionary complexDataDictionary) {
            ComplexDataDictionary=complexDataDictionary;
        }

        public ComplexDataDictionary ComplexDataDictionary { get; set; }
    }
    #endregion
}
