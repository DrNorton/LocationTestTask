using System;
using System.Collections.ObjectModel;
using System.Linq;
using LocationTestTask.Core;
using LocationTestTask.DataLayer.Dto;
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
        private readonly INavigationService _navigationService;
        private readonly ILocationManager _locationManager;

        public new DelegateCommand<object> StartGeoWatcherCommand { get; set; }
        public new DelegateCommand<object> StopGeoWatcherCommand { get; set; }

        private DelegateCommand<object> _navigateToMapCommand;
        
            
            
        [Injection]
        public LocationViewModel(INavigationService navigationService,ILocationManager locationManager){
       
            _navigationService = navigationService;
            _locationManager = locationManager;
            
            _locationManager.OnNewPositionReceived += new EventHandler<NewLocationEventArgs>(_locationManager_OnNewPositionReceived);
            StopGeoWatcherCommand = new DelegateCommand<object>(StopGeoWatcher, (obj) => _locationManager.IsStarted);
            StartGeoWatcherCommand = new DelegateCommand<object>(StartGeoWatcher, (obj) => !_locationManager.IsStarted);
          
        }


        void _locationManager_OnNewPositionReceived(object sender, NewLocationEventArgs e)
        {
            base.RaisePropertyChanged(() => Locations);
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
      

        private void StartGeoWatcher(object obj){
            _locationManager.StartGeoReceiver();
            RaiseStartStopButtonCanExecute();
        }

        private void RaiseStartStopButtonCanExecute(){
            StartGeoWatcherCommand.RaiseCanExecuteChanged();
            StopGeoWatcherCommand.RaiseCanExecuteChanged();
        }

        private void StopGeoWatcher(object obj){
            _locationManager.StopGeoReceiver();
            RaiseStartStopButtonCanExecute();
        }
        private void NavigateToMap(object obj)
        {
            _navigationService.UriFor<MapViewModel>().Navigate();
        }

        public ObservableCollection<LocationDto> Locations{
            get{
                return new ObservableCollection<LocationDto>(_locationManager.Locations.OrderByDescending(x=>x.MeasurementDatetime));
            }
        }
    }
}
