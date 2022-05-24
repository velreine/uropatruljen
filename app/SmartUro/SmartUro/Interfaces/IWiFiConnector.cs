using System;
using System.Collections.Generic;
using System.Text;

namespace SmartUro.Interfaces
{
    internal interface IWiFiConnector
    {
        void ConnectToWifi(string ssid, string password);
    }
}
