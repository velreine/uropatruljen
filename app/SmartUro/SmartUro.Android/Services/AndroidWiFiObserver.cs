using Android.App;
using Android.Content;
using System;
using Android.Net;
using SmartUro.Interfaces;
using Xamarin.Essentials;

namespace SmartUro.Droid.Services
{
    internal class AndroidWiFiObserver : IWiFiObserver
    {
        private readonly ConnectivityManager _connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);

        public AndroidWiFiObserver()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (_connectivityManager.ActiveNetworkInfo != null)
                OnDeviceWiFiConnected?.Invoke(this, EventArgs.Empty);
            else
                OnDeviceWiFiDisconnected?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler OnDeviceWiFiDisconnected;
        public event EventHandler OnDeviceWiFiConnected;

        [Obsolete]
        public string GetCurrentSSID()
        {
            return _connectivityManager.ActiveNetworkInfo?.ExtraInfo;
        }

        [Obsolete]
        public bool? IsWifiCurrentlyConnected()
        {
            return _connectivityManager.ActiveNetworkInfo?.Type == ConnectivityType.Wifi ? true : false;
        }
    }
    
}