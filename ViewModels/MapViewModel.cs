using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.UI.Controls;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.Generic;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Symbology;
using System.Linq;

namespace EsriCarRentalApp.ViewModels
{
    public class MapViewModel : ViewModelBase, IMapViewModel
    {
        private MapView myMapView;
        private Envelope uaeEnvelope;
        private IMediator mediator;
        private Dictionary<Car, GraphicsOverlay> localOverlayCollection;
        private Car selectedCar;
        private CarStatus carStatus;
        private bool mapTap;

        public MapView MyMapView
        {
            get => myMapView;
            set => myMapView = value;
        }

        public MapViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            this.mediator.Register<List<Car>>(cars => this.AddCarIconsToMap(cars));
            this.mediator.Register<Car>(this.OnCarSelected);
            this.mediator.Register<CarStatus>(this.HireOrReturn);
        }

        private void HireOrReturn(CarStatus carStatus)
        {
            try
            {
                this.carStatus = carStatus;
                if (carStatus.HireReturn == HireReturn.Hire)
                {
                    var overlay = this.localOverlayCollection[carStatus.Car];
                    this.MyMapView.GraphicsOverlays.Remove(overlay);
                    this.localOverlayCollection.Remove(carStatus.Car);
                    this.UnselectAll();
                }
                else if (carStatus.HireReturn == HireReturn.Return)
                {
                    MyMapView.GeoViewTapped += MyMapView_GeoViewTapped;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void MyMapView_GeoViewTapped(object sender, GeoViewInputEventArgs e)
        {
            try
            {
                MyMapView.GeoViewTapped -= MyMapView_GeoViewTapped;
                this.carStatus.Car.Location = e.Location;
                AddIcon(carStatus.Car, true);

                this.mapTap = true;
                this.mediator.Unregister<CarStatus>(HireOrReturn);
                this.mediator.SendMessage(this.carStatus);
                this.mapTap = false;

                this.mediator.Register<CarStatus>(HireOrReturn);
            }
            catch (Exception ex)
            {
            }
        }

        private void OnCarSelected(Car selected)
        {
            if (this.localOverlayCollection.Count == 0 || this.mapTap)
                return;

            this.UnselectAll();

            var overlay = this.localOverlayCollection[selected];
            this.MyMapView.GraphicsOverlays.Remove(overlay);

            GraphicsOverlay newOverlay = new GraphicsOverlay();

            SimpleMarkerSymbol simpleSymbol = new SimpleMarkerSymbol()
            {
                Color = System.Drawing.Color.Blue,
                Size = 35,
                Style = SimpleMarkerSymbolStyle.Diamond
            };

            Graphic graphicWithSymbol = new Graphic(selected.Location, simpleSymbol);
            newOverlay.Graphics.Add(graphicWithSymbol);

            this.MyMapView.GraphicsOverlays.Add(newOverlay);
            localOverlayCollection[selected] = newOverlay;

            this.selectedCar = selected;
        }

        private void UnselectAll()
        {
            this.MyMapView.GraphicsOverlays.Clear();

            foreach (var item in localOverlayCollection.Keys)
            {
                AddIcon(item, false);
            }
        }

        public void Initialize(MapView mapView)
        {
            this.MyMapView = mapView;
            Map myMap = new Map(BasemapStyle.ArcGISNavigation);
            this.uaeEnvelope = new Envelope(51.0, 22.0, 56.0, 26.0, SpatialReferences.Wgs84);
            MyMapView.Map = myMap;
            MyMapView.SetViewpoint(new Viewpoint(uaeEnvelope));
            MyMapView.Map.MaxExtent = uaeEnvelope;
            this.localOverlayCollection = new Dictionary<Car, GraphicsOverlay>();
        }

        private void AddCarIconsToMap(List<Car> cars)
        {
            foreach (var car in cars)
            {
                AddIcon(car, true);
            }
        }

        private void AddIcon(Car car, bool ignoreCol)
        {
            GraphicsOverlay overlay = new GraphicsOverlay();

            SimpleMarkerSymbol simpleSymbol = new SimpleMarkerSymbol()
            {
                Color = System.Drawing.Color.Red,
                Size = 25,
                Style = SimpleMarkerSymbolStyle.Diamond
            };

            //MapPoint randomPoint = new MapPoint(randomX, randomY, SpatialReferences.Wgs84);
            Graphic graphicWithSymbol = new Graphic(car.Location, simpleSymbol);
            overlay.Graphics.Add(graphicWithSymbol);

            // Add created overlay to the MapView
            this.MyMapView.GraphicsOverlays.Add(overlay);

            if (ignoreCol)
                this.localOverlayCollection.Add(car, overlay);
        }

        public ICommand RentCommand { get; private set; }
    }
}
