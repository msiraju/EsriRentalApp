using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EsriCarRentalApp.ViewModels;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace EsriCarRentalApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var container = new Container();
            container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();
            container.Register<IMediator, Mediator>(Lifestyle.Singleton);
            container.Register<IRentalViewModel, RentViewModel>(Lifestyle.Singleton);
            container.Register<IMapViewModel, MapViewModel>(Lifestyle.Singleton);
            container.Register<Rental>(Lifestyle.Transient);
            container.Register<MapViewer>(Lifestyle.Transient);

            container.Verify();

            var mainWindow = new MainWindow(container);

            mainWindow.Show();
        }
    }
}
