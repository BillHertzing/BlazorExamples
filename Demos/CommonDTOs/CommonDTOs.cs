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
    [Route("/PostSimpleData")]
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
        //public ComplexData() : this(string.Empty, 0, 0, decimal.Zero) { }
        public ComplexData() : this(string.Empty, new DateTime(), 0, 0, decimal.Zero) { }

        //public ComplexData(string stringData, int intData, double doubleData, decimal decimalData) {
                        public ComplexData(string stringData, DateTime dateTimeData, int intData, double doubleData, decimal decimalData) {
            StringData=stringData;
            DateTimeData=dateTimeData;
            IntData=intData;
            DoubleData=doubleData;
            DecimalData=decimalData;
        }

        public string StringData { get; set; }
         public DateTime DateTimeData { get; set; }
        public int IntData { get; set; }
        public double DoubleData { get; set; }
        public decimal DecimalData { get; set; }
    }

    public class ComplexDataDictionary {
        public ComplexDataDictionary() : this(new Dictionary<string, ComplexData>()) {
        }

        public ComplexDataDictionary(Dictionary<string, ComplexData> complexDataDict) {
            ComplexDataDict=complexDataDict;
        }

        public Dictionary<string, ComplexData> ComplexDataDict { get; set; }
    }
    #endregion

    #region PostComplexDataAsString Clases and route attribute
    [Route("/PostComplexDataAsString")]
    public class ComplexDataAsStringReqDTO : IReturn<ComplexDataAsStringRspDTO> {
        public ComplexDataAsStringReqDTO() : this(string.Empty) {
        }

        public ComplexDataAsStringReqDTO(string complexDataAsString) {
            ComplexDataAsString=complexDataAsString;
        }

        public string ComplexDataAsString { get; set; }
    }
    public class ComplexDataAsStringRspDTO {
        public ComplexDataAsStringRspDTO() : this(string.Empty) {
        }

        public ComplexDataAsStringRspDTO(string complexDataAsString) {
            ComplexDataAsString=complexDataAsString;
        }

        public string ComplexDataAsString { get; set; }
    }
    #endregion

    #region PostComplexDataDictionary DTO classes and route attribute
    [Route("/PostComplexDataDictionaryAsString")]
    public class ComplexDataDictionaryAsStringReqDTO : IReturn<ComplexDataDictionaryAsStringRspDTO> {
        public ComplexDataDictionaryAsStringReqDTO() : this(string.Empty) {
        }

        public ComplexDataDictionaryAsStringReqDTO(string complexDataDictionaryAsString) {
            ComplexDataDictionaryAsString=complexDataDictionaryAsString;
        }

        public string ComplexDataDictionaryAsString { get; set; }
    }
    public class ComplexDataDictionaryAsStringRspDTO {
        public string ComplexDataDictionaryAsString { get; set; }
    }
    #endregion

    #region PostComplexDataAsString Clases and route attribute
    [Route("/PostComplexData")]
    public class ComplexDataReqDTO : IReturn<ComplexDataRspDTO> {
        public ComplexDataReqDTO() : this(new ComplexData()) {
        }

        public ComplexDataReqDTO(ComplexData complexData) {
            ComplexData=complexData;
        }

        public ComplexData ComplexData { get; set; }
    }
    public class ComplexDataRspDTO {
        public ComplexDataRspDTO() : this(new ComplexData()) {
        }

        public ComplexDataRspDTO(ComplexData complexData) {
            ComplexData=complexData;
        }

        public ComplexData ComplexData { get; set; }
    }
    #endregion

    #region PostComplexDataDictionary DTO classes and route attribute
    [Route("/PostComplexDataDictionary")]
    public class ComplexDataDictionaryReqDTO : IReturn<ComplexDataDictionaryRspDTO> {
        public ComplexDataDictionaryReqDTO() : this(new ComplexDataDictionary()) {
        }

        public ComplexDataDictionaryReqDTO(ComplexDataDictionary complexDataDictionary) {
            ComplexDataDictionary=complexDataDictionary;
        }

        public ComplexDataDictionary ComplexDataDictionary { get; set; }
    }
    public class ComplexDataDictionaryRspDTO {
        public ComplexDataDictionaryRspDTO() : this(new ComplexDataDictionary()) {
        }

        public ComplexDataDictionaryRspDTO(ComplexDataDictionary complexDataDictionary) {
            ComplexDataDictionary=complexDataDictionary;
        }

        public ComplexDataDictionary ComplexDataDictionary { get; set; }
    }
    #endregion

    #endregion
}
