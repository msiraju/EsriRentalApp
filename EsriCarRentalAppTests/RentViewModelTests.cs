using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Moq;
using EsriCarRentalApp;
using EsriCarRentalApp.ViewModels;

namespace EsriCarRentalAppTests
{
    [TestClass]
    public class RentViewModelTests
    {
        [TestMethod]
        public void Cars_Property_Should_Set_Value_And_Raise_PropertyChanged()
        {
            var mediatorMock = new Mock<IMediator>();
            var viewModel = new RentViewModel(mediatorMock.Object);
            var cars = new List<Car> { new Car { Model = "TestCar" } };

            viewModel.Cars = cars;

            CollectionAssert.AreEqual(cars, viewModel.Cars);
            AssertPropertyChanged(viewModel, nameof(viewModel.Cars));
        }

        private void AssertPropertyChanged(RentViewModel viewModel, string propertyName)
        {
            bool propertyChangedRaised = false;
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == propertyName)
                {
                    propertyChangedRaised = true;
                }
            };

            viewModel.OnPropertyChanged(propertyName);

            Assert.IsTrue(propertyChangedRaised, $"PropertyChanged not raised for {propertyName}");
        }
    }
}
