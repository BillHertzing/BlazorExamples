
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
                // ToDo: Raise OnPropertyChangeNotify event for the state-backed Property
                Logger.LogDebug("leaving AnIntegerProperty_set");
            }
        }


        #endregion

        #region Properties local to the page
        // Blazor code cannot directly access DOM elements, but HTML elements on the Razor page can reference Blazor properties, and Blazor can modify these properties in code
        // Create local properties for the Property's <Text> element's HTML attributes
        public string AnIntegerPropertyTextSpanStyle;
        // Create local properties for the [IncrementAnIntegerPropertyButton] button's HTML attributes
        public string IncrementAnIntegerPropertyButtonText;
        public string IncrementAnIntegerPropertyButtonClass;
        public string IncrementAnIntegerPropertyButtonStyle;
        // Create local properties for the State of the triggering element
        public StateTriggerStates IncrementAnIntegerPropertyButtonOnClickTriggerState { get; set; }
        // Create local properties for event handler methods to be attached to the button's events. We will choose to use async versions of the event handlers,
        //  so the methods return Task objects
        public Func<Task> IncrementAnIntegerPropertyButtonHandlerCurrent;
        // A structure to hold multiple state transition trigger handlers ( and hence multiple event handlers).
        //  The structure is populated via the page lifecycle event OnInitAsync
        //  The structure is populated with all the state transition trigger handlers for the program
        //  The structure is manually constructred for this demo
        public IEnumerable<StateTransitionTriggerHandler> AllStateTransitionTriggerHandlers;

        #endregion

        #region Page Initialization Handler
        // This method is automagically called by the Blazor runtime as part of a page's lifecycle
        protected override async Task OnInitAsync() {
            Logger.LogDebug($"Starting Index.OnInitAsync");
            // Create all the state triggers
            // ToDo: move to a separate assembly
            AllStateTransitionTriggerHandlers=new List<StateTransitionTriggerHandler>() {
                //() => {ElementName: "IncrementAnIntegerProperty", ElementType: "Button", TriggerKind: StateTriggerKinds.OnClick TriggerState:StateTriggerStates.Active MethodToUse:IncrementAnIntegerPropertyButtonOnClickTriggerActive},
                //() => {ElementName: "IncrementAnIntegerProperty", ElementType: "Button", TriggerKind: StateTriggerKinds.OnClick TriggerState:StateTriggerStates.Enqueue MethodToUse:IncrementAnIntegerPropertyButtonOnClickTriggerEnqueue},
                };
            // Enable the initial state triggers
            IncrementAnIntegerPropertyButtonOnClickTriggerState = StateTriggerStates.Active;
            // Connect (enable) the IncrementAnIntegerPropertyButtonOnClick event handler method to the IncrementAnIntegerProperty button's OnClick event 
            // create (in C#) the initial element attributes and text spans
            AnIntegerPropertyTextSpanStyle = "background-color:black;color:white;";
            IncrementAnIntegerPropertyButtonClass=$"\"btn btn-primary\"";
            IncrementAnIntegerPropertyButtonStyle="background-color:black;color:white;";
            IncrementAnIntegerPropertyButtonText=$"click to enqueue class = {IncrementAnIntegerPropertyButtonClass} style = {IncrementAnIntegerPropertyButtonStyle}";
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonText = {IncrementAnIntegerPropertyButtonText}");
            // Set the IncrementAnIntegerPropertyButtonOnClick's current event handler to the Active handler
            // Start with All trigger handlers, query select just those trigger(s) that match the IncrementAnIntegerPropertyButton button's name, type, and Active
            // A LINQ query against the Ienumerable to produce an enumerable var specific to this visual element, the Trigger Active actions to take
            // materialize the query into a single StateTransitionTriggerHandler instance
            // Zero or more than 1 is an error, implies a mistake in the state triggers definitions
            StateTransitionTriggerHandler IncrementAnIntegerPropertyButtonStateTransitionTriggerHandler;
            try {
                IncrementAnIntegerPropertyButtonStateTransitionTriggerHandler = AllStateTransitionTriggerHandlers.Where(triggerHandler => triggerHandler.ElementName=="IncrementAnIntegerProperty"
                                                                                     &&triggerHandler.ElementType=="Button"
                                                                                     &&triggerHandler.TriggerKind==StateTriggerKinds.OnClick&&
                                                                                     triggerHandler.TriggerState==StateTriggerStates.Active)
                                                                                    .Single();
            }
            catch (Exception e) {
                // ToDo: Exception message string to the StringConstants.
                Logger.LogError($"in OnInitAsync error in state triggers definitions");
            }
            // pull the state transitions commanded by this trigger.
            // Start with the trigger handler, get the MethodToUse, and assign it to the state transition event being generated by the bound visual element
            // I.E., hook the button's OnClick to the Active method
            IncrementAnIntegerPropertyButtonHandlerCurrent= IncrementAnIntegerPropertyButtonStateTransitionTriggerHandler.MethodToUse;
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonHandlerCurrent = {nameof(IncrementAnIntegerPropertyButtonHandlerCurrent)}");
            StateHasChanged();
            // Connect (enable) the IncrementAnIntegerPropertyOnNotifyPropertyChange event handler method to the IncrementAnIntegerProperty's OnNotifyPropertyChange event 
            Logger.LogDebug($"Leaving Index.OnInitAsync");
        }

        // Don't worry about disconnecting the event handler from the event, as the page's dispose method will
        // ToDo: better understanding and write-up of how "navigating away from the page will eventually trigger a lifecycle event that removes the OnClick handler from the element"

        #endregion

        #region the buttons' OnClick Handlers
		// Event Handler for the button when the trigger is Active
        public async Task IncrementAnIntegerPropertyButtonOnClickTriggerActive() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyButtonOnClickTriggerActive");
            // Connect (enable) the IncrementAnIntegerPropertyButtonOnClick event handler method to the IncrementAnIntegerProperty button's Enqueue event
            IncrementAnIntegerPropertyButtonOnClickTriggerState =  StateTriggerStates.Enqueue;
            StateTransitionTriggerHandler incrementAnIntegerPropertyButtonStateTransitionTriggerHandler;
            try {
                incrementAnIntegerPropertyButtonStateTransitionTriggerHandler=AllStateTransitionTriggerHandlers.Where(triggerHandler => triggerHandler.ElementName=="IncrementAnIntegerProperty"
                                                                                   &&triggerHandler.ElementType=="Button"
                                                                                   &&triggerHandler.TriggerKind==StateTriggerKinds.OnClick&&
                                                                                   triggerHandler.TriggerState==StateTriggerStates.Enqueue)
                                                                                    .Single();
            }
            catch (Exception e) {
                // ToDo: Exception message string to the StringConstants.
                Logger.LogError($"in OnInitAsync error in state triggers definitions");
            }
            // pull the state transitions commanded by this trigger.
            // Start with the trigger handler, get the MethodToUse, and assign it to the state transition event being generated by the bound visual element
            // I.E., hook the button's OnClick to the Active method
            IncrementAnIntegerPropertyButtonHandlerCurrent=incrementAnIntegerPropertyButtonStateTransitionTriggerHandler.MethodToUse;

            //var incrementAnIntegerPropertyButtonStateTransitionTriggerHandlersPreAction = AllStateTransitionTriggerHandlers.SelectWhere(ElementName: "IncrementAnIntegerProperty", ElementType: "Button", TriggerKind: StateTriggerKinds.OnClick, TriggerState: StateTriggerStates.Enqueue);
            // materialize the query into a single StateTransitionTriggerHandler instance
            //var incrementAnIntegerPropertyButtonStateTransitionTriggerHandlerPreAction = incrementAnIntegerPropertyButtonStateTransitionTriggerHandlersPreAction.First();
            // pull the state transitions commanded by this trigger.
            // I.E., hook the button's OnClick to the Enqueue method
            IncrementAnIntegerPropertyButtonHandlerCurrent=incrementAnIntegerPropertyButtonStateTransitionTriggerHandlerPreAction.MethodToUse;
            // ToDo: store the mutating Property's OnNotifyPropertyChange StateTriggerState
            // ToDo: set the mutating Property's OnNotifyPropertyChange StateTriggerState to Enqueue
            // ToDo: set the mutating Property's OnNotifyPropertyChange Handler to its Enqueue method
            // Update visual attributes of the mutating element to show it is being modified
            AnIntegerPropertyTextSpanStyle= "background-color:orange;color:white;";
            // Update visual attributes of the triggering element to show it has had a state transition pre-Action
            IncrementAnIntegerPropertyButtonClass=$"\"btn btn-primary disabled\"";
            IncrementAnIntegerPropertyButtonStyle="background-color:white;color:black;";
            IncrementAnIntegerPropertyButtonText=$"click to enqueue. class = {IncrementAnIntegerPropertyButtonClass} style = {IncrementAnIntegerPropertyButtonStyle}";
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonText = {IncrementAnIntegerPropertyButtonText}");
            // Tell Blazor to re-render, when execution comes back to the GUI thread
            StateHasChanged();
            // Execution will come back to the GUI thread as soon as the following await is hit (unless it completes immediately? is that a thing to worry about?)
            // ToDo: Disable the Property's OnNotifyPropertyChange Handler
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
            // Update visual attributes of the triggering element to show it has had a state transition post-Action
            IncrementAnIntegerPropertyButtonClass=$"\"btn btn-primary\"";
            IncrementAnIntegerPropertyButtonStyle="background-color:black;color:white;";
            IncrementAnIntegerPropertyButtonText=$"click to enqueue class = {IncrementAnIntegerPropertyButtonClass} style = {IncrementAnIntegerPropertyButtonStyle}";
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonText = {IncrementAnIntegerPropertyButtonText}");
            // ToDo: Set the mutating Property's OnNotifyPropertyChange Handler back to what was stored.
            AnIntegerPropertyTextSpanStyle="background-color:black;color:white;";
            // update the [ToDo: right names] triggering element's TriggerState to Active
            IncrementAnIntegerPropertyButtonOnClickTriggerState=StateTriggerStates.Active;
            // Connect (enable) the IncrementAnIntegerPropertyButtonOnClick event handler method to the IncrementAnIntegerProperty button's Active event
            var incrementAnIntegerPropertyButtonStateTransitionTriggerHandlersPostAction = AllStateTransitionTriggerHandlers.SelectWhere(ElementName : nameof(IncrementAnIntegerPropertyButton), ElementType: "Button", TriggerKind: StateTriggerKinds.OnClick TriggerState:StateTriggerStates.Active)
            // ToDo: ensure there is exactly one matching handler, anything else is an error in the state machine
            // materialize the query into a single StateTransitionTriggerHandler instance
            var incrementAnIntegerPropertyButtonStateTransitionTriggerHandlerPostAction = incrementAnIntegerPropertyButtonStateTransitionTriggerHandlersPostAction.First();
            // pull the state transitions commanded by this trigger.
            // I.E., hook the button's OnClick to the Active method
            IncrementAnIntegerPropertyButtonHandlerCurrent=incrementAnIntegerPropertyButtonStateTransitionTriggerHandlerPostAction.MethodToUse;
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

        #region state
        public bool TriggerActive;
        #endregion
    }

    // Create an enumeration for the states that a trigger can be in
    public enum StateTriggerStates {
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
        OnClick}

 
    // A structure for identifying StateTransitionTrigger handlers
    public class StateTransitionTriggerHandler {
        public string ElementName;
        public string ElementType;
        public StateTriggerKinds TriggerKind;
        public StateTriggerStates TriggerState;
        public Func<Task> MethodToUse;

        public StateTransitionTriggerHandler(string elementName, string elementType, StateTriggerKinds triggerKind, StateTriggerStates triggerState, Func<Task> methodToUse) {
            ElementName=elementName;
            ElementType=elementType;
            TriggerKind=triggerKind;
            TriggerState=triggerState;
            MethodToUse=methodToUse;
        }
    }
    
    public static class StringConstants {
        public const string StateProgramExceptionMessage = "The StateProgram is invalid, The number of state program methods for the ElementTypeState is not 1. Element = {Element}";
        public const string CannotParseLocalStorageForAnIntegerPropertyExceptionMessage = "The value returned from LocalStorage for AnIntegerProperty cannot be parsed to an int";
        public const string ThirdPartyLinkCautionMessage = "As always be cautious about clicking on links. These are not under our control, so make sure your anti-malware precautions are operational before following any of these third-party links.";
    }

}