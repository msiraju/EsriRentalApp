using EsriCarRentalApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EsriCarRentalAppTests
{
    [TestClass]
    public class RentingServiceTests
    {
        [TestMethod]
        public void RentCar_ShouldSetBookingDetailsAndMarkCarAsUnavailable()
        {
            var cars = new List<Car>
            {
                new Car { Guid = Guid.NewGuid().ToString(), IsAvailable = true }
            };
            var rentingService = new RentingService(cars);
            var carToRent = cars.First();
            var bookedBy = "Siraj";

            var rentedCar = rentingService.RentCar(carToRent, bookedBy);

            Assert.IsFalse(carToRent.IsAvailable);
            Assert.AreEqual(bookedBy, carToRent.BookedBy);
            Assert.AreEqual(rentedCar, carToRent);
            Assert.AreNotEqual(DateTime.MinValue, carToRent.BookingTime);
        }

        [TestMethod]
        public void ReturnCar_ShouldMarkCarAsAvailableAndClearBookingDetails()
        {
            var cars = new List<Car>
            {
                new Car { Guid = Guid.NewGuid().ToString(), IsAvailable = true }
            };
            var rentingService = new RentingService(cars);
            var carToReturn = cars.First();

            var returnedCar = rentingService.ReturnCar(carToReturn);

            Assert.IsTrue(carToReturn.IsAvailable);
            Assert.AreEqual(string.Empty, carToReturn.BookedBy);
            Assert.AreEqual(DateTime.MinValue, carToReturn.BookingTime);
            Assert.AreEqual(returnedCar, carToReturn);
        }

        [TestMethod]
        public void GetCar_ShouldReturnCorrectCar()
        {
            var cars = new List<Car>
            {
                    new Car { Guid = Guid.NewGuid().ToString() },
                    new Car { Guid = Guid.NewGuid().ToString() }
            };
            var rentingService = new RentingService(cars);
            var carToGet = cars.First();

            var retrievedCar = rentingService.GetCar(carToGet);

            Assert.IsNotNull(retrievedCar);
            Assert.AreEqual(carToGet, retrievedCar);
        }

        [TestMethod]
        public void GetCar_ShouldReturnNullIfCarNotFound()
        {
            var cars = new List<Car>
            {
                    new Car { Guid = Guid.NewGuid().ToString() }
            };
            var rentingService = new RentingService(cars);
            var carToGet = new Car { Guid = Guid.NewGuid().ToString() };

            var retrievedCar = rentingService.GetCar(carToGet);

            Assert.IsNull(retrievedCar);
        }
    }

}
