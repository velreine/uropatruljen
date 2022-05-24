using System;

namespace SmartUro.Services
{
    public interface IWiFiObserver
    {
        event EventHandler OnDeviceWiFiDisconnected;
        event EventHandler OnDeviceWiFiConnected;
        bool? IsWifiCurrentlyConnected();
        string GetCurrentSSID();
    }
}