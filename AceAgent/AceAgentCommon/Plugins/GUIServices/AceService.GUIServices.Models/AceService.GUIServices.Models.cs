using ServiceStack;


namespace Ace.AceService.GUIServices.Models
{

  // This route will ensure that serviceStack has the GUI PlugIn loaded
  [Route("/VerifyGUI")]
  [Route("/VerifyGUI/{Kind};{Version}")]
  public class VerifyGUIRequest : IReturn<VerifyGUIResponse>
    {
        public string Kind { get; set; }
        public string Version { get; set; }
    }
    public class VerifyGUIResponse
    {
        public string Result { get; set; }
    }
}
