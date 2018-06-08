using System;
using System.Web;
using ServiceStack;


namespace Ace.AceService.BaseServicesModel
{
    [Route("/isAlive")]
    [Route("/isAlive/{Name}")]
    public class BaseServiceIsAlive : IReturn<IsAliveResponse>
    {
        public string Name { get; set; }
    }

    public class IsAliveResponse
    {
        public string Result { get; set; }
    }
}
