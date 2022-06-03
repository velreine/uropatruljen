using SkiaSharp;
using SkiaSharp.Views.Forms;
using SmartUro.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartUro.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorPickerView : ContentPage
    {
        public ColorPickerView()
        {
            InitializeComponent();
            this.BindingContext = App.GetViewModel<ColorPickerViewModel>();
        }
        private void ColorPicker_PickedColorChanged(object sender, Color colorPicked)
        {
            // Use the selected color
            SelectedColorDisplayFrame.BackgroundColor = colorPicked;
            SelectedColorValueLabel.Text = colorPicked.ToHex();
        }
    }
}