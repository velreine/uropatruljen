using System;
using System.Diagnostics;
using CoreFoundation;
using Network;
using SmartUro.Services;

namespace SmartUro.iOS.Services
{
    public class iOSWiFiObserver : IWiFiObserver
    {

        private readonly NWPathMonitor _nwPathMonitor = new NWPathMonitor(NWInterfaceType.Wifi);

        public iOSWiFiObserver()
        {
            
            var queue = new DispatchQueue("foobar");
            _nwPathMonitor.SetQueue(queue);
            _nwPathMonitor.Start();
            
            _nwPathMonitor.SnapshotHandler = (path) =>
            {
                Debug.Print("SnapshotHandler()... Path is: " + (int)path.Status);

                if (path.Status == NWPathStatus.Satisfied)
                {
                    OnDeviceWiFiConnected?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    OnDeviceWiFiDisconnected?.Invoke(this, EventArgs.Empty);
                }
                
            };

        }


        public event EventHandler OnDeviceWiFiDisconnected;
        public event EventHandler OnDeviceWiFiConnected;

        public bool? IsWifiCurrentlyConnected()
        {
            if (_nwPathMonitor.CurrentPath == null) return null;
            
            return _nwPathMonitor.CurrentPath.Status == NWPathStatus.Satisfied ? true : false;
        }
    }
}