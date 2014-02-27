using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Scheduler;
using System.Device.Location;
using Microsoft.Phone.Shell;

namespace LocationTestTask.GpsPositionSaverAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static volatile bool _classInitialized;
        public GeoCoordinateWatcher _geoCoordinateWatcher;

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                // Subscribe to the managed exception handler
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
                });
            }
        }

        /// Code to execute on Unhandled Exceptions
        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

      
        protected override void OnInvoke(ScheduledTask task)
        {
            _geoCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            var position=_geoCoordinateWatcher.Position;
            //Включим получатель координат
            UpdateTile();
            NotifyComplete();
        }

        private void UpdateTile()
        {
            ShellTile appTile = ShellTile.ActiveTiles.First();
            if (appTile != null)
            {
                StandardTileData tileData = new StandardTileData {BackContent = GetLastUpdatedTimeMessage()};
                appTile.Update(tileData);
            }
        }

        private string GetLastUpdatedTimeMessage()
        {
            return string.Format("Обновлено: {0}", DateTime.Now);
        }
    }
}