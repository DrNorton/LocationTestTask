using System;
using System.Device.Location;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LocationTestTask.DataLayer.Dto;

namespace LocationTestTask.UI.Converters
{
    public class LocationDtoToGeoCoordinateConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture){
            var locationDto = value as LocationDto;
            if (locationDto != null){
                return new GeoCoordinate(locationDto.MapPosition.Latitude,locationDto.MapPosition.Longitude);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture){
            var geoCoordinate = value as GeoCoordinate;
            if (geoCoordinate != null){
                return new LocationDto(){
                    MapPosition =
                        new MapPositionDto(){Latitude = geoCoordinate.Latitude, Longitude = geoCoordinate.Longitude}
                };
            }
            return null;
        }
    }
}
