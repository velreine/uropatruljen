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
        private UroViewModel _uvm;

        //public ICollection<HardwareLayout> HardwareLayouts { get; set; }

        private ObservableCollection<Device> _userDevices;

        public ObservableCollection<Device> UserDevices { get => _userDevices; set => OnPropertyChanged(ref _userDevices, value); }

        public ICommand Navigate { get; }
        public ICommand BeginAddUroFlowCommand { get; }

        public ICommand GotoHomeManagementCommand { get; }

        public ICommand GotoRoomManagementCommand { get; }

        public ICommand GotoProfileManagementCommand { get; }


        #region DEBUG_HOME_AND_ROOMS
        private static Home home1 = new Home() {
            Id = 1,
            Name = "Mock Home 1",
            Rooms = {
                new Room() { Id = 1, Home = home1, Name = "Living Room" },
                new Room() { Id = 2, Home = home1, Name = "Bedroom" },
            },
        };
        private static Home home2 = new Home() { 
            Id = 1,
            Name = "Mock Home 2",
            Rooms =
            {
                new Room() { Id = 3, Home = home2, Name = "Stue" },
                new Room() { Id = 4, Home = home2, Name = "Soveværelse" },
            }
        };

        public List<Home> AvailableHomes { get; } = new List<Home>
        {
            home1,
            home2,
        };

        public Color IsCurrentMenuDeviceManagement { get; set; } = Color.Blue;
        public Color IsCurrentMenuHomeManagement { get; set; } = Color.Black;
        public Color IsCurrentMenuRoomManagement { get; set; } = Color.Black;
        public Color IsCurrentMenuProfileManagement { get; set; } = Color.Black;

        public Home SelectedHome { get; set; } = home1;

        public Room SelectedRoom { get; set; } = null;
        
        #endregion DEBUG_HOME_AND_ROOMS
        
        public StartViewModel(IDeviceService deviceService)
        {
            _deviceService = deviceService;

            LoadUserDevices();
            //HardwareLayouts = new List<HardwareLayout>();
            //GetListOfUros();

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