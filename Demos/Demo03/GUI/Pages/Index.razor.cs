
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components;
// Required for the logger/logging
using Microsoft.Extensions.Logging;
// For the OnInit Async task
using System.Threading.Tasks;
// Required for keeping State
using Blazored.LocalStorage;

namespace GUI.Pages {
    public class IndexCodeBehind : ComponentBase {

        #region DI container Auto-wired properties

        // Access the synchronous LocalStorage extensions registered in the DI container
        [Inject]
        public Blazored.LocalStorage.ISyncLocalStorageService LStorage { get; set; }

        // Access the Logging extensions registered in the DI container
        [Inject]
        public ILogger<IndexCodeBehind> Logger { get; set; }

        #endregion

        # region Properties backed by browser-local storage
        //  Create an integer Property, backed by LocalStorage. The integer Property is local to this page
        public int AnIntegerProperty { get {;
                Logger.LogDebug("entering AnIntegerProperty_get");
                string anIntString = LStorage.GetItem<string>("Index.AnIntegerProperty");
                int anInt;
                if (string.IsNullOrEmpty(anIntString))
                {
                    anInt = default;
                } else {
                    bool success = int.TryParse(anIntString, out anInt);
                    if (!success) {
                        // The browser-local-storage is not immune to tampering, so do not trust the string value returned
                        // ToDo: figure out how to safely validate the (erroneous) string returned from browser-local storage
                        // ToDo: Safely display in the exception message the (erroneous) string
                        throw new System.Exception(StringConstants.CannotParseLocalStorageForAnIntegerPropertyExceptionMessage);
                    } 
                }
                Logger.LogDebug("leaving AnIntegerProperty_get");
                return anInt;
            }
            set {
                Logger.LogDebug("entering AnIntegerProperty_set");
                LStorage.SetItem("Index.AnIntegerProperty",  value.ToString());
                Logger.LogDebug("leaving AnIntegerProperty_set");
            }
        }
        #endregion

        # region Properties local to the page
        //  Create a simple integer Property for the Page
        #endregion

        #region Page Initialization Handler
        // This method is automagically called by the Blazor runtime as part of a page's lifecycle
        protected override async Task OnInitAsync() {
            Logger.LogDebug($"Starting Index.OnInitAsync");
            Logger.LogDebug($"Leaving Index.OnInitAsync");
        }
        #endregion

        #region the button's OnClick Handler
        public void IncrementAnIntegerPropertyButtonOnClick() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyButtonOnClick");
            AnIntegerProperty+=1;
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyButtonOnClick");
        }
        #endregion
    }

    // ToDo: Localize these strings
    public static class StringConstants {
        public const string ThirdPartyLinkCautionMessage = "As always be cautious about clicking on links. These are not under our control, so make sure your anti-malware precautions are operational before following any of these third-party links.";
        public const string CannotParseLocalStorageForAnIntegerPropertyExceptionMessage = "The value returned from LocalStorage for AnIntegerProperty cannot be parsed to an int";
    }
}
