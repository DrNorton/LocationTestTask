using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocationTestTask.DataLayer.Dto
{
    public class MapPositionDto:IDto
    {
        public long Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
       
    }
}
