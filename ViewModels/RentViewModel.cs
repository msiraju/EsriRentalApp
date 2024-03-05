using Esri.ArcGISRuntime.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EsriCarRentalApp.ViewModels
{
    public class RentViewModel : INotifyPropertyChanged, IRentalViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Car selectedCar;
        private RentingService rentingService;
        private IMediator mediator;
        private List<Car> cars;
        private List<Car> hiredCars;
        private Car selectedHiredCar;
        private string lastModelName;
        private string lastBookingName;
        private string lastBookingTime;
        private string lastReturnModelName;
        private string lastReturnBookingName;
        private string lastReturnTime;
        private string bookedBy;
        private string helperText;
        private bool returnedClicked;

        public List<Car> Cars
        {
            get => cars;
            set
            {
                cars = value;
                OnPropertyChanged();
            }
        }

        public Car SelectedCar
        {
            get => selectedCar;
            set
            {
                if (value != null)
                {
                    selectedCar = value;
                    OnPropertyChanged();

                    this.mediator.SendMessage(value);
                }
            }
        }

        public List<Car> HiredCars
        {
            get => hiredCars;
            set
            {
                if (value != null)
                {
                    hiredCars = value;
                    OnPropertyChanged();
                }
            }
        }

        public Car SelectedHiredCar
        {
            get => selectedHiredCar;
            set
            {
                if (value != null)
                {
                    selectedHiredCar = value;
                    OnPropertyChanged();

                    var car = this.rentingService.GetCar(value);

                    this.LastReturnModelName = car.Model;
                    this.LastReturnTime = car.BookingTime.ToString();
                    this.LastReturnBookingName = car.BookedBy;
                }
            }
        }

        public string LastModelName
        {
            get => lastModelName;
            set
            {
                lastModelName = value;
                OnPropertyChanged();
            }
        }

        public string LastBookingName
        {
            get => lastBookingName;
            set
            {
                lastBookingName = value;
                OnPropertyChanged();
            }
        }

        public string LastBookingTime
        {
            get => lastBookingTime;
            set
            {
                lastBookingTime = value;
                OnPropertyChanged();
            }
        }

        public string LastReturnModelName
        {
            get => lastReturnModelName;
            set
            {
                lastReturnModelName = value;
                OnPropertyChanged();
            }
        }

        public string LastReturnBookingName
        {
            get => lastReturnBookingName;
            set
            {
                lastReturnBookingName = value;
                OnPropertyChanged();
            }
        }

        public string LastReturnTime
        {
            get => lastReturnTime;
            set
            {
                lastReturnTime = value;
                OnPropertyChanged();
            }
        }

        public string BookedBy
        {
            get => bookedBy;
            set
            {
                bookedBy = value;
                OnPropertyChanged();
            }
        }

        public string HelperText
        {
            get => helperText;
            set
            {
                helperText = value;
                OnPropertyChanged();
            }
        }

        public RentViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            this.mediator.Register<string>(HandleMessage);

            this.HiredCars = new List<Car>();
            this.Cars = this.GetMockCars();
            rentingService = new RentingService(new List<Car>(this.Cars.ToList()));
            RentCommand = new RelayCommand(HireCar);
            ReturnCommand = new RelayCommand(ReturnCar);
            var models = this.Cars.Select(x => x.Model).ToList();

            this.SetAvailableCars();

            this.HelperText = "Hint:\n" +
                "1. Select a model from dropdown" +
                "\n2. Hit Return" +
                "\n3. Select a location from map to return";
        }

        private void HireOrReturn(CarStatus carStatus)
        {
            if (carStatus.HireReturn == HireReturn.Return)
            {
                this.returnedClicked = false;
                this.mediator.Unregister<CarStatus>(this.HireOrReturn);

                this.Cars.Add(carStatus.Car);
                this.HiredCars.Remove(carStatus.Car);

                var refreshed = new List<Car>(this.Cars);
                this.Cars = refreshed;

                refreshed = new List<Car>(this.HiredCars);
                this.HiredCars = refreshed;

                this.rentingService.ReturnCar(carStatus.Car);
                this.SelectedCar = this.Cars.FirstOrDefault();
                this.SelectedHiredCar = this.HiredCars.FirstOrDefault();

            }
        }

        private void ReturnCar(object obj)
        {
            if (this.returnedClicked)
                return;

            this.returnedClicked = true;
            var status = new CarStatus
            {
                Car = this.selectedHiredCar,
                HireReturn = HireReturn.Return
            };

            this.mediator.SendMessage(status);
            this.mediator.Register<CarStatus>(this.HireOrReturn);
        }

        private List<Car> GetMockCars()
        {
            return new List<Car>
            {
                new Car { Model = "Tesla Model 3", IsAvailable = true, Location = new MapPoint(6051537.68851016, 2808516.95091897, SpatialReferences.WebMercator) },
                new Car { Model = "Volkswagen Golf", IsAvailable = true, Location = new MapPoint(6222287.48827597, 2908481.17888848, SpatialReferences.WebMercator) },
                new Car { Model = "Volkswagen Polo", IsAvailable = true, Location = new MapPoint(6271640.67884281, 2891391.43935443, SpatialReferences.WebMercator) },
                new Car { Model = "Volkswagen Polo", IsAvailable = true, Location = new MapPoint(6208110.69256744, 2780090.84324409, SpatialReferences.WebMercator) },
                new Car { Model = "Volkswagen Polo", IsAvailable = true, Location = new MapPoint(6121456.69076434, 2876061.01080588, SpatialReferences.WebMercator) },
                new Car { Model = "Range Rover", IsAvailable = true, Location = new MapPoint(6179347.00900426, 2816444.22892311, SpatialReferences.WebMercator) },
                new Car { Model = "Porsche 911", IsAvailable = true, Location = new MapPoint(6152549.71703201, 2899197.25476205, SpatialReferences.WebMercator) },
            };
        }

        private async void SetAvailableCars()
        {
            await Task.Delay(100);
            this.mediator.SendMessage(this.Cars);
            this.SelectedCar = this.Cars.FirstOrDefault();
        }

        private void HandleMessage(string obj)
        {
        }

        private void HireCar(object obj)
        {
            if (string.IsNullOrEmpty(this.BookedBy))
                return;

            var status = new CarStatus
            {
                Car = this.selectedCar,
                HireReturn = HireReturn.Hire
            };
            this.mediator.SendMessage(status);

            this.Cars.Remove(this.selectedCar);
            this.HiredCars.Add(this.selectedCar);

            var refreshed = new List<Car>(this.Cars);
            this.Cars = refreshed;

            refreshed = new List<Car>(this.HiredCars);
            this.HiredCars = refreshed;

            var rented = this.rentingService.RentCar(status.Car, this.BookedBy);
            this.LastModelName = rented.Model;
            this.LastBookingName = rented.BookedBy;
            this.LastBookingTime = rented.BookingTime.ToString();

            this.SelectedCar = this.Cars.FirstOrDefault();
            this.SelectedHiredCar = this.HiredCars.FirstOrDefault();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand RentCommand { get; private set; }

        public ICommand ReturnCommand { get; private set; }
    }
}
