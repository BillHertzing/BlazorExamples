using ServiceStack;

namespace CommonDTOs
{
  [Route("/Initialization")]
        public class InitializationReqDTO : IReturn<InitializationRspDTO>
  {
  }
  public class InitializationRspDTO
  {
  }
  [Route("/PostData")]
  public class PostDataReqDTO : IReturn<PostDataRspDTO>
  {
    public string StringDataObject { get; set; }
  }
  public class PostDataRspDTO
  {
    public string StringDataObject { get; set; }
  }
}
