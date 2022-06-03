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
using MQTTnet;
using MQTTnet.Client;
using SmartUro.Views.AddUroFlow;
using SmartUro.Interfaces;
using SmartUro.Views.RoomManagement;
using Device = CommonData.Model.Entity.Device;

namespace SmartUro.ViewModels
{
    internal class StartViewModel : BaseViewModel
    {
        private readonly IDeviceService _deviceService;
        private readonly IHomeService _homeService;
        private UroViewModel _uvm;

        //public ICollection<HardwareLayout> HardwareLayouts { get; set; }

        private ObservableCollection<Device> _userDevices;

        public ObservableCollection<Device> UserDevices
        {
            get => _userDevices;
            set => OnPropertyChanged(ref _userDevices, value);
        }

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
        
        
        
        public StartViewModel(IDeviceService deviceService , IHomeService homeService)
        {
            _deviceService = deviceService;
            _homeService = homeService;

            LoadUserDevices();
            LoadUserHomes();
            //HardwareLayouts = new List<HardwareLayout>();
            //GetListOfUros();
            
            // Register a handler for updating the AvailableRooms when the home changes.
            this.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SelectedHome))
                {
                    if (SelectedHome != null)
                    {
                        RoomsInSelectedHome = SelectedHome.Rooms.ToList();
                    }
                    else
                    {
                        RoomsInSelectedHome = null;
                    }
                }
            };

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
            var page = new Views.RoomManagement.RoomManagementView();
            await Application.Current.MainPage.Navigation.PushAsync(page, true);
        }

        private async Task GoToHomeManagement()
        {
            var page = new HomeManagementView();
            await Application.Current.MainPage.Navigation.PushAsync(page, true);
        }

        private async Task NavigateToUroView(Device device)
        {
            var page = new UroView();
            page.BindingContext = _uvm = (UroViewModel)App.GetViewModel<UroViewModel>();
            _uvm.Device = device;

            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        private async Task LoadUserDevices()
        {
            var devices = await _deviceService.GetUserDevices();
            UserDevices = new ObservableCollection<Device>(devices);
        }

        private async Task LoadUserHomes()
        {
            var homes = await _homeService.GetUserHomes();
            UserHomes = new ObservableCollection<Home>(homes);
            SelectedHome = UserHomes.Count > 0 ? UserHomes[0] : null;
        }
        
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