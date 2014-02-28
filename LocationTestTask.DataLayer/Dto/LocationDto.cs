using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocationTestTask.DataLayer.Dto
{
    public class LocationDto:IDto
    {
        public long Id { get; set; }
        public DateTime MeasurementDatetime { get; set; }
        public MapPositionDto MapPosition { get; set; }

        public string LatitudeString{
            get{
                if (MapPosition == null)
                {
                    return String.Empty;
                }
                return String.Format("Широта: {0}", MapPosition.Latitude);
            }
        }
        public string LongitudeString
        {
            get
            {
                if (MapPosition == null){
                    return String.Empty;
                }
                return String.Format("Долгота: {0}", MapPosition.Latitude);
            }
        }

        
    }
}
