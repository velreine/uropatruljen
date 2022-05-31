using CommonData.Model.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using SmartUro.Views;

namespace SmartUro.ViewModels
{
    internal class UroViewModel : BaseViewModel
    {
        private string _buttonText;
        private Color _buttonColor;
        private HardwareLayout _hardwareLayout;

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
        public HardwareLayout HardwareLayout 
        { 
            get => _hardwareLayout; 
            set => OnPropertyChanged(ref _hardwareLayout, value); 
        }

        public ICommand ToggleStateCommand { get; }

        public UroViewModel(HardwareLayout _config)
        {
            this.HardwareLayout = _config;

            ToggleStateCommand = new Command(async() => await ToggleState());
            ButtonText = "OFF";
            ButtonColor = Color.LightGray;
        }

        private async Task ToggleState()
        {
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
