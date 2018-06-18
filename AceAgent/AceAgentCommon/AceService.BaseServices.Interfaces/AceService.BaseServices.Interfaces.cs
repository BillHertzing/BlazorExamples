using System.Net;
using Ace.AceService.BaseServices.Models;
using ServiceStack;
using ServiceStack.Configuration;

namespace Ace.AceService.BaseServices.Interfaces {
    public class BaseServices : Service {
        public object Any(BaseServiceIsAlive request) {
            return new IsAliveResponse { Result = $"Hello, the Name you sent me is {request.Name}" };
        }
    }
}
