
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components;
// Required for the logger/logging
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GUI.Pages {
    public class IndexCodeBehind : ComponentBase {

        #region String Constants
        // ToDo: Eventually replace with localization
        #endregion

        #region DI container Auto-wired properties

        // Access the Logging extensions registered in the DI container
        [Inject]
        public ILogger<IndexCodeBehind> Logger { get; set; }

        #endregion

        # region Properties local to the page
        //  Create a simple integer Property for the Page
        public int aIntProperty { get; set; }
        #endregion

        #region the button's OnClick Handler
        public void Button1OnClick() {
            Logger.LogTrace($"Starting Button1OnClick");
            aIntProperty+=1;
            Logger.LogTrace($"Leaving Button1OnClick");
        }
        #endregion
    }
}
