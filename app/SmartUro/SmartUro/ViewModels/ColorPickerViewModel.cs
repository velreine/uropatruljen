using CommonData.Model.Action;
using MQTTnet;
using SmartUro.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Device = CommonData.Model.Entity.Device;

namespace SmartUro.ViewModels
{
    internal class ColorPickerViewModel : BaseViewModel
    {
        private readonly IMqttService _mqttService;

        public Color ColorPicked { get; set; }
        public Color StartColor { get; set; }
        private Device _device;

        public int ComponentID { get; set; }
        public Device Device
        {
            get => _device;
            set => OnPropertyChanged(ref _device, value);
        }

        public ICommand SaveColorCommand { get; }
        public ICommand CancelCommand { get; }

        public ColorPickerViewModel(IMqttService mqttService)
        {
            _mqttService = mqttService;
            ColorPicked = Color.White;

            SaveColorCommand = new Command(async () => await SaveColor());
            CancelCommand = new Command(async () => await CancelColorPicking() );
        }

        private async Task SaveColor()
        {
            Debug.WriteLine("ID: " + ComponentID);
            Debug.WriteLine("RED: " + Math.Truncate(ColorPicked.R * 255));
            Debug.WriteLine("GREEN: " + Math.Truncate(ColorPicked.G * 255));
            Debug.WriteLine("BLUE: " + Math.Truncate(ColorPicked.B * 255));


            IAction action = new SetColorAction()
            {
                RValue = (byte)Math.Truncate(ColorPicked.R * 255),
                GValue = (byte)Math.Truncate(ColorPicked.G * 255),
                BValue = (byte)Math.Truncate(ColorPicked.B * 255)
            };

            var apl = ActionPayload.FromAction(action);
            var msg = new MqttApplicationMessage
            {
                Payload = apl.ToPayload(),
                Topic = $"/device_actions/{Device.SerialNumber}"
            };             

            await _mqttService.Client.PublishAsync(msg);


            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async Task CancelColorPicking()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
