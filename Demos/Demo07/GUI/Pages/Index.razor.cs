
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
// for the GUI State library
using GUI.State;
using System;
// For quering and selecting State triggers 
using System.Linq;
// for thread-safe timers "Comparing the Timer Classes in the .NET Framework Class Library" for a really old explanation... https://web.archive.org/web/20150329101415/https://msdn.microsoft.com/en-us/magazine/cc164015.aspx
// As soon as timers are added, a whole host of threading issues arise. [ToDo: Add more explanations in the blog post on this demo, then link here]
//  This class of Timers also supports synchronizationContexts and batch initialization
using System.Timers;
// For the System.Timers
using System.ComponentModel;

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

        #region IncrementAnIntegerPropertyTimerControlButton
        // IncrementAnIntegerPropertyTimerControlButton
        // Create local properties corresonding to the element's HTML attributes
        #region Visible attributes
        public string IncrementAnIntegerPropertyTimerControlButtonText;
        public string IncrementAnIntegerPropertyTimerControlButtonClass;
        public string IncrementAnIntegerPropertyTimerControlButtonStyle;
        #endregion
        #region state transition attributes
        // Create local properties for the element's Func<Task>()  HTML attributes (AKA event handler methods)
        // We will choose to use async versions of the event handlers so the methods return Task objects
        #region OnClick
        public Func<Task> IncrementAnIntegerPropertyTimerControlButtonOnClickHandler;
        // Create local properties for the elements state transition trigger for each of the element's Func<Task>()  HTML attributes
        public TriggerStates IncrementAnIntegerPropertyTimerControlButtonOnClickTriggerState { get; set; }
        #endregion
        #endregion
        #endregion

        #region Declare non-State, non-visual local objects
        #region IncrementAnIntegerPropertyTimer
        //IncrementAnIntegerPropertyTimer
        public Timer IncrementAnIntegerPropertyTimer { get; set; }
        #endregion
        #endregion

        #region IncrementAnIntegerPropertyTimer
        // IncrementAnIntegerPropertyTimer HTML element 
        // Create local properties for the element's HTML attributes
        #region Visible attributes
        public string IncrementAnIntegerPropertyTimerText;
        public string IncrementAnIntegerPropertyTimerClass;
        public string IncrementAnIntegerPropertyTimerStyle;
        #endregion
        #region state transition attributes
        // Create local properties for the element's Func<Task>()  HTML attributes (AKA event handler methods)
        // We will choose to use async versions of the event handlers so the methods return Task objects
        #region Expired
        public Func<Task> IncrementAnIntegerPropertyTimerExpiredHandler;
        // Create local properties for the elements state transition trigger for each of the element's Func<Task>()  HTML attributes
        public TriggerStates IncrementAnIntegerPropertyTimerExpiredTriggerState { get; set; }
        #endregion
        #endregion
        #endregion


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
                new StateTransitionTriggerHandler(){ElementName= "IncrementAnIntegerPropertyTimerControl", ElementType= "Button", TriggerKind= StateTriggerKinds.OnClick, TriggerState = TriggerStates.Ignore, MethodToUse=IncrementAnIntegerPropertyTimerControlButtonOnClickTriggerIgnore },
                new StateTransitionTriggerHandler(){ElementName= "IncrementAnIntegerPropertyTimerControl", ElementType= "Button", TriggerKind= StateTriggerKinds.OnClick, TriggerState = TriggerStates.Active, MethodToUse=IncrementAnIntegerPropertyTimerControlButtonOnClickTriggerActive },
                new StateTransitionTriggerHandler(){ElementName= "IncrementAnIntegerPropertyTimerControl", ElementType= "Button", TriggerKind= StateTriggerKinds.OnClick, TriggerState = TriggerStates.Enqueue, MethodToUse=IncrementAnIntegerPropertyTimerControlButtonOnClickTriggerEnqueue },
                new StateTransitionTriggerHandler(){ElementName= "IncrementAnIntegerPropertyTimer", ElementType= "Timer", TriggerKind= StateTriggerKinds.Expired, TriggerState = TriggerStates.Ignore, MethodToUse=IncrementAnIntegerPropertyTimerExpiredTriggerIgnore },
                new StateTransitionTriggerHandler(){ElementName= "IncrementAnIntegerPropertyTimer", ElementType= "Timer", TriggerKind= StateTriggerKinds.Expired, TriggerState = TriggerStates.Active, MethodToUse=IncrementAnIntegerPropertyTimerExpiredTriggerActive },
                new StateTransitionTriggerHandler(){ElementName= "IncrementAnIntegerPropertyTimer", ElementType= "Timer", TriggerKind= StateTriggerKinds.Expired, TriggerState = TriggerStates.Enqueue, MethodToUse=IncrementAnIntegerPropertyTimerExpiredTriggerEnqueue },
                new StateTransitionTriggerHandler(){ElementName= "IncrementAnIntegerProperty", ElementType= "Button", TriggerKind= StateTriggerKinds.OnClick, TriggerState = TriggerStates.Ignore, MethodToUse=IncrementAnIntegerPropertyButtonOnClickTriggerIgnore },
                new StateTransitionTriggerHandler(){ElementName= "IncrementAnIntegerProperty", ElementType= "Button", TriggerKind= StateTriggerKinds.OnClick, TriggerState = TriggerStates.Active, MethodToUse=IncrementAnIntegerPropertyButtonOnClickTriggerActive },
                new StateTransitionTriggerHandler(){ElementName= "IncrementAnIntegerProperty", ElementType= "Button", TriggerKind= StateTriggerKinds.OnClick, TriggerState = TriggerStates.Enqueue, MethodToUse=IncrementAnIntegerPropertyButtonOnClickTriggerEnqueue },
            };
            #endregion

            #region Instantiate non-State, non-visual local objects

            // In this simple Demo, the timer is created during the page initialization lifecycle event. 
            //  The timer will get destroyed and recreated each time the page is refreshed
            //  ToDo:In Blazor-Mono-Wasm-browser, do we know what thread the expired timer handlers run on? Do we care?
            //  ToDo: Investigate creating the timer in a static method, and that's impact on the thread the event handler runs on
            IncrementAnIntegerPropertyTimer=new Timer(4000) {
                AutoReset=false // The default is true, but the state program in this demo is going to control when to restart it
            };
            #endregion

            #region Initialize all the HTML attributes for all the HTML elements
            #region IncrementAnIntegerPropertyTimerControlButton
            // IncrementAnIntegerPropertyTimerControlButton
            // assign values to the local properties corresonding to the element's HTML attributes
            #region Visible attributes
            // assign values to the local properties corresonding to the element's visible HTML attributes
            IncrementAnIntegerPropertyTimerControlButtonClass=$"\"btn btn-primary\"";
            IncrementAnIntegerPropertyTimerControlButtonStyle="";
            IncrementAnIntegerPropertyTimerControlButtonText="Stop timer";
            Logger.LogDebug($"IncrementAnIntegerPropertyTimerControlButtonText = {IncrementAnIntegerPropertyButtonText};  class = {IncrementAnIntegerPropertyTimerControlButtonClass}; style = {IncrementAnIntegerPropertyTimerControlButtonStyle};");
            #endregion
            #region state transition attributes
            // assign values to the local properties for the element's Func<Task>()  HTML attributes (AKA event handler methods)
            // We will choose to use async versions of the event handlers so the methods return Task objects
            // initialize local properties for the elements state transition trigger for each of the element's Func<Task>()  HTML attributes

            #region OnClick
            // assign a state program to the local property corresonding to the element's state program for this state transition Kind
            try {
                IncrementAnIntegerPropertyTimerControlButtonOnClickHandler=AllStateTransitionTriggerHandlers.Where(triggerHandler => triggerHandler.ElementName=="IncrementAnIntegerPropertyTimerControl"
                                                                                 &&triggerHandler.ElementType=="Button"
                                                                                 &&triggerHandler.TriggerKind==StateTriggerKinds.OnClick&&
                                                                                 triggerHandler.TriggerState==TriggerStates.Active)
                                                                                    .Single().MethodToUse;
            }
            catch (Exception e) {
                Logger.LogError(StringConstants.StateProgramExceptionMessage);
            }
            Logger.LogDebug($"IncrementAnIntegerPropertyTimerControlButtonOnClickHandler = {IncrementAnIntegerPropertyTimerControlButtonOnClickHandler}");
            // Initialize the TriggerState for this element
            IncrementAnIntegerPropertyTimerControlButtonOnClickTriggerState=TriggerStates.Active;
            #endregion
            #endregion
            #endregion


            #region IncrementAnIntegerPropertyTimer
            // IncrementAnIntegerPropertyTimer HTML element 
            // assign values to the local properties corresonding to the element's HTML attributes
            // initialize local properties for the element's HTML attributes
            #region Visible attributes
            // assign values to the local properties corresonding to the element's visible HTML attributes
            IncrementAnIntegerPropertyTimerText="Running";
            IncrementAnIntegerPropertyTimerClass="";
            IncrementAnIntegerPropertyTimerStyle="Enabled";
            Logger.LogDebug($"IncrementAnIntegerPropertyTimerText = {IncrementAnIntegerPropertyButtonText};  class = {IncrementAnIntegerPropertyTimerClass}; style = {IncrementAnIntegerPropertyTimerStyle};");
            #endregion

            #region state transition attributes
            // assign values to the local properties for the element's Func<Task>()  HTML attributes (AKA event handler methods)
            // We will choose to use async versions of the event handlers so the methods return Task objects
            // initialize local properties for the elements state transition trigger for each of the element's Func<Task>()  HTML attributes
            #region Expired
            try {
                IncrementAnIntegerPropertyTimerExpiredHandler=AllStateTransitionTriggerHandlers.Where(triggerHandler => triggerHandler.ElementName=="IncrementAnIntegerPropertyTimer"
                                                                                 &&triggerHandler.ElementType=="Timer"
                                                                                 &&triggerHandler.TriggerKind==StateTriggerKinds.Expired&&
                                                                                 triggerHandler.TriggerState==TriggerStates.Active)
                                                                                    .Single().MethodToUse;
            }
            catch (Exception e) {
                Logger.LogError(StringConstants.StateProgramExceptionMessage);
            }
            //var x = IncrementAnIntegerPropertyTimerExpiredHandlerCurrent.GetMemberName<Func<Task>>(IncrementAnIntegerPropertyTimerExpiredHandlerCurrent);
            Logger.LogDebug($"IncrementAnIntegerPropertyTimerExpiredHandler = {IncrementAnIntegerPropertyTimerExpiredHandler}");
            // Initialize the TriggerState for this element
            IncrementAnIntegerPropertyTimerExpiredTriggerState=TriggerStates.Active;
            #endregion
            #endregion
            #endregion

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
            AnIntegerPropertyTextSpanStyle = StringConstants.NotMutating;
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

            // Start timers as the last step
            IncrementAnIntegerPropertyTimer.Enabled=true;
            IncrementAnIntegerPropertyTimer.Start();

            Logger.LogDebug($"Leaving Index.OnInitAsync");
        }

        // Don't worry about disconnecting the event handler from the event, as the page's dispose method will
        // ToDo: better understanding and write-up of how "navigating away from the page will eventually trigger a lifecycle event that removes the OnClick handler from the element"

        #endregion

        #region Event Handlers (this Demos's State programs)

        #region the input buttons elements OnClick Handlers
        #region IncrementAnIntegerPropertyTimerControlButton
        // Event Handler for the button when the trigger is Ignore
        public async Task IncrementAnIntegerPropertyTimerControlButtonOnClickTriggerIgnore() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyTimerControlButtonOnClickTriggerIgnore");
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyTimerControlButtonOnClickTriggerIgnore");
        }

        // Event Handler for the button when the trigger is Active
        public async Task IncrementAnIntegerPropertyTimerControlButtonOnClickTriggerActive() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyTimerControlButtonOnClickTriggerActive");
            // ToDo: Figure out what state triggers to modify
            // change the visual properties of the elements that are affected by this state program
            // IncrementAnIntegerPropertyTimerControlButton
            IncrementAnIntegerPropertyTimerControlButtonClass=$"\"btn btn-primary disabled\"";
            IncrementAnIntegerPropertyTimerControlButtonStyle="";
            IncrementAnIntegerPropertyTimerControlButtonText="Enqueue";
            Logger.LogDebug($"IncrementAnIntegerPropertyTimerControlButtonText = {IncrementAnIntegerPropertyButtonText};  class = {IncrementAnIntegerPropertyTimerControlButtonClass}; style = {IncrementAnIntegerPropertyTimerControlButtonStyle};");
            // IncrementAnIntegerPropertyTimer
            if (IncrementAnIntegerPropertyTimer.Enabled) {
                IncrementAnIntegerPropertyTimerStyle="Stopping";
                IncrementAnIntegerPropertyTimerText="Stopping";
            } else {
                IncrementAnIntegerPropertyTimerStyle="Starting";
                IncrementAnIntegerPropertyTimerText="Starting";
            }
            // Tell Blazor to re-render, when execution comes back to the GUI thread
            StateHasChanged();
            await Task.Run(async () => {
                // Toggle the Timer State
                if (IncrementAnIntegerPropertyTimer.Enabled) {
                    IncrementAnIntegerPropertyTimer.Stop();
                    IncrementAnIntegerPropertyTimer.Enabled=false;
                    // ToDo: is resetting to 0 a thing here? Should we have another state variable to hold a boolean? Populated with a default in OnInit from Config? Controlled by yet another State input element?
                } else {
                    IncrementAnIntegerPropertyTimer.Enabled=true;
                    IncrementAnIntegerPropertyTimer.Start();
                }
                // simulate a 1 millisecond duration in the action operation
                System.Threading.Thread.Sleep(1);
            });
            // On the ContinuationTask
            // change the visual properties of the elements (2) that are affected by this state program
            // IncrementAnIntegerPropertyTimerControlButton
            IncrementAnIntegerPropertyTimerControlButtonStyle="";
            if (IncrementAnIntegerPropertyTimer.Enabled) {
                // ToDo make a slight visual distinction to the element to discriminate the two state transition trigger 's visual attribute triggerstates
                // ToDo Add Aria tags to assist populations with insufficiant visual acuity to discriminate the different states visually 
                IncrementAnIntegerPropertyTimerControlButtonClass=$"\"btn btn-primary\"";
                IncrementAnIntegerPropertyTimerControlButtonText="Stop Timer";
            } else {
                IncrementAnIntegerPropertyTimerControlButtonClass=$"\"btn btn-primary\"";
                IncrementAnIntegerPropertyTimerControlButtonText="Start Timer";
            }
            Logger.LogDebug($"IncrementAnIntegerPropertyTimerControlButtonText = {IncrementAnIntegerPropertyButtonText};  class = {IncrementAnIntegerPropertyTimerControlButtonClass}; style = {IncrementAnIntegerPropertyTimerControlButtonStyle};");
            // IncrementAnIntegerPropertyTimer
            if (IncrementAnIntegerPropertyTimer.Enabled) {
                IncrementAnIntegerPropertyTimerStyle="Running";
                IncrementAnIntegerPropertyTimerText="Running";
            } else {
                IncrementAnIntegerPropertyTimerStyle="Stopped";
                IncrementAnIntegerPropertyTimerText="Stopped";
            }
            Logger.LogDebug($"IncrementAnIntegerPropertyTimerText = {IncrementAnIntegerPropertyTimerText};  class = {IncrementAnIntegerPropertyTimerClass}; style = {IncrementAnIntegerPropertyTimerStyle};");
            // ToDo: Figure out what state triggers to modify
            // Tell Blazor to re-render, when execution comes back to the GUI thread
            StateHasChanged();
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyTimerControlButtonOnClickTriggerActive");
        }


        // Event Handler for the button when the trigger is Enqueue
        public async Task IncrementAnIntegerPropertyTimerControlButtonOnClickTriggerEnqueue() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyTimerControlButtonOnClickTriggerEnqueue");
            // ToDo: Figure out where to store the enqueue counts
            await Task.Run(async () => {
                // simulate a 1 millisecond duration in the action operation
                System.Threading.Thread.Sleep(1);
            });
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyTimerControlButtonOnClickTriggerEnqueue");
        }

        #endregion

        #region the timer element Expired Handlers
        // Timer callback (Event handler) when the timer's state transition trigger is Active	
        // This is the state program the GUI runs when the timer fires.
        public async Task IncrementAnIntegerPropertyTimerExpiredTriggerActive() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyTimerExpiredTriggerActive");
            // The OnInit State program specified the timer would be created with AutoRefresh false, otherwise we would stop the timer
            // Set the IncrementAnIntegerPropertyTimerExpired's current event handler to the Enqueue handler
            try {
                // Start with All trigger handlers,
                IncrementAnIntegerPropertyTimerExpiredHandler=AllStateTransitionTriggerHandlers
                // use LINQ query to select just those triggerHandlers(s) that match the element's name, type, kind and having an Active TriggerState
                    .Where(triggerHandler => triggerHandler.ElementName=="IncrementAnIntegerProperty"
                        &&triggerHandler.ElementType=="Timer"
                        &&triggerHandler.TriggerKind==StateTriggerKinds.Expired&&
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
            Logger.LogDebug($"IncrementAnIntegerPropertyTimerExpiredHandler = {IncrementAnIntegerPropertyTimerExpiredHandler}");

            // Set the new TriggerState for this element
            IncrementAnIntegerPropertyTimerExpiredTriggerState=TriggerStates.Enqueue;

            // change the visual appearence of the mutating property
            AnIntegerPropertyTextSpanStyle=StringConstants.Mutating;

            // change the visual appearence of other elements on the page that are part of this program that expect to react to this state transition
            IncrementAnIntegerPropertyButtonClass="\"btn btn-primary disabled\"";
            IncrementAnIntegerPropertyButtonStyle="background-color:white;color:black;margin:0;";
            IncrementAnIntegerPropertyButtonText=$"click to enqueue. class = {IncrementAnIntegerPropertyButtonClass} style = {IncrementAnIntegerPropertyButtonStyle}";
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonText = {IncrementAnIntegerPropertyButtonText}");

            // ToDo: visually modify the IncrementAnIntegerPropertyTimerControlButton

            // ToDo: Disable the Property's OnNotifyPropertyChange Handler

            // Tell Blazor to re-render, when execution comes back to the GUI thread
            StateHasChanged();
            // Perform the action on the state Property and await it
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
            // The timer has no visual attribute, but the button that controls the timer does
            // ToDo: visually modify the IncrementAnIntegerPropertyTimerControlButton

            // Change the class, style, and text of the IncrementAnIntegerPropertyButton back to their active values
            IncrementAnIntegerPropertyButtonClass=$"\"btn btn-primary\"";
            IncrementAnIntegerPropertyButtonStyle="background-color:black;color:white;margin:0;";
            IncrementAnIntegerPropertyButtonText=$"click to increment class = {IncrementAnIntegerPropertyButtonClass} style = {IncrementAnIntegerPropertyButtonStyle}";
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonText = {IncrementAnIntegerPropertyButtonText}");

            // Update visual attributes of the mutating element to show it is no longer being modified
            AnIntegerPropertyTextSpanStyle=StringConstants.NotMutating;

            // Set the Timers's Expired Handler to Active
            // Set the IncrementAnIntegerPropertyTimerExpired's current event handler to the Active handler
            try {
                // Start with All trigger handlers,
                IncrementAnIntegerPropertyTimerExpiredHandler=AllStateTransitionTriggerHandlers
                // use LINQ query to select just those triggerHandlers(s) that match the element's name, type, kind and having an Active TriggerState
                    .Where(triggerHandler => triggerHandler.ElementName=="IncrementAnIntegerProperty"
                        &&triggerHandler.ElementType=="Timer"
                        &&triggerHandler.TriggerKind==StateTriggerKinds.Expired&&
                        triggerHandler.TriggerState==TriggerStates.Active)
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
            Logger.LogDebug($"IncrementAnIntegerPropertyTimerExpiredHandler = {IncrementAnIntegerPropertyTimerExpiredHandler}");
            // Set the new TriggerState for this element
            IncrementAnIntegerPropertyTimerExpiredTriggerState=TriggerStates.Active;

            // Restart the timer
            IncrementAnIntegerPropertyTimer.Start();

            // Tell Blazor to re-render when the GUI thread follows-up on the TaskContinuation
            StateHasChanged();
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyTimerActive");
            // The continuation task ends here
        }

        // Timer callback when the timer's state trigger is Ignore		
        public async Task IncrementAnIntegerPropertyTimerExpiredTriggerIgnore() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyTimerExpiredTriggerIgnore");
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyTimerExpiredTriggerIgnore");
        }

        // Timer callback when the timer's state trigger is Enqueue		
        public async Task IncrementAnIntegerPropertyTimerExpiredTriggerEnqueue() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyTimerExpiredTriggerEnqueue");
            // ToDo: decide where to store enqueued event counts, somewhere in State
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyTimerExpiredTriggerEnqueue");
        }
        #endregion

        #region IncrementAnIntegerPropertyButton
        // Event Handler for the button when the trigger is Ignore
        public async Task IncrementAnIntegerPropertyButtonOnClickTriggerIgnore() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyButtonOnClickTriggerIgnore");
            await Task.Run(async () => {
                // simulate a 1 millisecond duration in the action operation
                System.Threading.Thread.Sleep(1);
            });
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyButtonOnClickTriggerIgnore");
        }

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
            AnIntegerPropertyTextSpanStyle=StringConstants.Mutating;
            // Update visual attributes of the triggering element to show it has had a state transition pre-Action
            IncrementAnIntegerPropertyButtonClass=$"\"btn btn-primary disabled\"";
            IncrementAnIntegerPropertyButtonStyle="background-color:white;color:black;margin:0;";
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
            IncrementAnIntegerPropertyButtonStyle="background-color:black;color:white;margin:0;";
            IncrementAnIntegerPropertyButtonText=$"click to increment";
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonText = {IncrementAnIntegerPropertyButtonText};  class = {IncrementAnIntegerPropertyButtonClass}; style = {IncrementAnIntegerPropertyButtonStyle};");

            // Update visual attributes of the mutating element to show it is no longer being modified
            AnIntegerPropertyTextSpanStyle = StringConstants.NotMutating;

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

    // ToDo: Localize these strings
    public static class StringConstants {
        public const string Mutating = "background-color:orange;color:white;margin:0;";
        public const string NotMutating = "background-color:black;color:white;margin:0;";
        public const string IncrementAnIntegerPropertyTimerTimeoutInSeconds = "2";
        public const string StateProgramExceptionMessage = "The StateProgram is invalid, The number of state program methods for the ElementTypeState is not 1. Element = {Element}";
        public const string CannotParseLocalStorageForAnIntegerPropertyExceptionMessage = "The value returned from LocalStorage for AnIntegerProperty cannot be parsed to an int";
        public const string ThirdPartyLinkCautionMessage = "As always be cautious about clicking on links. These are not under our control, so make sure your anti-malware precautions are operational before following any of these third-party links.";
    }
}