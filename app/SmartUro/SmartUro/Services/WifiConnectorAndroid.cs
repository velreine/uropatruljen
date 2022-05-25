using SmartUro.Services;
using SmartUro.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Android.Net.Wifi;
using Android.Content;
using System.Linq;
using System.Diagnostics;
using Android.Net;

[assembly: Dependency(typeof(WifiConnectorAndroid))]
namespace SmartUro.Services
{
    internal class WifiConnectorAndroid : IWiFiConnector
    {
        public void ConnectToWifi(string ssid, string password)
        {
            var _connectManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);

            

            /*
            var formattedSsid = $"\"{ssid}\"";
            var formattedPassword = $"\"{password}\"";

            var wifiConfig = new WifiConfiguration
            {
                Ssid = formattedSsid,
                PreSharedKey = formattedPassword
            };

            var addNetwork = wifiManager.AddNetwork(wifiConfig);
            var network = wifiManager.ConfiguredNetworks.FirstOrDefault(n => n.Ssid == ssid);

            if (network == null)
            {
                Debug.WriteLine($"Cannot connect to network: {ssid}");
                return;
            }

            wifiManager.Disconnect();
            var enableNetwork = wifiManager.EnableNetwork(network.NetworkId, true);*/
        }
    }
}
