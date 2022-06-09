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
using CommonData.Model.Action;
using SmartUro.Interfaces;
using Device = CommonData.Model.Entity.Device;

namespace SmartUro.ViewModels
{
    internal class UroViewModel : BaseViewModel
    {
        private string _buttonText;
        private Color _buttonColor;
        private Color _diodeColor;
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
        public Color DiodeColor
        {
            get => _diodeColor;
            set => OnPropertyChanged(ref _diodeColor, value);
        }
        public Device Device 
        { 
            get => _device; 
            set => OnPropertyChanged(ref _device, value); 
        }

        public ICommand ToggleStateCommand { get; }
        public ICommand GoToColorPickerCommand { get; set; }

        public UroViewModel(IMqttService mqttService)
        {
            _mqttService = mqttService;
            DiodeColor = Color.Violet;

            ToggleStateCommand = new Command<Component>(async (component) => await ToggleState(component));
            GoToColorPickerCommand = new Command<Component>(async _component => await GoToColorPicker(_component));
            ButtonText = "OFF";
            ButtonColor = Color.LightGray;
        }

        private async Task GoToColorPicker(Component _component)
        {
            var page = new ColorPickerView();
            var _cpvm = (ColorPickerViewModel)page.BindingContext;
            _cpvm.ComponentID = (int)_component.Id!;
            await Application.Current.MainPage.Navigation.PushModalAsync(page, true);
            DiodeColor = _cpvm.ColorPicked;
        }

        private async Task ToggleState(Component component)
        {
            IAction action = null;
            
            if (ButtonText == "ON")
            {

                action = new TurnOnOffAction()
                {
                    ComponentIdentifier = (int)component.Id!,
                    TurnOn = false
                };
                
                ButtonText = "OFF";
                ButtonColor = Color.LightGray;
            }
            else if (ButtonText == "OFF")
            {

                action = new TurnOnOffAction()
                {
                    ComponentIdentifier = (int)component.Id!,
                    TurnOn = true
                };
                
                ButtonText = "ON";
                ButtonColor = Color.LightGreen;
            }

            if (action != null)
            {
                var apl = ActionPayload.FromAction(action);
                var msg = new MqttApplicationMessage
                {
                    Payload = apl.ToPayload(),
                    Topic = $"/device_actions/{Device.SerialNumber}"
                };
                
                // { topic: "/device_actions/Device.SerialNumber", action:                
                
                await _mqttService.Client.PublishAsync(msg);
            }
        }
    }
}
