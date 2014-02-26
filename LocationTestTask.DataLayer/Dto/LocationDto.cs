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
    }
}
