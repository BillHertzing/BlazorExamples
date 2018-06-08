using System.Net;
using Ace.AceService.BaseServicesModel;
using ServiceStack;
using ServiceStack.Configuration;

namespace Ace.AceService.BaseServicesInterface {
    public class BaseServices : Service {
        public object Any(BaseServiceIsAlive request) {
            return new IsAliveResponse { Result = $"Hello, the Name you sent me is {request.Name}" };
        }
    }
}
