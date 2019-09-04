
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

        // Access the State instance registered in the DI container
        [Inject]
        public IState S { get; set; }

        [Inject]
        public ILoggerFactory LoggerFactory { get; set; }

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
        public string IncrementAnIntegerPropertyTimerSrc;
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

        #endregion

        #region Page Initialization Handler
        // This method is automagically called by the Blazor runtime as part of a page's lifecycle
        protected override async Task OnInitializedAsync() {
            Logger.LogDebug($"<Index.OnInitializedAsync");

            #region InitializeState
            // If the State object S in the DI container already has this page's state present, good to go, otherwise 
            // create this page's state and merge it into the DI state

            // Create a StateBuilder with the information for this page
            IStateBuilder sb = new StateBuilder(LoggerFactory, LStorage)
                .AddPage(new PageBuilder()
                    .AddPAID("index")
                    .AddElement(new ElementBuilder()
                        .AddNOID(new NOID("IncrementAnIntegerPropertyButton", ""))
                        .AddVisualAttribute("IncrementAnIntegerPropertyButtonStyle", StringConstants.PrimaryButtonStyleActive)
                        .AddVisualAttribute("IncrementAnIntegerPropertyButtonClass", StringConstants.PrimaryButtonClassActive)
                        .AddVisualAttribute("IncrementAnIntegerPropertyButtonText", StringConstants.PrimaryButtonTextActive)
                        .Build() // Element Build
                    ) // AddElement
                    .AddElement(new ElementBuilder()
                        .AddNOID(new NOID("AnIntegerProperty", ""))
                        .AddVisualAttribute("AnIntegerPropertyTextSpanStyle", StringConstants.NotMutating)
                        .Build() // Element Build
                    ) // AddElement
                    .Build() // Page Build
                    )  // AddPage
                .AddEventHandler(elementName: "IncrementAnIntegerPropertyTimer", elementType: "Timer", triggerKind: StateTriggerKinds.Expired, triggerState: TriggerStates.Ignore, methodToUse: IncrementAnIntegerPropertyTimerExpiredTriggerIgnore)
                .AddEventHandler(elementName: "IncrementAnIntegerPropertyTimer", elementType: "Timer", triggerKind: StateTriggerKinds.Expired, triggerState: TriggerStates.Active, methodToUse: IncrementAnIntegerPropertyTimerExpiredTriggerActive)
                .AddEventHandler(elementName: "IncrementAnIntegerPropertyTimer", elementType: "Timer", triggerKind: StateTriggerKinds.Expired, triggerState: TriggerStates.Enqueue, methodToUse: IncrementAnIntegerPropertyTimerExpiredTriggerEnqueue)
                .AddEventHandler(elementName: "IncrementAnIntegerPropertyButton", elementType: "Button", triggerKind: StateTriggerKinds.OnClick, triggerState: TriggerStates.Ignore, methodToUse: IncrementAnIntegerPropertyButtonOnClickTriggerIgnore)
                .AddEventHandler(elementName: "IncrementAnIntegerPropertyButton", elementType: "Button", triggerKind: StateTriggerKinds.OnClick, triggerState: TriggerStates.Active, methodToUse: IncrementAnIntegerPropertyButtonOnClickTriggerActive)
                .AddEventHandler(elementName: "IncrementAnIntegerPropertyButton", elementType: "Button", triggerKind: StateTriggerKinds.OnClick, triggerState: TriggerStates.Enqueue, methodToUse: IncrementAnIntegerPropertyButtonOnClickTriggerEnqueue)
            ;

            // If the State object S in the DI container already has this page's state present, good to go, otherwise 
            // merge this page's state into the DI state

            // Instantiate the State
            S=sb.Build(); // replace the State currently in the DI with a new one

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

            #region IncrementAnIntegerPropertyTimer
            // IncrementAnIntegerPropertyTimer HTML element 
            // assign values to the local properties corresponding to the element's HTML attributes
            // initialize local properties for the element's HTML attributes
            #region Visible attributes
            // assign values to the local properties corresponding to the element's visible HTML attributes
            IncrementAnIntegerPropertyTimerSrc="timer_running.gif";
            IncrementAnIntegerPropertyTimerStyle="Enabled";
            IncrementAnIntegerPropertyTimerClass="animated-gif";
            Logger.LogDebug($"IncrementAnIntegerPropertyTimerSrc = {IncrementAnIntegerPropertyTimerSrc};  class = {IncrementAnIntegerPropertyTimerClass}; style = {IncrementAnIntegerPropertyTimerStyle};");
            #endregion

            #region state transition attributes
            // assign values to the local properties for the element's Func<Task>()  HTML attributes (AKA event handler methods)
            // We will choose to use async versions of the event handlers so the methods return Task objects
            // initialize local properties for the elements state transition trigger for each of the element's Func<Task>()  HTML attributes
            #region Expired
            try {
                IncrementAnIntegerPropertyTimerExpiredHandler=S.FromStateHandlers("IncrementAnIntegerPropertyTimer", "Timer", StateTriggerKinds.Expired, TriggerStates.Active);
            }
            catch (Exception e) {
                Logger.LogError(string.Format(StringConstants.StateProgramExceptionMessage, "IncrementAnIntegerPropertyTimerExpiredActiveHandler"));
                // ToDo: Create a new kind of exception and then throw it upwards
            }
            //var x = IncrementAnIntegerPropertyTimerExpiredHandlerCurrent.GetMemberName<Func<Task>>(IncrementAnIntegerPropertyTimerExpiredHandlerCurrent);
            Logger.LogDebug($"IncrementAnIntegerPropertyTimerExpiredHandler = IncrementAnIntegerPropertyTimerExpiredActiveHandler");
            // Initialize the TriggerState for this element
            IncrementAnIntegerPropertyTimerExpiredTriggerState=TriggerStates.Active;
            #endregion
            #endregion
            #endregion

            #region IncrementAnIntegerPropertyButton
            // IncrementAnIntegerPropertyButton HTML element 
            // assign values to the local properties corresponding to the element's HTML attributes
            #region Visible attributes
            // assign values to the local properties corresponding to the element's visible HTML attributes
            S.P.IncrementAnIntegerPropertyButtonClass=StringConstants.PrimaryButtonClassActive;
            S.P.IncrementAnIntegerPropertyButtonStyle=StringConstants.PrimaryButtonStyleActive;
            S.P.IncrementAnIntegerPropertyButtonText=StringConstants.PrimaryButtonTextActive;
            Logger.LogDebug($"S.P.IncrementAnIntegerPropertyButtonText = {S.P.IncrementAnIntegerPropertyButtonText};  class = {S.P.IncrementAnIntegerPropertyButtonClass}; style = {S.P.IncrementAnIntegerPropertyButtonStyle};");
            #endregion

            #region state transition attributes
            // Initialize local properties for the element's Func<Task>()  HTML attributes (AKA event handler methods)
            // We will choose to use async versions of the event handlers so the methods return Task objects
            // initialize local properties for the elements state transition trigger for each of the element's Func<Task>()  HTML attributes
            #region OnClick
            try {
                IncrementAnIntegerPropertyButtonOnClickHandler=S.FromStateHandlers("IncrementAnIntegerPropertyButton", "Button", StateTriggerKinds.OnClick, TriggerStates.Active);
            }
            catch (Exception e) {
                Logger.LogError(string.Format(StringConstants.StateProgramExceptionMessage, "IncrementAnIntegerPropertyButtonOnClickActiveHandler"));
                // ToDo: throw it upwards
            }
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonOnClickHandler = {IncrementAnIntegerPropertyButtonOnClickHandler} (IncrementAnIntegerPropertyButtonOnClickActiveHandler)");
            // Initialize the TriggerState for this element
            IncrementAnIntegerPropertyButtonOnClickTriggerState = TriggerStates.Active;
            #endregion
            #endregion
            #endregion

            #region AnIntegerProperty
            // AnIntegerProperty HTML element 
            // assign initial values to the local properties corresponding to the element's HTML attributes
            #region Visible attributes
            // assign initial values to the local properties corresponding to the element's visible HTML attributes
            // AnIntegerPropertyTextSpan
            S.P.AnIntegerPropertyTextSpanStyle = StringConstants.NotMutating;
            Logger.LogDebug($"S.P.AnIntegerPropertyTextSpanStyle = {S.P.AnIntegerPropertyTextSpanStyle}");
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

            // to make this method into an async
            await Task.Delay(1);

            Logger.LogDebug($"Leaving Index.OnInitializedAsync");
        }

        // Don't worry about disconnecting the event handler from the event, as the page's dispose method will
        // ToDo: better understanding and write-up of how "navigating away from the page will eventually trigger a lifecycle event that removes the OnClick handler from the element"

        #endregion

        #region Event Handlers (this Demos's State programs)

        #region the timer element Expired Handlers
        // Timer callback (Event handler) when the timer's state transition trigger is Active	
        // This is the state program the GUI runs when the timer fires.
        public async Task IncrementAnIntegerPropertyTimerExpiredTriggerActive() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyTimerExpiredTriggerActive");
            // The OnInit State program specified the timer would be created with AutoRefresh false, otherwise we would stop the timer
            // Set the IncrementAnIntegerPropertyTimerExpired's current event handler to the Enqueue handler
            try {
                IncrementAnIntegerPropertyTimerExpiredHandler=S.FromStateHandlers("IncrementAnIntegerPropertyTimer", "Timer", StateTriggerKinds.Expired, TriggerStates.Enqueue);
            }
            catch (Exception e) {
                Logger.LogError(string.Format(StringConstants.StateProgramExceptionMessage, "IncrementAnIntegerPropertyTimerExpiredTriggerEnqueue"));
                // ToDo: Create a new kind of exception and then throw it upwards
            }
            Logger.LogDebug($"IncrementAnIntegerPropertyTimerExpiredHandler = IncrementAnIntegerPropertyTimerExpiredTriggerEnqueue");

            // Set the new TriggerState for this element
            IncrementAnIntegerPropertyTimerExpiredTriggerState=TriggerStates.Enqueue;

            // change the visual appearance of the mutating property
            S.P.AnIntegerPropertyTextSpanStyle = StringConstants.Mutating;

            // change the visual appearance of other elements on the page that are part of this program that expect to react to this state transition
            //S.P.IncrementAnIntegerPropertyButtonClass=StringConstants.PrimaryButtonClassEnqueue;
            //S.P.IncrementAnIntegerPropertyButtonStyle=StringConstants.PrimaryButtonStyleEnqueue;
            //S.P.IncrementAnIntegerPropertyButtonText=StringConstants.PrimaryButtonTextEnqueue;
            //Logger.LogDebug($"S.P.IncrementAnIntegerPropertyButtonText = {S.P.IncrementAnIntegerPropertyButtonText};  class = {S.P.IncrementAnIntegerPropertyButtonClass}; style = {S.P.IncrementAnIntegerPropertyButtonStyle};");

            // ToDo: visually modify the IncrementAnIntegerPropertyTimerControlButton

            // ToDo: Disable the Property's OnNotifyPropertyChange Handler

            // Tell Blazor to re-render, when execution comes back to the GUI thread
            await InvokeAsync(() => StateHasChanged());
            // Perform the action on the state Property and await it
            await Task.Run(async () => {
                // ensure this async method includes an await (eliminate Warning CS1998)
                // simulate a 1 second delay in the processing
                await Task.Delay(1000);
                AnIntegerProperty+=1;
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
            //S.P.IncrementAnIntegerPropertyButtonClass=StringConstants.PrimaryButtonClassActive;
            //S.P.IncrementAnIntegerPropertyButtonStyle=StringConstants.PrimaryButtonStyleActive;
            //S.P.IncrementAnIntegerPropertyButtonText=StringConstants.PrimaryButtonTextActive;
            //Logger.LogDebug($"S.P.IncrementAnIntegerPropertyButtonText = {S.P.IncrementAnIntegerPropertyButtonText};  class = {S.P.IncrementAnIntegerPropertyButtonClass}; style = {S.P.IncrementAnIntegerPropertyButtonStyle};");

            // Update visual attributes of the mutating element to show it is no longer being modified
            S.P.AnIntegerPropertyTextSpanStyle= StringConstants.NotMutating;

            // Set the Timers's Expired Handler to Active
            // Set the IncrementAnIntegerPropertyTimerExpired's current event handler to the Active handler
            try {
                IncrementAnIntegerPropertyTimerExpiredHandler=S.FromStateHandlers("IncrementAnIntegerPropertyTimer", "Timer", StateTriggerKinds.Expired, TriggerStates.Active);
            }
            catch (Exception e) {
                Logger.LogError(string.Format(StringConstants.StateProgramExceptionMessage, "IncrementAnIntegerPropertyTimerActiveHandler"));
                // ToDo: Create a new kind of exception and then throw it upwards
            }
            Logger.LogDebug($"IncrementAnIntegerPropertyTimerExpiredHandler = IncrementAnIntegerPropertyTimerActiveHandler");
            // Set the new TriggerState for this element
            IncrementAnIntegerPropertyTimerExpiredTriggerState=TriggerStates.Active;

            // Restart the timer
            IncrementAnIntegerPropertyTimer.Start();

            // Tell Blazor to re-render when the GUI thread follows-up on the TaskContinuation
            await InvokeAsync(() => StateHasChanged());
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyTimerActive");
            // The continuation task ends here
        }

        // Timer callback when the timer's state trigger is Ignore		
        public async Task IncrementAnIntegerPropertyTimerExpiredTriggerIgnore() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyTimerExpiredTriggerIgnore");
            await Task.Delay(1);
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyTimerExpiredTriggerIgnore");
        }

        // Timer callback when the timer's state trigger is Enqueue		
        public async Task IncrementAnIntegerPropertyTimerExpiredTriggerEnqueue() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyTimerExpiredTriggerEnqueue");
            // ToDo: decide where to store enqueued event counts, somewhere in State
            await Task.Delay(1);
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyTimerExpiredTriggerEnqueue");
        }
        #endregion
        #endregion

        #region the input buttons elements OnClick Handlers
        #region IncrementAnIntegerPropertyButton
        // Event Handler for the button when the trigger is Ignore
        public async Task IncrementAnIntegerPropertyButtonOnClickTriggerIgnore() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyButtonOnClickTriggerIgnore");
            await Task.Delay(1);
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyButtonOnClickTriggerIgnore");
        }

        // State program for (AKA Event Handler) for IncrementAnIntegerProperty button when the trigger is Active
        public async Task IncrementAnIntegerPropertyButtonOnClickTriggerActive() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyButtonOnClickTriggerActive");
            // Set the IncrementAnIntegerPropertyButtonOnClick's current event handler to the Enqueue handler
            try {
                IncrementAnIntegerPropertyButtonOnClickHandler=S.FromStateHandlers("IncrementAnIntegerPropertyButton", "Button", StateTriggerKinds.OnClick, TriggerStates.Enqueue);
            }
            catch (Exception e) {
                Logger.LogError(string.Format(StringConstants.StateProgramExceptionMessage, "IncrementAnIntegerPropertyButtonOnClickEnqueueHandler"));
                // ToDo: throw it upwards
            }
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonOnClickHandler = {IncrementAnIntegerPropertyButtonOnClickHandler} (IncrementAnIntegerPropertyButtonOnClickEnqueueHandler)");
            // Set the new TriggerState for this element
            IncrementAnIntegerPropertyButtonOnClickTriggerState=TriggerStates.Enqueue;

            // ToDo: store the mutating Property's OnNotifyPropertyChange StateTriggerState
            // ToDo: set the mutating Property's OnNotifyPropertyChange StateTriggerState to Enqueue
            // ToDo: set the mutating Property's OnNotifyPropertyChange Handler to its Enqueue method

            // Modify the visual attributes of all elements affected by this state program
            // Update visual attributes of the mutating element to show it is being modified
            S.P.AnIntegerPropertyTextSpanStyle=StringConstants.Mutating;
            // Update visual attributes of the triggering element to show it has had a state transition pre-Action
            S.P.IncrementAnIntegerPropertyButtonClass=StringConstants.PrimaryButtonClassEnqueue;
            S.P.IncrementAnIntegerPropertyButtonStyle=StringConstants.PrimaryButtonStyleEnqueue;
            S.P.IncrementAnIntegerPropertyButtonText=StringConstants.PrimaryButtonTextEnqueue;
            Logger.LogDebug($"S.P.IncrementAnIntegerPropertyButtonText = {S.P.IncrementAnIntegerPropertyButtonText};  class = {S.P.IncrementAnIntegerPropertyButtonClass}; style = {S.P.IncrementAnIntegerPropertyButtonStyle};");

            // Tell Blazor to re-render, when execution comes back to the GUI thread
            await InvokeAsync(() => StateHasChanged());
            // Execution will come back to the calling (usually the main GUI) thread as soon as the following await is hit (unless it completes immediately? is that a thing to worry about?)
            // Perform the action on the state Property and await it
            // For the demo, an async lambda performs the action, which runs a Task and returns that Task to the event handler right away
            await Task.Run(async () => {
                // simulate a 2 second duration in the action operation
                await Task.Delay(2000);
                AnIntegerProperty+=1;
            });
            // Code from here until the end of the event handler is put into a TaskContinuation and automagically run after the previous Task.Run completes (after the Action that modifies the AnIntegerProperty completes)
            // Code from here to the end is not executed until later, so the event handler effectively returns to the GUI thread right here, while awaiting the async lambda
            // This continuationTask will execute on (an unknown? the main GUI?) thread after the async Action completes
            // ToDo: ensure this TaskContinuation is run on a non-GUI thread (?)
            // Modify the visual attributes of all elements affected by this state program
            // Update visual attributes of the triggering element to show it has had a state transition post-Action
            S.P.IncrementAnIntegerPropertyButtonClass=StringConstants.PrimaryButtonClassActive;
            S.P.IncrementAnIntegerPropertyButtonStyle=StringConstants.PrimaryButtonStyleActive;
            S.P.IncrementAnIntegerPropertyButtonText=StringConstants.PrimaryButtonTextActive;
            Logger.LogDebug($"S.P.IncrementAnIntegerPropertyButtonText = {S.P.IncrementAnIntegerPropertyButtonText};  class = {S.P.IncrementAnIntegerPropertyButtonClass}; style = {S.P.IncrementAnIntegerPropertyButtonStyle};");

            // Update visual attributes of the mutating element to show it is no longer being modified
            S.P.AnIntegerPropertyTextSpanStyle=StringConstants.NotMutating;

            // ToDo: retrieve the mutating Property's OnNotifyPropertyChange StateTriggerState
            // ToDo: set the mutating Property's OnNotifyPropertyChange StateTriggerState to the retrieved value
            // ToDo: set the mutating Property's OnNotifyPropertyChange Handler to its StateTriggerState retrieved value method

            // Set the IncrementAnIntegerPropertyButtonOnClick's current event handler to the Active handler
            try {
                IncrementAnIntegerPropertyButtonOnClickHandler=S.FromStateHandlers("IncrementAnIntegerPropertyButton", "Button", StateTriggerKinds.OnClick, TriggerStates.Active);
            }
            catch (Exception e) {
                Logger.LogError(string.Format(StringConstants.StateProgramExceptionMessage, "IncrementAnIntegerPropertyButtonOnClickActiveHandler"));
                // ToDo: throw it upwards
            }
            Logger.LogDebug($"IncrementAnIntegerPropertyButtonOnClickHandler = {IncrementAnIntegerPropertyButtonOnClickHandler} (IncrementAnIntegerPropertyButtonOnClickActiveHandler)");
            // Change the IncrementAnIntegerPropertyButtonOnClick trigger state
            IncrementAnIntegerPropertyButtonOnClickTriggerState=TriggerStates.Active;

            // Tell Blazor to re-render when the GUI thread follows-up on the TaskContinuation
            await InvokeAsync(() => StateHasChanged());
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyButtonOnClickTriggerActive");
            // The continuation task ends here, and ToDo: execution goes where?
        }

        // Event Handler for the button when the trigger is Enqueue
        public async Task IncrementAnIntegerPropertyButtonOnClickTriggerEnqueue() {
            Logger.LogDebug("Starting IncrementAnIntegerPropertyButtonOnClickTriggerEnqueue");
            // ToDo: record the event in a non-visual State property
            await Task.Delay(1);
            Logger.LogDebug("Leaving IncrementAnIntegerPropertyButtonOnClickTriggerEnqueue");
        }
        #endregion
        #endregion


    }

    // ToDo: Localize these strings
    public static class StringConstants {
        public const string PrimaryButtonClassActive = "\"btn btn-primary\"";
        public const string PrimaryButtonStyleActive = "background-color:black;color:white;margin:0;";
        public const string PrimaryButtonTextActive = "click to increment";
        public const string PrimaryButtonClassEnqueue = "\"btn btn-primary disabled\"";
        public const string PrimaryButtonStyleEnqueue = "background-color:white;color:black;margin:0;";
        public const string PrimaryButtonTextEnqueue = "click to enqueue";
        public const string Mutating = "background-color:orange;color:white;margin:0;";
        public const string NotMutating = "background-color:black;color:white;margin:0;";
        public const string IncrementAnIntegerPropertyTimerTimeoutInSeconds = "2";
        public const string StateProgramExceptionMessage = "The StateProgram is invalid, The number of state program methods for the ElementTypeState is not 1. Element = {0}";
        public const string CannotParseLocalStorageForAnIntegerPropertyExceptionMessage = "The value returned from LocalStorage for AnIntegerProperty cannot be parsed to an int";
        public const string ThirdPartyLinkCautionMessage = "As always be cautious about clicking on links. These are not under our control, so make sure your anti-malware precautions are operational before following any of these third-party links.";
    }
}
