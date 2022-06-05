using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity {

   
    public class Room : AbstractEntity
    {
        public string Name { get; set; }

        public Home Home { get; set; }

        // This needs to be here only because of EF.
        public int HomeId { get; set; }
        
        private ICollection<Device> _devices = new List<Device>();

        public IReadOnlyCollection<Device> Devices { get => (IReadOnlyCollection<Device>)_devices; }
        
        [Obsolete("This constructor should only be used by Entity Framework and not in User-Land as using this constructor cannot guarantee a \"valid\" entity state.")]
        public Room() {}
        
        public Room(string name, Home home, ICollection<Device> devices)
        {
            Name = name;
            Home = home;
            _devices = devices;
        }

        public Room(string name, int homeId)
        {
            HomeId = homeId;
            Name = name;
        }
        
        public Room AddDevice(Device device)
        {
            // If the list already contains this device return and do nothing.
            if (_devices.Any(d => d.Id == device.Id)) return this;

            // Otherwise add the device.
            _devices.Add(device);
            // And also update the inverse side.
            device.Room = this;

            return this;
        }

        public Room RemoveDevice(Device device)
        {
            // If the list contains the room, remove it.
            if (_devices.Any(r => r.Id == device.Id))
            {
                _devices.Remove(device);

                // And also update the inverse side (unless already changed).
                if (device.Room == this)
                {
                    device.Room = null;
                }
                
            }

            return this;
        }
        
        
    }

}