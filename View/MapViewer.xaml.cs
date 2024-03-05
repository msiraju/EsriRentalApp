using EsriCarRentalApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace EsriCarRentalApp
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>ViewA
    public partial class MapViewer : UserControl
    {
        public MapViewer(IMapViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            Loaded += MapViewer_Loaded;
        }

        private void MapViewer_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MapViewModel;
            vm.Initialize(this.MyMapView);
        }
    }
}
