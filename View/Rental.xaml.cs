using EsriCarRentalApp.ViewModels;
using System.Windows.Controls;

namespace EsriCarRentalApp
{
    /// <summary>
    /// Interaction logic for Rental.xaml
    /// </summary>
    public partial class Rental : UserControl
    {
        public Rental(IRentalViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
