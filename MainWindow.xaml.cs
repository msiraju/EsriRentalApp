using System.Collections.Generic;
using System.Windows;
using SimpleInjector;
using System.Windows.Controls;
using System.Configuration;

namespace EsriCarRentalApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Container container;
        public List<Car> Cars { get; set; }

        public MainWindow(Container container)
        {
            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = ConfigurationManager.AppSettings["ApiKey"];

            InitializeComponent();
            this.container = container;
            SetContentView<Rental>(this.Rental);
            SetContentView<MapViewer>(this.MapViewer);
        }

        private void SetContentView<T>(ContentControl contentControl) where T : UIElement
        {
            var view = container.GetInstance<T>();
            contentControl.Content = view;
        }
    }
}
