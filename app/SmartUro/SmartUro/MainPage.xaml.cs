using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SmartUro.Services;

namespace SmartUro
{
    public partial class MainPage : ContentPage
    {
        WebAPIService service;
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnOff_Clicked(object sender, EventArgs e)
        {
            service = new WebAPIService();
            await service.OnOff();
        }
    }
}
