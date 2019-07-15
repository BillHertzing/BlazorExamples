
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components;
// Required for the logger/logging
using Microsoft.Extensions.Logging;
// For Sleep in simulating a long-running Action 
using System.Threading.Tasks;
// For browser-local persistence
using Blazored.LocalStorage;
// For the array of event handler methods
using System.Collections.Generic;
// For the Description Attribute on the State enumerations
using System.ComponentModel;

using System;
// For quering and selecting State triggers 
using System.Linq;

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

        // state provides browser-local storage, and allows the property to participate in state transitions
        //  Create a public integer Property, backed by state.
        // [State Visual=True] // This attribute and its parameters will be defined later
        public int AnIntegerProperty {
            get {
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
                // ToDo: Raise OnPropertyChangeNotify event for the state-backed Property
                Logger.LogDebug("leaving AnIntegerProperty_set");
            }
        }

        #endregion

        #region Declaration of Properties page-local assigned to HTML element attributes
        // Blazor code cannot directly access DOM elements, but HTML elements on the Razor page can reference Blazor properties, and Blazor can modify these properties in code


        #region IncrementAnIntegerPropertyButton
        // IncrementAnIntegerPropertyButton HTML element 
        // Create local properties for the elements HTML attributes

        #region Visual Attributes
        public string IncrementAnIntegerPropertyButtonText;
        public string IncrementAnIntegerPropertyButtonClass;
        public string IncrementAnIntegerPropertyButtonStyle;
        #endregion

        #region state transition attributes
        // Create local properties for the element's Func<Task>()  HTML attributes (AKA event handler methods)
        // We will choose to use async versions of the event handlers so the methods return Task objects
        #region OnClick
        public Func<Task> IncrementAnIntegerPropertyButtonOnClickHandler;
        // Create local properties for the elements state transition trigger for each of the element's Func<Task>()  HTML attributes
        public TriggerStates IncrementAnIntegerPropertyButtonOnClickTriggerState { get; set; }
        #endregion
        #endregion
        #endregion

        #region AnIntegerProperty
        // AnIntegerProperty HTML element 
        // Create local properties for the element's HTML attributes
        // Create local properties for the element's visual  HTML attributes
        #region Visible attributes
        //  TextSpanStyle attribute
        public string AnIntegerPropertyTextSpanStyle;
        #endregion
        #endregion

        #region State structures
        // A structure to hold multiple state transition trigger handlers ( and hence multiple event handlers).
        //  The structure is populated via the page lifecycle event OnInitAsync
        //  The structure is populated with all the state transition trigger handlers for the program
        //  The structure is manually constructred for this demo
        public IEnumerable<StateTransitionTriggerHandler> AllStateTransitionTriggerHandlers;


        #endregion

        #endregion

        #region Page Initialization Handler
        // This method is automagically called by the Blazor runtime as part of a page's lifecycle
        protected override async Task OnInitAsync() {
            Logger.LogDebug($"Starting Index.OnInitAsync");

            #region InitializeState
            // Create all the state triggers
            // ToDo: move to a separate assembly
            AllStateTransitionTriggerHandlers=new List<StateTransitionTriggerHandler>() {
                new StateTransitionTriggerHandler(){ElementName= "IncrementAnIntegerProperty", ElementType= "Button", TriggerKind= StateTriggerKinds.OnClick, TriggerState = TriggerStates.Active, MethodToUse=IncrementAnIntegerPropertyButtonOnClickTriggerActive },
                new StateTransitionTriggerHandler(){ElementName= "IncrementAnIntegerProperty", ElementType= "Button", TriggerKind= StateTriggerKinds.OnClick, TriggerState = TriggerStates.Enqueue, MethodToUse=IncrementAnIntegerPropertyButtonOnClickTriggerEnqueue },
            };
            #endregion

            #region Initialize all the HTML attributes for all the HTML elements

            #region IncrementAnIntegerPropertyButton
            // IncrementAnIntegerPropertyButton HTML element 
            // assign values to the local properties corresonding to the element's HTML attributes
            #region Visible attributes
            // assign values to the local properties corresonding to the element's visible HTML attributes
            IncrementAnIntegerPropertyButtonClass=$"\"btn btn-primary\"";
            IncrementAnIntegerPropertyButtonStyle="background-color:black;color:white;";
            IncrementAnIntegerPropertyButtonText=$"click to increment";
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonText = {IncrementAnIntegerPropertyButtonText};  class = {IncrementAnIntegerPropertyButtonClass}; style = {IncrementAnIntegerPropertyButtonStyle};");
            #endregion

            #region state transition attributes
            // Initialize local properties for the element's Func<Task>()  HTML attributes (AKA event handler methods)
            // We will choose to use async versions of the event handlers so the methods return Task objects
            // initialize local properties for the elements state transition trigger for each of the element's Func<Task>()  HTML attributes
            #region OnClick
            try {
                // Start with All trigger handlers,
                IncrementAnIntegerPropertyButtonOnClickHandler=AllStateTransitionTriggerHandlers
                // use LINQ query to select just those triggerHandlers(s) that match the element's name, type, kind and having an Active TriggerState
                    .Where(triggerHandler => triggerHandler.ElementName=="IncrementAnIntegerProperty"
                        &&triggerHandler.ElementType=="Button"
                        &&triggerHandler.TriggerKind==StateTriggerKinds.OnClick&&
                        triggerHandler.TriggerState==TriggerStates.Active)
                    // materialize the query into a single StateTransitionTriggerHandler instance
                    // Zero or more than 1 is an error, implies a mistake in the state triggers definitions
                    .Single()
                    // pull the state program assigned to this trigger handler and assign it to the elements OnClick.
                    // i.e., hook the button's OnClick to the Active state Event Handler method
                    .MethodToUse;
            }
            catch (Exception e) {
                Logger.LogError(StringConstants.StateProgramExceptionMessage);
            }
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonOnClickHandler = {IncrementAnIntegerPropertyButtonOnClickHandler}");
            // Initialize the TriggerState for this element
            IncrementAnIntegerPropertyButtonOnClickTriggerState = TriggerStates.Active;
            #endregion
            #endregion
            #endregion

            #region AnIntegerProperty
            // AnIntegerProperty HTML element 
            // assign values to the local properties corresonding to the element's HTML attributes
            #region Visible attributes
            // assign values to the local properties corresonding to the element's visible HTML attributes
            // AnIntegerPropertyTextSpan
            AnIntegerPropertyTextSpanStyle="background-color:black;color:white;";
            #endregion

            #region state transition attributes
            // Initialize local properties for the element's Func<Task>()  HTML attributes (AKA event handler methods)
            // We will choose to use async versions of the event handlers so the methods return Task objects
            // ToDo: Connect (enable) the AnIntegerPropertyOnNotifyPropertyChange event handler method to the IncrementAnIntegerProperty's OnNotifyPropertyChange event 
            // initialize local properties for the elements state transition trigger for each of the element's Func<Task>()  HTML attributes
            // Initialize the TriggerState for this element / this trigger
            // ToDo: AnIntegerPropertyOnNotifyPropertyChangeTriggerState = TriggerStates.Active
            #endregion
            #endregion
            #endregion

            Logger.LogDebug($"Leaving Index.OnInitAsync");
        }

        // Don't worry about disconnecting the event handler from the event, as the page's dispose method will
        // ToDo: better understanding and write-up of how "navigating away from the page will eventually trigger a lifecycle event that removes the OnClick handler from the element"

        #endregion

        #region Event Handlers (this Demos's State programs)

        #region the input buttons elements OnClick Handlers
        #region IncrementAnIntegerPropertyButton
        // State program for (AKA Event Handler) for IncrementAnIntegerProperty button when the trigger is Active
        public async Task IncrementAnIntegerPropertyButtonOnClickTriggerActive() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyButtonOnClickTriggerActive");
            // Set the IncrementAnIntegerPropertyButtonOnClick's current event handler to the Enqueue handler
            try {
                // Start with All trigger handlers,
                IncrementAnIntegerPropertyButtonOnClickHandler=AllStateTransitionTriggerHandlers
                // use LINQ query to select just those triggerHandlers(s) that match the element's name, type, kind and having an Active TriggerState
                    .Where(triggerHandler => triggerHandler.ElementName=="IncrementAnIntegerProperty"
                        &&triggerHandler.ElementType=="Button"
                        &&triggerHandler.TriggerKind==StateTriggerKinds.OnClick&&
                        triggerHandler.TriggerState==TriggerStates.Enqueue)
                    // materialize the query into a single StateTransitionTriggerHandler instance
                    // Zero or more than 1 is an error, implies a mistake in the state triggers definitions
                    .Single()
                    // pull the state program assigned to this trigger handler and assign it to the elements OnClick.
                    // i.e., hook the button's OnClick to the Enqueue state Event Handler method
                    .MethodToUse;
            }
            catch (Exception e) {
                Logger.LogError(StringConstants.StateProgramExceptionMessage);
            }
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonOnClickHandler = {IncrementAnIntegerPropertyButtonOnClickHandler}");
            // Set the new TriggerState for this element
            IncrementAnIntegerPropertyButtonOnClickTriggerState=TriggerStates.Enqueue;

            // ToDo: store the mutating Property's OnNotifyPropertyChange StateTriggerState
            // ToDo: set the mutating Property's OnNotifyPropertyChange StateTriggerState to Enqueue
            // ToDo: set the mutating Property's OnNotifyPropertyChange Handler to its Enqueue method

            // Modify the visual attributes of all elements affected by this state program
            // Update visual attributes of the mutating element to show it is being modified
            AnIntegerPropertyTextSpanStyle="background-color:orange;color:white;";
            // Update visual attributes of the triggering element to show it has had a state transition pre-Action
            IncrementAnIntegerPropertyButtonClass=$"\"btn btn-primary disabled\"";
            IncrementAnIntegerPropertyButtonStyle="background-color:white;color:black;";
            IncrementAnIntegerPropertyButtonText=$"click to enqueue";
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonText = {IncrementAnIntegerPropertyButtonText};  class = {IncrementAnIntegerPropertyButtonClass}; style = {IncrementAnIntegerPropertyButtonStyle};");

            // Tell Blazor to re-render, when execution comes back to the GUI thread
            StateHasChanged();
            // Execution will come back to the calling (usually the main GUI) thread as soon as the following await is hit (unless it completes immediately? is that a thing to worry about?)
            // Perform the action on the state Property and await it
            // For the demo, an async lambda performs the action, which runs a Task and returns that Task to the event handler right away
            await Task.Run(async () => {
                AnIntegerProperty+=1;
                // simulate a 2 second duration in the action operation
                System.Threading.Thread.Sleep(2000);
            });
            // Code from here until the end of the event handler is put into a TaskContinuation and automagically run after the previous Task.Run completes (after the Action that modifies the AnIntegerProperty completes)
            // Code from here to the end is not executed until later, so the event handler effectively returns to the GUI thread right here, while awaiting the async lambda
            // This continuationTask will execute on (an unknown? the main GUI?) thread after the async Action completes
            // ToDo: ensure this TaskContinuation is run on a non-GUI thread (?)
            // Modify the visual attributes of all elements affected by this state program
            // Update visual attributes of the triggering element to show it has had a state transition post-Action
            IncrementAnIntegerPropertyButtonClass=$"\"btn btn-primary\"";
            IncrementAnIntegerPropertyButtonStyle="background-color:black;color:white;";
            IncrementAnIntegerPropertyButtonText=$"click to increment";
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonText = {IncrementAnIntegerPropertyButtonText};  class = {IncrementAnIntegerPropertyButtonClass}; style = {IncrementAnIntegerPropertyButtonStyle};");

            // Update visual attributes of the mutating element to show it is no longer being modified
            AnIntegerPropertyTextSpanStyle="background-color:black;color:white;";

            // ToDo: retrieve the mutating Property's OnNotifyPropertyChange StateTriggerState
            // ToDo: set the mutating Property's OnNotifyPropertyChange StateTriggerState to the retrieved value
            // ToDo: set the mutating Property's OnNotifyPropertyChange Handler to its StateTriggerState retrieved value method

            // Set the IncrementAnIntegerPropertyButtonOnClick's current event handler to the Active handler
            try {
                // Start with All trigger handlers,
                IncrementAnIntegerPropertyButtonOnClickHandler=AllStateTransitionTriggerHandlers
                // use LINQ query to select just those triggerHandlers(s) that match the element's name, type, kind and having an Active TriggerState
                    .Where(triggerHandler => triggerHandler.ElementName=="IncrementAnIntegerProperty"
                        &&triggerHandler.ElementType=="Button"
                        &&triggerHandler.TriggerKind==StateTriggerKinds.OnClick&&
                        triggerHandler.TriggerState==TriggerStates.Active)
                    // materialize the query into a single StateTransitionTriggerHandler instance
                    // Zero or more than 1 is an error, implies a mistake in the state triggers definitions
                    .Single()
                    // pull the state program assigned to this trigger handler and assign it to the elements OnClick.
                    // i.e., hook the button's OnClick to the Active state Event Handler method
                    .MethodToUse;
            }
            catch (Exception e) {
                Logger.LogError(StringConstants.StateProgramExceptionMessage);
            }
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonOnClickHandler = {IncrementAnIntegerPropertyButtonOnClickHandler}");
            // Change the IncrementAnIntegerPropertyButtonOnClick trigger state
            IncrementAnIntegerPropertyButtonOnClickTriggerState=TriggerStates.Active;

            // Tell Blazor to re-render when the GUI thread follows-up on the TaskContinuation
            StateHasChanged();
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyButtonOnClickTriggerActive");
            // The continuation task ends here, and ToDo: execution goes where?
        }

        // Event Handler for the button when the trigger is Enqueue
        public async Task IncrementAnIntegerPropertyButtonOnClickTriggerEnqueue() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyButtonOnClickTriggerEnqueue");
            // ToDo: record the event in a non-visual State property
            await Task.Run(async () => {
                // simulate a 1 millisecond second duration in the action operation
                System.Threading.Thread.Sleep(1);
            });
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyButtonOnClickTriggerEnqueue");
        }
        #endregion
        #endregion
        #endregion

    }

    // Create an enumeration for the states that a trigger can be in
    public enum TriggerStates {
        //ToDo: Add [LocalizedDescription("Active", typeof(Resource))]
        [Description("Active")]
        Active,
        [Description("Enqueue")]
        Enqueue
    }

    // Create an enumeration for the kinds of triggers
    public enum StateTriggerKinds {
        //ToDo: Add an attribute for "<input class=>" value
        //ToDo: Add [LocalizedDescription("OnClick", typeof(Resource))]
        //ToDo: Add an attribute for "<input class=>" value
        [Description("OnClick")]
        OnClick
    }

    // A class for identifying StateTransitionTrigger handlers
    public class StateTransitionTriggerHandler {
        public string ElementName;
        public string ElementType;
        public StateTriggerKinds TriggerKind;
        public TriggerStates TriggerState;
        public Func<Task> MethodToUse;

        public StateTransitionTriggerHandler() {
        }

        public StateTransitionTriggerHandler(string elementName, string elementType, StateTriggerKinds triggerKind, TriggerStates triggerState, Func<Task> methodToUse) {
            ElementName=elementName;
            ElementType=elementType;
            TriggerKind=triggerKind;
            TriggerState=triggerState;
            MethodToUse=methodToUse;
        }
    }

    // ToDo: Localize these strings
    public static class StringConstants {
        public const string StateProgramExceptionMessage = "The StateProgram is invalid, The number of state program methods for the ElementTypeState is not 1. Element = {Element}";
        public const string CannotParseLocalStorageForAnIntegerPropertyExceptionMessage = "The value returned from LocalStorage for AnIntegerProperty cannot be parsed to an int";
        public const string ThirdPartyLinkCautionMessage = "As always be cautious about clicking on links. These are not under our control, so make sure your anti-malware precautions are operational before following any of these third-party links.";
    }

}