using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LocationTestTask.DataLayer.Dto;
using LocationTestTask.DataLayer.Repositories;
using Phone7.Fx.Ioc;
using System.Device.Location;

namespace LocationTestTask.Core
{
    public class LocationManager : ILocationManager
    {
        private readonly ILocationRepository _locationRepository;
        private GeoCoordinateWatcher _geoCoordinateWatcher;
        public event EventHandler<NewLocationEventArgs> OnNewPositionReceived;

        public bool IsStarted { get; set; }

        public List<LocationDto> Locations { get; set; }

        [Injection]
        public LocationManager(ILocationRepository locationRepository){
            _locationRepository = locationRepository;
            _geoCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            _geoCoordinateWatcher.MovementThreshold = 100;
            _geoCoordinateWatcher.PositionChanged += _geoCoordinateWatcher_PositionChanged;
            LoadPrevioslyLocations();
        }

        private void LoadPrevioslyLocations(){
            Locations = _locationRepository.GetAll().ToList();
        }


        private void _geoCoordinateWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var newLocation = ConvertToLocation(e);
            Locations.Add(newLocation);
            InsertToDatabase(newLocation);
            if (OnNewPositionReceived != null){
                OnNewPositionReceived(this,new NewLocationEventArgs(){NewPosition = newLocation});
            }
        }

        private LocationDto ConvertToLocation(GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            return new LocationDto()
            {
                MapPosition = new MapPositionDto() { Latitude = e.Position.Location.Latitude, Longitude = e.Position.Location.Longitude },
                MeasurementDatetime = ConvertFromDateTimeOffset(e.Position.Timestamp)
            };
        }
        private void InsertToDatabase(LocationDto location)
        {
            _locationRepository.Insert(location);
        }
        private DateTime ConvertFromDateTimeOffset(DateTimeOffset dateTime)
        {
            if (dateTime.Offset.Equals(TimeSpan.Zero))
                return dateTime.UtcDateTime;
            else if (dateTime.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(dateTime.DateTime)))
                return DateTime.SpecifyKind(dateTime.DateTime, DateTimeKind.Local);
            else
                return dateTime.DateTime;
        }

        public void StartGeoReceiver(){
            _geoCoordinateWatcher.Start();
            IsStarted = true;
        }

        public void StopGeoReceiver(){
            _geoCoordinateWatcher.Stop();
            IsStarted = false;
        }
    }
}
