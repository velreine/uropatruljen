using System;

namespace SmartUro.Interfaces
{
    public interface IWiFiObserver
    {
        event EventHandler OnDeviceWiFiDisconnected;
        event EventHandler OnDeviceWiFiConnected;
        bool? IsWifiCurrentlyConnected();
        string GetCurrentSSID();
    }
}