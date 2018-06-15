// Required for the HttpClient
using System.Net.Http;
using System.Threading.Tasks;
using Ace.AceService.BaseServicesModel;
// Required for the logger/logging
using Blazor.Extensions.Logging;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
// Required for the logger/logging
using Microsoft.Extensions.Logging;

namespace Ace.AceGUI.Pages {
    public class BaseServicesCodeBehind : BlazorComponent {
        // Displayed on the .cshtml page
        public string isAliveResult;

    // This syntax adds to the class a Method that accesses the DI container, and retrieves the instance having the specified type from the DI container. In this case, we are accessing a builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type. This method call returns that object from the DI container  
    [Inject]
    HttpClient HttpClient {
        get; set;
    }

    protected override async Task OnInitAsync() {
        Logger.LogDebug($"Starting OnInitAsync");
      Logger.LogDebug($"Calling GetJsonAsync<IsAliveResponse>");
      IsAliveResponse _isAliveResponse = await HttpClient.GetJsonAsync<IsAliveResponse>("/isAlive/Bill?format=json");
      Logger.LogDebug($"Returned from GetJsonAsync<IsAliveResponse>");
      isAliveResult = _isAliveResponse.Result;
      Logger.LogDebug($"Leaving OnInitAsync");
    }

    // Access the Logging extensions registered in the DI container
    [Inject]
    protected ILogger<BaseServicesCodeBehind> Logger {
        get; set;
    }
    }
}
