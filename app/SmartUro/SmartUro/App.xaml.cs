using System;
using SmartUro.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartUro.Views;

namespace SmartUro
{
    public partial class App : Application
    {
        public static IWiFiObserver WiFiObserver { get; private set; }
        
        public App(IWiFiObserver wiFiObserver)
        {
            InitializeComponent();

            WiFiObserver = wiFiObserver;

            MainPage = new NavigationPage(new LoginView());
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
