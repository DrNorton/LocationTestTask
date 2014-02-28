using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LocationTestTask.DataLayer.Dto;

namespace LocationTestTask.Core
{
    public interface ILocationManager
    {
        event EventHandler<NewLocationEventArgs> OnNewPositionReceived;
        List<LocationDto> Locations { get; set; }
        void StartGeoReceiver();
        void StopGeoReceiver();
        bool IsStarted { get; set; }
    }
}