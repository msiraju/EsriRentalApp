using Esri.ArcGISRuntime.Geometry;
using EsriCarRentalApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace EsriCarRentalAppTests
{
    [TestClass]
    public class CarTests
    {
        [TestMethod]
        public void Car_Constructor_ShouldGenerateGuid()
        {
            var car = new Car();

            Assert.IsNotNull(car.Guid);
        }

        [TestMethod]
        public void Car_SetModel_ShouldSetModel()
        {
            var car = new Car();
            var model = "TestModel";

            car.Model = model;

            Assert.AreEqual(model, car.Model);
        }

        [TestMethod]
        public void Car_SetIsAvailable_ShouldSetIsAvailable()
        {
            var car = new Car();
            var isAvailable = true;

            car.IsAvailable = isAvailable;

            Assert.AreEqual(isAvailable, car.IsAvailable);
        }

        [TestMethod]
        public void Car_SetBookingTime_ShouldSetBookingTime()
        {
            var car = new Car();
            var bookingTime = DateTime.Now;

            car.BookingTime = bookingTime;

            Assert.AreEqual(bookingTime, car.BookingTime);
        }

        [TestMethod]
        public void Car_SetBookedBy_ShouldSetBookedBy()
        {
            var car = new Car();
            var bookedBy = "Siraj";

            car.BookedBy = bookedBy;

            Assert.AreEqual(bookedBy, car.BookedBy);
        }

        [TestMethod]
        public void Car_SetLocation_ShouldSetLocation()
        {
            var car = new Car();
            var mockMapPoint = new Mock<MapPoint>();

            car.Location = mockMapPoint.Object;

            Assert.AreEqual(mockMapPoint.Object, car.Location);
        }
    }
}
