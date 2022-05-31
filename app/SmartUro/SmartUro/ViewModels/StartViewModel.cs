using System;
using System.Collections.Generic;
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
using SmartUro.Views.AddUroFlow;

namespace SmartUro.ViewModels
{
    internal class StartViewModel : BaseViewModel
    {        
        public ICollection<HardwareLayout> HardwareLayouts { get; set; }

        public ICommand Navigate { get; }
        public ICommand BeginAddUroFlowCommand { get; }

        public StartViewModel()
        {
            HardwareLayouts = new List<HardwareLayout>();
            GetListOfUros();

            Navigate = new Command<HardwareLayout>(async hw => await NavigateToUroView(hw));
            BeginAddUroFlowCommand = new Command(async () => await NavigateToSelectUserWifi());
        }

        private async Task NavigateToSelectUserWifi()
        {
            var page = new SelectUserWiFi();
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        private async Task NavigateToUroView(HardwareLayout hw)
        {
            var page = new UroView();
            page.BindingContext = new UroViewModel(hw);
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }



        private void GetListOfUros()
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
                                { new Pin() {Id=1, Direction = PinDirection.Input, Descriptor = "r", HwPinNumber = 10 } },
                                { new Pin() {Id=2, Direction = PinDirection.Input, Descriptor = "g", HwPinNumber = 11 } },
                                { new Pin() {Id=3, Direction = PinDirection.Input, Descriptor = "b", HwPinNumber = 12 } },
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
                                { new Pin() {Id=1, Direction = PinDirection.Input, Descriptor = "r", HwPinNumber = 13 } },
                                { new Pin() {Id=2, Direction = PinDirection.Input, Descriptor = "g", HwPinNumber = 14 } },
                                { new Pin() {Id=3, Direction = PinDirection.Input, Descriptor = "b", HwPinNumber = 15 } },
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
                                { new Pin() {Id=1, Direction = PinDirection.Input, Descriptor = "d", HwPinNumber = 16 } }
                            }
                        }
                    }
                }
            };
            HardwareLayouts.Add(hw1);
            HardwareLayouts.Add(hw2);

        }
    }
}
