using EsriCarRentalApp;
using EsriCarRentalApp.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Windows.Input;

[TestClass]
public class MapViewModelTests
{
    [TestMethod]
    public void Initialize_WithMapView_SetsMapView()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var mapViewMock = new Mock<Esri.ArcGISRuntime.UI.Controls.MapView>();
        var viewModel = new MapViewModel(mediatorMock.Object);

        // Act
        viewModel.Initialize(mapViewMock.Object);

        // Assert
        Assert.AreEqual(mapViewMock.Object, viewModel.MyMapView);
    }

    [TestMethod]
    public void RentCommand_CanExecute_ReturnsTrue()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var viewModel = new MapViewModel(mediatorMock.Object);

        // Act
        bool canExecute = viewModel.RentCommand.CanExecute(null);

        // Assert
        Assert.IsTrue(canExecute);
    }

    [TestMethod]
    public void RentCommand_Executed_SendsMessageToMediator()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var viewModel = new MapViewModel(mediatorMock.Object);
        ICommand rentCommand = viewModel.RentCommand;

        // Act
        rentCommand.Execute(null);

        // Assert
        mediatorMock.Verify(m => m.SendMessage(It.IsAny<CarStatus>()), Times.Once);
    }

    [TestMethod]
    public void OnPropertyChanged_RaisesPropertyChangedEvent()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var viewModel = new MapViewModel(mediatorMock.Object);
        bool eventRaised = false;

        // Act
        viewModel.PropertyChanged += (sender, args) => { eventRaised = true; };
        viewModel.OnPropertyChanged(nameof(viewModel.MyMapView));

        // Assert
        Assert.IsTrue(eventRaised);
    }

    // Add more tests for other methods and edge cases as needed
}
