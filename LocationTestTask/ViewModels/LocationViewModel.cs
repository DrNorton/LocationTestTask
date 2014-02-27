using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Linq;
using System.Text;
using LocationTestTask.DataLayer.Dto;
using LocationTestTask.DataLayer.Repositories;
using LocationTestTask.UI.Views;
using Phone7.Fx.Commands;
using Phone7.Fx.Ioc;
using Phone7.Fx.Mvvm;
using Phone7.Fx.Navigation;

namespace LocationTestTask.UI.ViewModels
{
    [ViewModel(typeof(LocationView))]
    public class LocationViewModel:ViewModelBase
    {
        private readonly ILocationRepository _locationRepository;
        private readonly INavigationService _navigationService;
        private GeoCoordinateWatcher _geoCoordinateWatcher;
        private ObservableCollection<LocationDto> _locations; 

        private DelegateCommand<object> _startGeoWatcherCommand;
        private DelegateCommand<object> _stopGeoWatcherCommand;
        private DelegateCommand<object> _navigateToMapCommand;
            
        [Injection]
        public LocationViewModel(ILocationRepository locationRepository,INavigationService navigationService){
            _locationRepository = locationRepository;
            _navigationService = navigationService;
            InitWatcher();
        }

        public override void InitalizeData(){
            Locations=new ObservableCollection<LocationDto>(_locationRepository.GetAll().ToList());
        }

        private void InitWatcher(){
            _geoCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            _geoCoordinateWatcher.MovementThreshold = 100;
            _geoCoordinateWatcher.PositionChanged +=new System.EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(_geoCoordinateWatcher_PositionChanged);
        }

        private void _geoCoordinateWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e){
            var newLocation=ConvertToLocation(e);
            Locations.Add(newLocation);
            InsertToDatabase(newLocation);
        }

        private void InsertToDatabase(LocationDto location){
            _locationRepository.Insert(location);
        }

        private LocationDto ConvertToLocation(GeoPositionChangedEventArgs<GeoCoordinate> e){
            return new LocationDto(){MapPosition = new MapPositionDto(){Latitude = e.Position.Location.Latitude,Longitude = e.Position.Location.Longitude},
                MeasurementDatetime = ConvertFromDateTimeOffset(e.Position.Timestamp)};
        }

        #region Commands
        public DelegateCommand<object> StartGeoWatcherCommand{
            get{
                if (_startGeoWatcherCommand == null){
                    _startGeoWatcherCommand=new DelegateCommand<object>(StartGeoWatcher);
                }
                return _startGeoWatcherCommand;
            }
        }
        public DelegateCommand<object> StopGeoWatcherCommand
        {
            get
            {
                if (_stopGeoWatcherCommand == null)
                {
                    _stopGeoWatcherCommand = new DelegateCommand<object>(StopGeoWatcher);
                }
                return _stopGeoWatcherCommand;
            }
        }
        public DelegateCommand<object> NavigateToMapCommand
        {
            get
            {
                if (_navigateToMapCommand == null)
                {
                    _navigateToMapCommand = new DelegateCommand<object>(NavigateToMap);
                }
                return _navigateToMapCommand;
            }
        }

      

        #endregion



        private void StartGeoWatcher(object obj){
            _geoCoordinateWatcher.Start();
        }
        private void StopGeoWatcher(object obj){
            _geoCoordinateWatcher.Stop();
        }
        private void NavigateToMap(object obj)
        {
            _navigationService.UriFor<MapViewModel>().Navigate();
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

        public ObservableCollection<LocationDto> Locations
        {
            get { return _locations; }
            set{
                _locations = value;
                base.RaisePropertyChanged(()=>Locations);
            }
        }
    }
}
