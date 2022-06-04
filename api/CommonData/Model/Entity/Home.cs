using System.Collections.Generic;
using System.Collections.Immutable;
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
        public IImmutableList<Person> Residents { get => _residents.ToImmutableList(); }

        /**
        * OneToMany relation, inverse=Room.Home
        */
        public IImmutableList<Room> Rooms { get => _rooms.ToImmutableList(); }

        /**
        * OneToMany relation, inverse=Device.Home
        */
        public IImmutableList<Device> Devices { get => _devices.ToImmutableList(); } 
        
        public Home SetResidents(ICollection<Person> persons)
        {
            this._residents = persons;

            return this;
        }

        public Home SetRooms(ICollection<Room> rooms)
        {
            _rooms = rooms;

            return this;
        }

        public Home SetDevices(ICollection<Device> devices)
        {
            _devices = devices;

            return this;
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