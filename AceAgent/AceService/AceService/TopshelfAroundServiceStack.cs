using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;
using ServiceStack.Logging;
using ServiceStack;
using ServiceStack.Configuration;

namespace Ace.AceService
{
    class TopShelfAroundServiceStackWrapper : ServiceControl
    {
        static ILog Log = LogManager.GetLogger(typeof(TopShelfAroundServiceStackWrapper));
        const string SelfUpdatingServiceDistributionLocation = @"C:\Users\whertzing\Source\Repos\SelfUpdatingService\Releases";
        private AppHost appHost;
        //AutoUpdateData autoUpdateData;
        System.Timers.Timer autoUpdateCheckTimer;

        public TopShelfAroundServiceStackWrapper()
        {
            Log.Debug("Starting TopShelfAroundServiceStackWrapper Ctor");

            #region create the AutoUpdate data structure
            //ToDo Implement Squirrel or equivalent to AutoUpdate a service
            //Log.Debug("In TopShelfAroundServiceStackWrapper Ctor, creating autoUpdateData");
            //autoUpdateData = new AutoUpdateData { CurrentVersion = "0.0.0", NextVersion = "0,0,0", ShowTheWelcomeWizard = false, SelfUpdatingServiceDistributionLocation = SelfUpdatingServiceDistributionLocation };
            #endregion create the AutoUpdate data structure

            #region create autoUpdateCheckTimer, connect callback
            Log.Debug("In TopShelfAroundServiceStackWrapper Ctor, creating autoUpdateCheckTimer");
            autoUpdateCheckTimer = new System.Timers.Timer(1000)            {                AutoReset = true            };
            autoUpdateCheckTimer.Elapsed += new ElapsedEventHandler(AutoUpdateCheckTimer_Elapsed);
            #endregion create autoUpdateCheckTimer, connect callback, and store in container's timers

            // Create the Ace.AceService Host
            Log.Debug("In TopShelfAroundServiceStackWrapper Ctor, creating appHost");
            appHost = new AppHost();
            Log.Debug("Leaving TopShelfAroundServiceStackWrapper Ctor");
        }
        public bool Start(HostControl hostControl)
        {
            Log.Debug("Starting TopShelfAroundServiceStackWrapper Start Method");
            try
            {
                Log.Debug(" In TopShelfAroundServiceStackWrapper Start Method calling appHost.Init");
                appHost.Init();
                string listeningOn = appHost.AppSettings.GetString("Ace.AceService:ListeningOn");
                Log.Debug($"In TopShelfAroundServiceStackWrapper Start Method calling appHost.Start, listeningOn {listeningOn}");
                appHost.Start(listeningOn);
                Log.Debug("In TopShelfAroundServiceStackWrapper Start Method calling Process.Start");
                Process.Start(listeningOn);
            }
            catch (Exception ex)
            {
                
                Log.Error(ex, "exception occurred in TopShelfAroundServiceStackWrapper Start Method");
                throw ex;
            }
            //
            autoUpdateCheckTimer.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            Log.Debug("Starting TopShelfAroundServiceStackWrapper Stop Method");
            autoUpdateCheckTimer.Stop();
            // Dispose of the AutoUpdate data and task
            try
            {
                Log.Debug("In TopShelfAroundServiceStackWrapper.Stop method, trying to resolve any outstanding autoupdate task");
            }
            catch (Exception ex)
            {
                Log.Debug($"In TopShelfAroundServiceStackWrapper.Stop method, resolving any outstanding autoupdate task threw exception {ex.Message}");
                throw;
            }
            // Cancel the updateTask if it exists and is running.
            // if (updateTask != null) {// ToDo: Cancel the updateTask. }

            //if autoUpdateData.UpdateManager is not null, then dispose of it
            //autoUpdateData.UpdateManager?.Dispose();


            // Stop the ServiceStack service
            Log.Debug("In TopShelfAroundServiceStackWrapper Stop Method calling appHost.Stop");
            this.appHost.Stop();
            Log.Debug("Leaving TopShelfAroundServiceStackWrapper Stop Method");
            return true;
        }
        public bool AfterStartingService(HostControl hostControl)
        {
            Log.Debug("Starting TopShelfAroundServiceStackWrapper AfterStartingService Method");
            // updater?.Start();;
            Log.Debug("Leaving TopShelfAroundServiceStackWrapper AfterStartingService Method");
            return true;
        }
        public bool BeforeStoppingService(HostControl hostControl)
        {
            Log.Debug("Starting TopShelfAroundServiceStackWrapper BeforeStoppingService Method");
            // updater?.Cancel();
            Log.Debug("Leaving TopShelfAroundServiceStackWrapper BeforeStoppingService Method");
            return true;
        }
        public bool WhenStopped(HostControl hostControl)
        {
            Log.Debug("Entering TopShelfAroundServiceStackWrapper WhenStopped Method");
            // updater?.Cancel();
            Log.Debug("Leaving TopShelfAroundServiceStackWrapper WhenStopped Method");
            return true;
        }


        void AutoUpdateCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Log.Debug("Entering the TopShelfAroundServiceStackWrapper.AutoUpdateCheckTimer_Elapsed Method");
            autoUpdateCheckTimer.Stop();
            // first time through this, create an instance of the update Manager, and if it fails, raise an event
            /*
            if (autoUpdateData.UpdateManager == null)
            {
                try
                {
                    autoUpdateData.UpdateManager = new UpdateManager(autoUpdateData.SelfUpdatingServiceDistributionLocation);
                }
                catch (Exception ex)
                {
                    Log.Fatal("exception occurred in TopShelfAroundServiceStackWrapper.AutoUpdateCheckTimer_Elapsed while creating the UpdateManager", ex);
                    throw ex;
                }
            }
            */
            // if not the first time through this loop, see if an update task is already created
            // if there is a task do nothing, else create one
            /*
            if (autoUpdateData.updaterTask == null)            {
                Log.Debug("updaterTask is null, creating new");
                autoUpdateData.updaterTask = autoUpdateData.UpdateManager.UpdateApp();
            } else {
                Log.Debug("updaterTask task exists, deal with completion, canceled, and faulted");
                switch (autoUpdateData.updaterTask.Status)
                {
                    case TaskStatus.Created:
                        break;
                    case TaskStatus.WaitingForActivation:
                        break;
                    case TaskStatus.WaitingToRun:
                        break;
                    case TaskStatus.Running:
                        break;
                    case TaskStatus.WaitingForChildrenToComplete:
                        break;
                    case TaskStatus.RanToCompletion:
                        break;
                    case TaskStatus.Canceled:
                        break;
                    case TaskStatus.Faulted:
                        break;
                    default:
                        break;
                }

        }
                */
            autoUpdateCheckTimer.Start();
        }
    }

}
