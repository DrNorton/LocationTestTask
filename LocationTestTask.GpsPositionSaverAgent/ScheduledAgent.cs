using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Scheduler;
using System.Device.Location;
using Microsoft.Phone.Shell;

namespace LocationTestTask.GpsPositionSaverAgent
{
    public class ScheduledGpsAgent : ScheduledTaskAgent
    {
        private static volatile bool _classInitialized;
        public GeoCoordinateWatcher _geoCoordinateWatcher;

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledGpsAgent()
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
            //TODO: Add code to perform your task in background
            _geoCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            _geoCoordinateWatcher.MovementThreshold = 100;
            _geoCoordinateWatcher.PositionChanged += new System.EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(_geoCoordinateWatcher_PositionChanged);
            //TODO: добавьте код для выполнения задачи в фоновом режиме
            //Включим получатель координат
            _geoCoordinateWatcher.Start();
            NotifyComplete();
        }

        void _geoCoordinateWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
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