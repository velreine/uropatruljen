using System;
using System.Diagnostics;
using CoreFoundation;
using Network;
using NetworkExtension;
using SmartUro.Services;
using NWPathStatus = Network.NWPathStatus;

namespace SmartUro.iOS.Services
{
    public class iOSWiFiObserver : IWiFiObserver
    {

        private readonly NWPathMonitor _nwPathMonitor = new NWPathMonitor(NWInterfaceType.Wifi);
        private NEHotspotNetwork _currentHotspotNetwork;
        
        public iOSWiFiObserver()
        {
            
            // iOS needs a dispatch queue, not exactly sure why this is needed.
            var queue = new DispatchQueue("foobar");
            _nwPathMonitor.SetQueue(queue);
            _nwPathMonitor.Start();
            
            // This is called SnapshotHandler instead of pathUpdatedHandler (as it is in Swift?)
            _nwPathMonitor.SnapshotHandler = (path) =>
            {
                Debug.Print("SnapshotHandler()... Path is: " + (int)path.Status);

                // If the network is available.
                if (path.Status == NWPathStatus.Satisfied)
                {
                    NEHotspotNetwork.FetchCurrent((nw) => this._currentHotspotNetwork = nw);
                    OnDeviceWiFiConnected?.Invoke(this, EventArgs.Empty);
                }
                else // The network is unavailable.
                {
                    NEHotspotNetwork.FetchCurrent((nw) => this._currentHotspotNetwork = nw);
                    OnDeviceWiFiDisconnected?.Invoke(this, EventArgs.Empty);
                }
                
            };

        }

        // Common event handlers for our Android/iOS implementors.
        public event EventHandler OnDeviceWiFiDisconnected;
        public event EventHandler OnDeviceWiFiConnected;

        public bool? IsWifiCurrentlyConnected()
        {
            if (_nwPathMonitor.CurrentPath == null) return null;
            
            return _nwPathMonitor.CurrentPath.Status == NWPathStatus.Satisfied ? true : false;
        }

        public string GetCurrentSSID()
        {
            
            // Fetch the current hotspot network if it has not been initialized yet.
            if (this._currentHotspotNetwork == null)
            {
                NEHotspotNetwork.FetchCurrent((nw) =>
                {
                    var x = nw;
                    this._currentHotspotNetwork = nw;
                });
            }
            
            Debug.Print("The SSID of the current hotspot is: " + this._currentHotspotNetwork?.Ssid);
            
            // The FetchCurrent method of NEHotspotNetwork
            // Then return the SSID of the WiFI.
            return this._currentHotspotNetwork?.Ssid;
        }
    }
}