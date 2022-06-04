using System.Collections.Generic;
using System.Linq;
using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity
{
    public class Home : AbstractEntity
    {
        public string Name { get; set; }
        
        private ICollection<Person> _residents = new List<Person>();
        private ICollection<Room> _rooms = new List<Room>();
        private ICollection<Device> _devices = new List<Device>();

        /**
        * ManyToMany relation, inverse=Person.ConnectedHomes
        */
        public ICollection<Person> Residents
        {
            get => _residents;
            set => _residents = value;
        }

        /**
        * OneToMany relation, inverse=Room.Home
        */
        public ICollection<Room> Rooms { 
            get => _rooms;
            set => _rooms = value;
        }

        /**
        * OneToMany relation, inverse=Device.Home
        */
        public ICollection<Device> Devices
        {
            get => _devices; 
            set => _devices = value;
        } 

        public Home AddPerson(Person person)
        {
            // If the list already contains this person return and do nothing.
            if (Residents.Any(p => p.Id == person.Id)) return this;

            // Otherwise add the person.
            Residents.Add(person);
            // And also populate the inverse side.
            person.AddHome(this);

            return this;
        }

        public Home RemovePerson(Person person)
        {
            // If the list contains the person, remove it.
            if (Residents.Any(p => p.Id == person.Id))
            {
                Residents.Remove(person);

                // And also update the inverse side.
                person.RemoveHome(this);
            }

            return this;
        }

        public Home AddDevice(Device device)
        {
            // If the list already contains this device return and do nothing.
            if (Devices.Any(d => d.Id == device.Id)) return this;

            // Otherwise add the device.
            Devices.Add(device);
            // And also populate the inverse side.
            device.Home = this;

            return this;
        }
        
        public Home RemoveDevice(Device device)
        {
            // If the list contains the device, remove it.
            if (Residents.Any(d => d.Id == device.Id))
            {
                Devices.Remove(device);

                // And also update the inverse side (unless already changed).
                if (device.Home == this)
                {
                    device.Home = null;
                }
                
            }

            return this;
        }
        
        public Home AddRoom(Room room)
        {
            // If the list already contains this room return and do nothing.
            if (Devices.Any(r => r.Id == room.Id)) return this;

            // Otherwise add the room.
            Rooms.Add(room);
            // And also populate the inverse side.
            room.Home = this;

            return this;
        }
        
        public Home RemoveRoom(Room room)
        {
            // If the list contains the room, remove it.
            if (Residents.Any(r => r.Id == room.Id))
            {
                Rooms.Remove(room);

                // And also update the inverse side (unless already changed).
                if (room.Home == this)
                {
                    room.Home = null;
                }
                
            }

            return this;
        }
        
        
        
    }
}