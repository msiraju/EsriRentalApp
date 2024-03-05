using System;
using Esri.ArcGISRuntime.Geometry;

namespace EsriCarRentalApp
{
    // Car class representing each type of car
    public class Car
    {
        public Car()
        {
            this.Guid = System.Guid.NewGuid().ToString();
        }

        public string Guid { get; private set; }
        public string Model { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime BookingTime { get; set; }
        public string BookedBy { get; set; }
        public MapPoint Location { get; set; } // ESRI MapPoint representing location
    }

}
