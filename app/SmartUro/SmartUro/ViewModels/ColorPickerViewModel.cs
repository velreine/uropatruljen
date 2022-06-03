using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartUro.ViewModels
{
    internal class ColorPickerViewModel : BaseViewModel
    {
        public Color ColorPicked { get; set; }
        public Color StartColor { get; set; }

        public int ComponentID { get; set; }

        public ICommand SaveColorCommand { get; }
        public ICommand CancelCommand { get; }

        public ColorPickerViewModel()
        {
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
            //await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async Task CancelColorPicking()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
