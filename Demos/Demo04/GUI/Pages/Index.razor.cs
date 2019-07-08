
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components;
// Required for the logger/logging
using Microsoft.Extensions.Logging;
// For the OnInit Async task
using System.Threading.Tasks;
// For browser-local persistence
using Blazored.LocalStorage;
using System.Collections.Generic;
using System;

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

        # region Properties backed by state storage
        // state provides both browser-local storage, and allows the property to participate in state transition selection
        //  Create a public integer Property, backed by state.
        public int AnIntegerProperty {
            get {
                ;
                Logger.LogDebug("entering AnIntegerProperty_get");
                string anIntString = LStorage.GetItem<string>("Index.AnIntegerProperty");
                int anInt;
                if (string.IsNullOrEmpty(anIntString)) {
                    anInt=default;
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
                LStorage.SetItem("Index.AnIntegerProperty", value.ToString());
                Logger.LogDebug("leaving AnIntegerProperty_set");
            }
        }

        // Add a OnPropertyChangeNotify event handler for the state-backed Property
        #endregion

        #region Properties local to the page
        public string IncrementAnIntegerPropertyButtonText;
        public string IncrementAnIntegerPropertyButtonClass;
        public string IncrementAnIntegerPropertyButtonStyle;
        public Func<Task>[] IncrementAnIntegerPropertyButtonHandlers;
        public Func<Task> IncrementAnIntegerPropertyButtonHandlerCurrent;
        #endregion

        #region Page Initialization Handler
        // This method is automagically called by the Blazor runtime as part of a page's lifecycle
        protected override async Task OnInitAsync() {
            Logger.LogDebug($"Starting Index.OnInitAsync");
            // Enable the initial state triggers
            // Connect (enable) the IncrementAnIntegerPropertyButtonOnClick event handler method to the IncrementAnIntegerProperty button's OnClick event 
            TriggerActive=true;
            // create the initial element attributes and text spans
            IncrementAnIntegerPropertyButtonClass=$"\"btn btn-primary{(TriggerActive ? string.Empty : " disabled")}\"";
            IncrementAnIntegerPropertyButtonStyle="background-color:black; color:white;";
            IncrementAnIntegerPropertyButtonText=$"click to increment class = {IncrementAnIntegerPropertyButtonClass} style = {IncrementAnIntegerPropertyButtonStyle}";
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonText = {IncrementAnIntegerPropertyButtonText}");
            // create the array of onclick handlers
            IncrementAnIntegerPropertyButtonHandlers= new Func<Task>[] {() => IncrementAnIntegerPropertyButtonOnClick(), () => IncrementAnIntegerPropertyButtonOnClickEnQueue() };
            // Set the Button's Current OnClick Handler to the OnClick handler (triggerActive = true)
            IncrementAnIntegerPropertyButtonHandlerCurrent=IncrementAnIntegerPropertyButtonHandlers[0];
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonHandlerCurrent = {nameof(IncrementAnIntegerPropertyButtonHandlerCurrent)}");
            StateHasChanged();
            // Connect (enable) the IncrementAnIntegerPropertyOnNotifyPropertyChange event handler method to the IncrementAnIntegerProperty's OnNotifyPropertyChange event 
            Logger.LogDebug($"Leaving Index.OnInitAsync");
        }

        // Don't worry about dissconnecting the event handler from the event, as the page's dispose method will
        // ToDo: better understanding and write-up of how "navigating away from the page will eventually trigger a lifecycle event that removes the OnClick handler from the element"

        #endregion

        #region the buttons' OnClick Handlers
        public async Task IncrementAnIntegerPropertyButtonOnClick() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyButtonOnClick");
            TriggerActive=false;
            IncrementAnIntegerPropertyButtonClass=$"\"btn btn-primary{(TriggerActive ? string.Empty : " disabled")}\"";
            IncrementAnIntegerPropertyButtonStyle="background-color:white;color:black;";
            IncrementAnIntegerPropertyButtonText=$"click to increment class = {IncrementAnIntegerPropertyButtonClass} style = {IncrementAnIntegerPropertyButtonStyle}";
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonText = {IncrementAnIntegerPropertyButtonText}");
            // Set the Button's OnClick Handler to the EnQueue handler
            IncrementAnIntegerPropertyButtonHandlerCurrent=IncrementAnIntegerPropertyButtonHandlers[1];
            StateHasChanged();
            // ToDo: Disable the Property's OnNotifyPropertyChange Handler
            // Perform the action on the state Property and await it
            await Task.Run(async () => {
                AnIntegerProperty+=1;
                // simulate a 2 second duration in the action operation
                System.Threading.Thread.Sleep(2000);
            });
            // ToDo: Should these below be in a task continuation
            // ToDo: Enable the Property's OnNotifyPropertyChange Handler
            TriggerActive=true;
            IncrementAnIntegerPropertyButtonClass=$"\"btn btn-primary {(TriggerActive ? string.Empty : " disabled")}\"";
            IncrementAnIntegerPropertyButtonStyle="background-color:black;color:white;";
            IncrementAnIntegerPropertyButtonText=$"click to increment class = {IncrementAnIntegerPropertyButtonClass} style = {IncrementAnIntegerPropertyButtonStyle}";
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonText = {IncrementAnIntegerPropertyButtonText}");
            // Set the Button's OnClick Handler to the OnClick handler
            IncrementAnIntegerPropertyButtonHandlerCurrent=IncrementAnIntegerPropertyButtonHandlers[0];
            StateHasChanged();
            // ToDo: Should these above Sbe in a task continuation
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyButtonOnClick");
        }

        public async Task IncrementAnIntegerPropertyButtonOnClickEnQueue() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyButtonOnClickEnQueue");
            await Task.Run(async () => {
                // simulate a 1 millisecond second duration in the action operation
                System.Threading.Thread.Sleep(1);
            });
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyButtonOnClickEnQueue");
        }
        #endregion

        #region state
        public bool TriggerActive;
        #endregion
    }


    // ToDo: Localize these strings
    public static class StringConstants {
        public const string ThirdPartyLinkCautionMessage = "As always be cautious about clicking on links. These are not under our control, so make sure your anti-malware precautions are operational before following any of these third-party links.";
        public const string CannotParseLocalStorageForAnIntegerPropertyExceptionMessage = "The value returned from LocalStorage for AnIntegerProperty cannot be parsed to an int";
    }
}
