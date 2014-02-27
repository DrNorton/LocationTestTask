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
using LocationTestTask.DataLayer.Dto;
using LocationTestTask.DataLayer.Repositories;
using LocationTestTask.UI.Views;
using Phone7.Fx.Commands;
using Phone7.Fx.Ioc;
using Phone7.Fx.Mvvm;

namespace LocationTestTask.UI.ViewModels
{
     [ViewModel(typeof(MapView))]
    public class MapViewModel:ViewModelBase
    {
         private readonly ILocationRepository _locationRepository;
         private int _zoomLevel=5;
         private DelegateCommand<object> _zoomInCommand;
         private DelegateCommand<object> _zoomOutCommand;
         private ObservableCollection<GeoCoordinate> _userPositions;
         private GeoCoordinate _mapCenter;
             
             
         [Injection]
         public MapViewModel(ILocationRepository locationRepository){
             _locationRepository = locationRepository;
             _userPositions=new ObservableCollection<GeoCoordinate>();
             _mapCenter=new GeoCoordinate();
         }

         public override void InitalizeData(){
             GetCoordinatesFromDbAndConvertForMap();
             
             
         }

         private void GetLastPositionAndCenterMap(IEnumerable<LocationDto> locations){
             var lastPosition=locations.FirstOrDefault(x=>x.MeasurementDatetime==locations.Max(y=>y.MeasurementDatetime));
             if (lastPosition != null){
                 MapCenter = new GeoCoordinate(lastPosition.MapPosition.Latitude, lastPosition.MapPosition.Longitude); 
             }
             
         }

         private void GetCoordinatesFromDbAndConvertForMap(){
             var positionsFromDb = _locationRepository.GetAll().ToList();
             GetLastPositionAndCenterMap(positionsFromDb);
             foreach (var locationDto in positionsFromDb){
                 UserPositions.Add(new GeoCoordinate(locationDto.MapPosition.Latitude, locationDto.MapPosition.Longitude));
             }
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

         public ObservableCollection<GeoCoordinate> UserPositions{
             get { return _userPositions; }
             set{
                 _userPositions = value;
                 base.RaisePropertyChanged(()=>UserPositions);
             }
         }

         public GeoCoordinate MapCenter{
             get { return _mapCenter; }
             set{
                 _mapCenter = value;
                 base.RaisePropertyChanged(()=>MapCenter);
             }
         }
    }
}
