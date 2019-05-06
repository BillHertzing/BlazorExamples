using ServiceStack;

namespace CommonDTOs
{
    [Route("/Initialization")]
    public class InitializationReqDTO : IReturn<InitializationRspDTO> {
        public InitializationReqDTO() {}
    }
    public class InitializationRspDTO {
        public InitializationRspDTO() {}
    }
    [Route("/PostData")]
  public class PostDataReqDTO : IReturn<PostDataRspDTO>
  {
        public PostDataReqDTO() :this(string.Empty) {
        }

        public PostDataReqDTO(string stringDataObject) {
            StringDataObject=stringDataObject;
        }

        public string StringDataObject { get; set; }
  }
  public class PostDataRspDTO
  {
        public PostDataRspDTO() :this(string.Empty) {
        }

        public PostDataRspDTO(string stringDataObject) {
            StringDataObject=stringDataObject;
        }

        public string StringDataObject { get; set; }
  }
}
