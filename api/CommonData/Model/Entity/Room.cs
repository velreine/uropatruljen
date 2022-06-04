using System.Collections.Generic;
using System.Linq;
using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity {

   
    public class Room : AbstractEntity
    {
        public string Name { get; set; }

        public Home Home { get; set; }

        private ICollection<Device> _devices = new List<Device>();

        public ICollection<Device> Devices
        {
            get => _devices; 
            set => _devices = value;
        }

        public Room AddDevice(Device device)
        {
            // If the list already contains this device return and do nothing.
            if (Devices.Any(d => d.Id == device.Id)) return this;

            // Otherwise add the device.
            Devices.Add(device);
            // And also update the inverse side.
            device.Room = this;

            return this;
        }

        public Room RemoveDevice(Device device)
        {
            // If the list contains the room, remove it.
            if (Devices.Any(r => r.Id == device.Id))
            {
                Devices.Remove(device);

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