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
            string text = ((Button)sender).Text;
            if (text == "ON")
            {
                text = "OFF";
            }
            else
            {
                text = "ON";
            }

            ((Button)sender).Text = text;

            //service = new WebAPIService();
            //await service.OnOff();
        }
    }
}
