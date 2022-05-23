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

        public HardwareConfiguration HardwareConfiguration { get; set; }

        public UroViewModel()
        {
            Debug.WriteLine("UroViewModel Constructor");
            ToggleState = new Command(async() => await ToggleStateAsync() );
            ButtonText = "OFF";
            Debug.WriteLine("UroViewModel Constructor Done");
        }
        private async Task ToggleStateAsync()
        {
            if (ButtonText == "ON") //if current component's state is 1
            {
                //await RestService.ToggleStateAsync(0);
                ButtonText = "OFF";
            }
            else if (ButtonText == "OFF") //if current component's state is 0
            {
                //await RestService.ToggleStateAsync(1);
                ButtonText = "ON";
            }
        }
    }
}
