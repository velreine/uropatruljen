using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using SmartUro.Services;
using SmartUro.Views;
using System.Runtime.CompilerServices;
using CommonData.Model.Entity;
using CommonData.Model.Static;
using CommonData.Model;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using CommonData.Model.DTO;
using MQTTnet;
using MQTTnet.Client;
using SmartUro.Views.AddUroFlow;
using SmartUro.Interfaces;
using SmartUro.ViewModels.HomeManagement;
using SmartUro.ViewModels.RoomManagement;
using SmartUro.Views.RoomManagement;
using Device = CommonData.Model.Entity.Device;

namespace SmartUro.ViewModels
{
    internal class StartViewModel : BaseViewModel
    {
        private readonly IDeviceService _deviceService;
        private readonly IHomeService _homeService;
        private readonly IHardwareLayoutService _hardwareLayoutService;

        //public ICollection<HardwareLayout> HardwareLayouts { get; set; }

        private ObservableCollection<Device> _devicesInSelectedHome;

        public ObservableCollection<Device> DevicesInSelectedHome
        {
            get => _devicesInSelectedHome;
            set => OnPropertyChanged(ref _devicesInSelectedHome, value);
        }

        private ICollection<Device> _userDevices;

        private ObservableCollection<Home> _userHomes;

        public ObservableCollection<Home> UserHomes
        {
            get => _userHomes;
            set => OnPropertyChanged(ref _userHomes, value);
        }

        public ICommand Navigate { get; }
        public ICommand BeginAddUroFlowCommand { get; }

        public ICommand GotoHomeManagementCommand { get; }

        public ICommand GotoRoomManagementCommand { get; }

        public ICommand GotoProfileManagementCommand { get; }

        public Color IsCurrentMenuDeviceManagement { get; set; } = Color.Blue;
        public Color IsCurrentMenuHomeManagement { get; set; } = Color.Black;
        public Color IsCurrentMenuRoomManagement { get; set; } = Color.Black;
        public Color IsCurrentMenuProfileManagement { get; set; } = Color.Black;

        private Home _selectedHome = null;
        private Room _selectedRoom = null;

        public Home SelectedHome
        {
            get => _selectedHome;
            set => OnPropertyChanged(ref _selectedHome, value);
        }

        public Room SelectedRoom
        {
            get => _selectedRoom;
            set => OnPropertyChanged(ref _selectedRoom, value);
        }

        private List<Room> _roomsInSelectedHome;

        public List<Room> RoomsInSelectedHome
        {
            get => _roomsInSelectedHome;
            set => OnPropertyChanged(ref _roomsInSelectedHome, value);
        }


        public StartViewModel(IDeviceService deviceService, IHomeService homeService,
            IHardwareLayoutService hardwareLayoutService)
        {
            _deviceService = deviceService;
            _homeService = homeService;
            _hardwareLayoutService = hardwareLayoutService;

            LoadUserData();
            //LoadUserDevices();
            //LoadUserHomes();

            // Register a handler for updating the Available Rooms when the home changes.
            /*this.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SelectedHome))
                {
                    if (SelectedHome != null)
                    {
                        //RoomsInSelectedHome = SelectedHome.Rooms.ToList();
                        DevicesInSelectedHome = (ObservableCollection<Device>)SelectedHome.Devices;
                    }
                    else
                    {
                        RoomsInSelectedHome = null;
                    }
                }
            };*/

            Navigate = new Command<Device>(async device => await NavigateToUroView(device));
            BeginAddUroFlowCommand = new Command(async () => await NavigateToSelectUserWifi());
            GotoHomeManagementCommand = new Command(async () => await GoToHomeManagement());
            GotoRoomManagementCommand = new Command(async () => await GotoRoomManagement());
            GotoProfileManagementCommand = new Command(async () => await GotoProfileManagement());
        }

        private async Task NavigateToSelectUserWifi()
        {
            var page = new SelectUserWiFi();
            await Application.Current.MainPage.Navigation.PushAsync(page, true);
        }

        private async Task GotoProfileManagement()
        {
            var page = new ProfileManagementView();
            await Application.Current.MainPage.Navigation.PushAsync(page, true);
        }

        private async Task GotoRoomManagement()
        {
            var page = new RoomManagementView();
            var vm = (RoomManagementViewModel)page.BindingContext;

            var rooms = SelectedHome != null
                ? new ObservableCollection<Room>(SelectedHome.Rooms)
                : null;

            vm.Rooms = rooms;

            await Application.Current.MainPage.Navigation.PushAsync(page, true);
        }

        private async Task GoToHomeManagement()
        {
            var page = new HomeManagementView();
            var vm = (HomeManagementViewModel)page.BindingContext;
            vm.Homes = UserHomes;
            await Application.Current.MainPage.Navigation.PushAsync(page, true);
        }

