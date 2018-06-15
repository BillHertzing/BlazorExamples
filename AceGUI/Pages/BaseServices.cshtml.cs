using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
// Both are required for the logger/logging
using Microsoft.Extensions.Logging;
using Blazor.Extensions.Logging;
using Ace.AceService.BaseServicesModel;
// Required for the HttpClient
using System.Net.Http;

namespace Ace.AceGUI.Pages
{
  public class BaseServicesCodeBehind : BlazorComponent
  {
   [Inject] protected  ILogger<BaseServicesCodeBehind> Logger { get; set; }
    // This syntax adds to the class a Method that access the DI container, and retrieves the instance having teh specified type from the DI container. In this case, we are accessing a builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type. This method call returns that object from the DI container  
    [Inject] private HttpClient HttpClient { get;  }

    public string isAliveResult;

    protected override async Task OnInitAsync()
    {
      Logger.LogDebug($"Starting OnInitAsync");
      Logger.LogDebug("Creating new HTTPClient");
                //Http.BaseAddress = new System.Uri("http://localhost/21100");
        Logger.LogTrace($"Calling GetJsonAsync<IsAliveResponse>");
        IsAliveResponse _isAliveResponse = await HttpClient.GetJsonAsync<IsAliveResponse>("/isAlive/Bill?format=json");
        Logger.LogTrace($"Returned from GetJsonAsync<IsAliveResponse>");
        isAliveResult = _isAliveResponse.Result;
      Logger.LogTrace("Disposed of HTTPClient");
      Logger.LogDebug($"Leaving OnInitAsync");
    }
  }
}
