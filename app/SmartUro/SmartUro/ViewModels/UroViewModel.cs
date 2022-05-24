using CommonData.Model.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartUro.ViewModels
{
    internal class UroViewModel : BaseViewModel
    {
        public ICommand ToggleState { get; }

        private string _buttonText;
        public string ButtonText { get => _buttonText; set => OnPropertyChanged(ref _buttonText, value); }
        private string _buttonColor;
        public string ButtonColor { get => _buttonColor; set => OnPropertyChanged(ref _buttonColor, value); }

        private string _currentUro;
        public string CurrentUro { get => _currentUro; set => OnPropertyChanged(ref _currentUro, value); }

        private HardwareConfiguration _hardwareConfiguration;
        public HardwareConfiguration HardwareConfiguration { get => _hardwareConfiguration; set => OnPropertyChanged(ref _hardwareConfiguration, value); }

        public UroViewModel()
        {
            /*//Debug.WriteLine("UROVIEWMODEL HARDWARE NAME: " + HardwareConfiguration.Name);
            if (HardwareConfiguration == null)
                CurrentUro = "UroView";
            else
                CurrentUro = HardwareConfiguration.Name;*/

            ToggleState = new Command(async() => await ToggleStateAsync() );
            ButtonText = "OFF";
            ButtonColor = "lightgray";
        }
        private async Task ToggleStateAsync()
        {
            if (ButtonText == "ON") //if current component's state is 1
            {
                //await RestService.ToggleStateAsync(0);
                ButtonText = "OFF";
                ButtonColor = "lightgray";
            }
            else if (ButtonText == "OFF") //if current component's state is 0
            {
                //await RestService.ToggleStateAsync(1);
                ButtonText = "ON";
                ButtonColor = "lightgreen";
            }
        }
    }
}
