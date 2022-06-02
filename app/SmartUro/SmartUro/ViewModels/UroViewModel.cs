using CommonData.Model.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using SmartUro.Views;
using MQTTnet;
using System.Threading;
using SmartUro.Interfaces;
using Device = CommonData.Model.Entity.Device;

namespace SmartUro.ViewModels
{
    internal class UroViewModel : BaseViewModel
    {
        private string _buttonText;
        private Color _buttonColor;
        private Device _device;
        private readonly IMqttService _mqttService;

        public string ButtonText 
        { 
            get => _buttonText; 
            set => OnPropertyChanged(ref _buttonText, value); 
        }
        public Color ButtonColor 
        { 
            get => _buttonColor; 
            set => OnPropertyChanged(ref _buttonColor, value); 
        }
        public Device Device 
        { 
            get => _device; 
            set => OnPropertyChanged(ref _device, value); 
        }

        public ICommand ToggleStateCommand { get; }

        public UroViewModel(IMqttService mqttService)
        {
            _mqttService = mqttService;

            ToggleStateCommand = new Command(async() => await ToggleState());
            ButtonText = "OFF";
            ButtonColor = Color.LightGray;
        }

        private async Task ToggleState()
        {
            await _mqttService.SendRequest();

            if (ButtonText == "ON") //if current component's state is 1
            {
                //await RestService.ToggleStateAsync(0);
                ButtonText = "OFF";
                ButtonColor = Color.LightGray;
            }
            else if (ButtonText == "OFF") //if current component's state is 0
            {
                //await RestService.ToggleStateAsync(1);
                ButtonText = "ON";
                ButtonColor = Color.LightGreen;
            }
        }
    }
}