        private async Task NavigateToUroView(Device device)
        {
            var page = new UroView();
            var vm = (UroViewModel)page.BindingContext;
            vm.Device = device;

            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        private async Task LoadUserData()
        {
            Debug.WriteLine("LoadUserData() invoked...");

            // Load all user data and stitch it together.
            var homesData = await _homeService.GetUserHomes();

            var x = 1;

            Debug.WriteLine("x;");


            // Convert AuthenticatedUserHomeResponseDTO to Home domain-model objects.
            var homes = new ObservableCollection<Home>();
            foreach (AuthenticatedUserHomeResponseDTO homeData in homesData)
            {
                // Convert the AuthenticatedUserRoom objects to Room domain-model objects.
                var rooms = new ObservableCollection<Room>();
                foreach (AuthenticatedUserRoom roomData in homeData.Rooms)
                {
                    var room = new Room(roomData.Name, roomData.HomeId);
                    rooms.Add(room);
                }

                // Create a Home domain-model object.
                var home = new Home(
                    homeData.Name,
                    new ObservableCollection<Person>(),
                    rooms,
                    new ObservableCollection<Device>()
                );
                home.Id = homeData.Id;

                // Add the home to the list.
                homes.Add(home);
            }


            // Load user used hardware layouts and devices.
            var usedLayouts = await _hardwareLayoutService.GetUserHardwareLayouts();
            var devicesData = await _deviceService.GetUserDevices();

            // Convert AuthenticatedUserDevice to Device domain-model objects.
            var devices = new ObservableCollection<Device>();
            foreach (AuthenticatedUserDevice deviceData in devicesData)
            {
                var zz = 5;
                var layout = usedLayouts.First(l => l.Id == deviceData.HardwareLayoutId);
                var home = homes.First(h => h.Id == deviceData.HomeId);
                var room = home.Rooms.FirstOrDefault(r => r.Id == deviceData.RoomId);

                var device = new Device(deviceData.Name, deviceData.SerialNumber, layout, home, room);

                layout.AddDevice(device);
                devices.Add(device);
                home.AddDevice(device);
            }


            var z = 3;

            UserHomes = homes;

            
            SelectedHome = homes.Count > 0 ? homes[0] : null;

            //_userDevices = (ObservableCollection<Device>)SelectedHome?.Devices;
            //DevicesInSelectedHome = (ObservableCollection<Device>)SelectedHome?.Devices;
        }

        /*private async Task LoadUserDevices()
        {
            var devices = await _deviceService.GetUserDevices();
            UserDevices = new ObservableCollection<Device>(devices);
        }

        private async Task LoadUserHomes()
        {
            var homes = await _homeService.GetUserHomes();
            UserHomes = new ObservableCollection<Home>(homes);
            SelectedHome = UserHomes.Count > 0 ? UserHomes[0] : null;
        }*/

        /*private void GetListOfUros()
        {
            //Dummy data for testing
            HardwareLayout hw1 = new HardwareLayout()
            {
                Id = 1,
                ProductName = "Smart Uro V1",
                ModelNumber = "ABC-123-123",
                AttachedComponents = new List<Component>()
                {
                    {
                        new Component()
                        {
                            Id = 1,
                            Name = "RGB_DIODE_1",
                            Type = ComponentType.RgbDiode,
                            Pins = new List<Pin>()
                            {
                                {
                                    new Pin()
                                    {
                                        Id = 1, Direction = PinDirection.Input, Descriptor = "r", HwPinNumber = 10
                                    }
                                },
                                {
                                    new Pin()
                                    {
                                        Id = 2, Direction = PinDirection.Input, Descriptor = "g", HwPinNumber = 11
                                    }
                                },
                                {
                                    new Pin()
                                    {
                                        Id = 3, Direction = PinDirection.Input, Descriptor = "b", HwPinNumber = 12
                                    }
                                },
                            }
                        }
                    },
                    {
                        new Component()
                        {
                            Id = 2,
                            Name = "RGB_DIODE_2",
                            Type = ComponentType.RgbDiode,
                            Pins = new List<Pin>()
                            {
                                {
                                    new Pin()
                                    {
                                        Id = 1, Direction = PinDirection.Input, Descriptor = "r", HwPinNumber = 13
                                    }
                                },
                                {
                                    new Pin()
                                    {
                                        Id = 2, Direction = PinDirection.Input, Descriptor = "g", HwPinNumber = 14
                                    }
                                },
                                {
                                    new Pin()
                                    {
                                        Id = 3, Direction = PinDirection.Input, Descriptor = "b", HwPinNumber = 15
                                    }
                                },
                            }
                        }
                    }
                }
            };
            HardwareLayout hw2 = new HardwareLayout()
            {
                Id = 1,
                ProductName = "Smart Uro V2",
                ModelNumber = "EDF-666-999",
                AttachedComponents = new List<Component>()
                {
                    {
                        new Component()
                        {
                            Id = 1,
                            Name = "DIODE_1",
                            Type = ComponentType.Diode,
                            Pins = new List<Pin>()
                            {
                                {
                                    new Pin()
                                    {
                                        Id = 1, Direction = PinDirection.Input, Descriptor = "d", HwPinNumber = 16
                                    }
                                }
                            }
                        }
                    }
                }
            };
            HardwareLayouts.Add(hw1);
            HardwareLayouts.Add(hw2);
        }*/
    }
}