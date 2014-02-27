using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;

namespace LocationTestTask.UI.Views
{
    public partial class LocationView : PhoneApplicationPage
    {
        private const string PeriodicTaskName = "dadad";

        // Constructor
        public LocationView(){
            InitializeComponent();

            //// Obtain a reference to the period task, if one exists
            //PeriodicTask periodicTask = ScheduledActionService.Find(PeriodicTaskName) as PeriodicTask;

            //if (periodicTask != null){
            //    RemoveAgent(PeriodicTaskName);
            //}

            //periodicTask = new PeriodicTask(PeriodicTaskName);

            //// The description is required for periodic agents. This is the string that the user
            //// will see in the background services Settings page on the device.
            //periodicTask.Description = "This demonstrates a periodic task.";

            //// Place the call to Add in a try block in case the user has disabled agents.
            //try{
            //    ScheduledActionService.Add(periodicTask);
             
            //    // If debugging is enabled, use LaunchForTest to launch the agent in one minute.

            //    ScheduledActionService.LaunchForTest(PeriodicTaskName, TimeSpan.FromSeconds(1));

            //}
            //catch (InvalidOperationException exception){
            //    if (exception.Message.Contains("BNS Error: The action is disabled")){
            //        MessageBox.Show("Background agents for this application have been disabled by the user.");


            //    }

            //    if (
            //        exception.Message.Contains(
            //            "BNS Error: The maximum number of ScheduledActions of this type have already been added.")){
            //        // No user action required. The system prompts the user when the hard limit of periodic tasks has been reached.

            //    }

            //}
            //catch (SchedulerServiceException){
            //    // No user action r
            //}
        }

        private void RemoveAgent(string name)
        {
            try
            {
                ScheduledActionService.Remove(name);
            }
            catch (Exception)
            {
            }
        }

    }
}