﻿using System;
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

namespace SmartUro.ViewModels
{
    internal class StartViewModel : BaseViewModel
    {
        public ICommand Navigate { get; }

        public ICollection<HardwareConfiguration> HardwareConfigurations { get; set; }


        public StartViewModel()
        {
            HardwareConfigurations = new List<HardwareConfiguration>();
            GetListOfUrosAsync();
            //Navigate = new Command(async() => await NavigateToUroView());

            Navigate = new Command(async () => await NavigateToUroView());
        }

        private async Task NavigateToUroView()
        {
            var page = new UroView();
            //var pageContext = page.BindingContext as UroViewModel;
            //pageContext.HardwareConfiguration = new HardwareConfiguration();
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        private void GetListOfUrosAsync()
        {
            //Dummy data for testing
            HardwareConfiguration hw1 = new HardwareConfiguration()
            {
                Id = 1,
                Name = "Smart Uro V1",
                Serialnumber = "ABC-123-123",
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
                    }
                }
            };
            HardwareConfiguration hw2 = new HardwareConfiguration()
            {
                Id = 1,
                Name = "Smart Uro V2",
                Serialnumber = "EDF-666-999",
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
                                { new Pin() {Id=1, Direction = PinDirection.Input, Descriptor = "d", HwPinNumber = 10 } }
                            }
                        }
                    }
                }
            };
            HardwareConfigurations.Add(hw1);
            HardwareConfigurations.Add(hw2);

        }
    }
}