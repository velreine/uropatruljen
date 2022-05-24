using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Text;
using Android.Content;
using Android.Net.Wifi;
using SmartUro.Interfaces;
using SmartUro.Services;

[assembly: Dependency(typeof(GetSSIDAndroid))]
namespace SmartUro.Services
{
    internal class GetSSIDAndroid : IGetSSID
    {
        public string GetSSID()
        {
            WifiManager wifiManager = (WifiManager)(Android.App.Application.Context.GetSystemService(Context.WifiService));

            if (wifiManager != null && !string.IsNullOrEmpty(wifiManager.ConnectionInfo.SSID))
            {
                return wifiManager.ConnectionInfo.SSID;
            }
            else
            {
                return "WiFiManager is NULL";
            }
        }
    }
}
