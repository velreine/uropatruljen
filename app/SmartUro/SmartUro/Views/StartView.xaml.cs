using SmartUro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartUro.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartView : ContentPage
    {
        public StartView()
        {
            InitializeComponent();
            BindingContext = App.GetViewModel<StartViewModel>();
        }

        private void HomePicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void RoomPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}