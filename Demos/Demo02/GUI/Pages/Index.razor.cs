
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components;
// Required for the logger/logging
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GUI.Pages {
    public class IndexCodeBehind : ComponentBase {

 
        #region DI container Auto-wired properties

        // Access the Logging extensions registered in the DI container
        [Inject]
        public ILogger<IndexCodeBehind> Logger { get; set; }

        #endregion

        # region Properties local to the page
        //  Create a simple integer Property for the Page
        public int AnIntegerProperty { get; set; }
        #endregion

        #region the button's OnClick Handler
        public void IncrementAnIntegerPropertyButtonOnClick() {
            Logger.LogTrace("Starting IncrementAnIntegerPropertyButtonOnClick");
            AnIntegerProperty+=1;
            Logger.LogTrace("Leaving IncrementAnIntegerPropertyButtonOnClick");
        }
        #endregion
    }


    // ToDo: Localize these strings
    public static class StringConstants {
        public const string ThirdPartyLinkCautionMessage = "As always be cautious about clicking on links. These are not under our control, so make sure your anti-malware precautions are operational before following any of these third-party links.";
    }
}