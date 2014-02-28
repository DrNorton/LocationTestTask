using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
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
using LocationTestTask.Core;
using LocationTestTask.DataLayer.Dto;
using LocationTestTask.DataLayer.Repositories;
using LocationTestTask.UI.Views;
using Microsoft.Phone.Controls.Maps;
using Phone7.Fx.Commands;
using Phone7.Fx.Extensions;
using Phone7.Fx.Ioc;
using Phone7.Fx.Mvvm;

namespace LocationTestTask.UI.ViewModels
{
     [ViewModel(typeof(MapView))]
    public class MapViewModel:ViewModelBase
    {
         private readonly ILocationManager _locationManager;
         private readonly ApplicationIdCredentialsProvider _credentials;

         private int _zoomLevel=5;
         private DelegateCommand<object> _zoomInCommand;
         private DelegateCommand<object> _zoomOutCommand;
         private DelegateCommand<LocationDto> _zoomSelectPinCommand;
         private ObservableCollection<LocationDto> _userPositions;
         private LocationDto _mapCenter;

             
             
         [Injection]
         public MapViewModel(ILocationManager locationManager,ApplicationIdCredentialsProvider credentials){
             _locationManager = locationManager;
             _credentials = credentials;
             _locationManager.OnNewPositionReceived += new EventHandler<NewLocationEventArgs>(_locationManager_OnNewPositionReceived);
             _userPositions=new ObservableCollection<LocationDto>();
             _mapCenter=new LocationDto();
         }

         void _locationManager_OnNewPositionReceived(object sender, NewLocationEventArgs e)
         {
             base.RaisePropertyChanged(()=>UserPositions);
         }

         public override void InitalizeData(){
             GetLastPositionAndCenterMap();
         }

         private void GetLastPositionAndCenterMap()
         {
             var lastPosition = UserPositions.FirstOrDefault(x => x.MeasurementDatetime == UserPositions.Max(y => y.MeasurementDatetime));
             if (lastPosition != null){
                 MapCenter = lastPosition;
             }
         }

         private void ZoomSelectedPin(LocationDto obj){
             MapCenter = obj;
             ZoomLevel = 18;
         }

         public int ZoomLevel{
             get { return _zoomLevel; }
             set{
                 if (value >= 0){
                     _zoomLevel = value;
                 }
          
                 base.RaisePropertyChanged(()=>ZoomLevel);
             }
         }

         public DelegateCommand<object> ZoomInCommand{
             get{
                 if (_zoomInCommand == null){
                     _zoomInCommand=new DelegateCommand<object>((obj)=>ZoomLevel++);
                 }
                 return _zoomInCommand;
             }
         }

         public DelegateCommand<object> ZoomOutCommand
         {
             get
             {
                 if (_zoomOutCommand == null)
                 {
                     _zoomOutCommand = new DelegateCommand<object>((obj) => ZoomLevel--);
                 }
                 return _zoomOutCommand;
             }
         }

         public DelegateCommand<LocationDto> ZoomSelectedPinCommand
         {
             get
             {
                 if (_zoomSelectPinCommand == null)
                 {
                     _zoomSelectPinCommand = new DelegateCommand<LocationDto>(ZoomSelectedPin);
                 }
                 return _zoomSelectPinCommand;
             }
         }

         public ObservableCollection<LocationDto> UserPositions{
             get { return new ObservableCollection<LocationDto>(_locationManager.Locations); }
           
         }

         public LocationDto MapCenter{
             get { return _mapCenter; }
             set{
                 _mapCenter = value;
                 base.RaisePropertyChanged(()=>MapCenter);
             }
         }

         public ApplicationIdCredentialsProvider Credentials{
             get { return _credentials; }
         }
    }
}
