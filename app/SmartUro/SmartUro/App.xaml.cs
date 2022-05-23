using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartUro.Views;

namespace SmartUro
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            MainPage = new StartView();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
