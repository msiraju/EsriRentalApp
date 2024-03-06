using Esri.ArcGISRuntime.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EsriCarRentalApp
{
    public class RentingService
    {
        private List<Car> cars;

        public RentingService(List<Car> cars)
        {
            this.cars = cars;
        }

        public Car RentCar(Car car, string bookedBy)
        {
            var rented = this.cars.First(x => x == car);
            rented.BookingTime = DateTime.Now;
            rented.IsAvailable = false;
            rented.BookedBy = bookedBy;
            return rented;
        }

        public Car ReturnCar(Car car)
        {
            var returned = this.cars.First(x => x == car);
            returned.IsAvailable = true;
            returned.Location = car.Location;
            returned.BookedBy = string.Empty;
            returned.BookingTime = DateTime.MinValue;
            return returned;
        }

        public Car GetCar(Car car)
        {
            return this.cars.FirstOrDefault(x => x.Guid == car.Guid);
        }
    }

    public class CarStatus
    {
        public Car Car { get; set; }
        public HireReturn HireReturn { get; set; }
    }

    public enum HireReturn
    {
        Hire = 1,
        Return = 2
    }
}
