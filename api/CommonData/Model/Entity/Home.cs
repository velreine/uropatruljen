using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity
{
    public class Home : AbstractEntity
    {
        public string Name { get; set; }
        
        private readonly ICollection<Person> _persons = new List<Person>();
        private readonly ICollection<Room> _rooms = new List<Room>();
        private readonly ICollection<Device> _devices = new List<Device>();

        /**
        * ManyToMany relation, inverse=Person.ConnectedHomes
        */
        public IReadOnlyCollection<Person> Persons => (IReadOnlyCollection<Person>)_persons;

        /**
        * OneToMany relation, inverse=Room.Home
        */
        public IReadOnlyCollection<Room> Rooms => (IReadOnlyCollection<Room>)_rooms;

        /**
        * OneToMany relation, inverse=Device.Home
        */
        public IReadOnlyCollection<Device> Devices => (IReadOnlyCollection<Device>)_devices;

        [Obsolete("This constructor should only be used by Entity Framework and not in User-Land as using this constructor cannot guarantee a \"valid\" entity state.")]
        public Home() {}
        
        public Home(int? id,string name) : base(id)
        {
            Name = name;
        }
        
        public Home(int? id,string name, ICollection<Person> persons, ICollection<Room> rooms, ICollection<Device> devices) : base(id)
        {
            Name = name;
            _persons = persons;
            _rooms = rooms;
            _devices = devices;
        }
        
        public Home AddPerson(Person person)
        {
            // If the list already contains this person return and do nothing.
            if (_persons.Any(p => p.Id == person.Id)) return this;

            // Otherwise add the person.
            _persons.Add(person);
            // And also populate the inverse side.
            person.AddHome(this);

            return this;
        }

        public Home RemovePerson(Person person)
        {
            // If the list contains the person, remove it.
            if (_persons.Any(p => p.Id == person.Id))
            {
                _persons.Remove(person);

                // And also update the inverse side.
                person.RemoveHome(this);
            }

            return this;
        }

        public Home AddDevice(Device device)
        {
            // If the list already contains this device return and do nothing.
            if (_devices.Any(d => d.Id == device.Id)) return this;

            // Otherwise add the device.
            _devices.Add(device);
            // And also populate the inverse side.
            device.Home = this;

            return this;
        }
        
        public Home RemoveDevice(Device device)
        {
            // If the list contains the device, remove it.
            if (_devices.Any(d => d.Id == device.Id))
            {
                _devices.Remove(device);

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
            if (_rooms.Any(r => r.Id == room.Id)) return this;

            // Otherwise add the room.
            _rooms.Add(room);
            // And also populate the inverse side.
            room.Home = this;

            return this;
        }
        
        public Home RemoveRoom(Room room)
        {
            // If the list contains the room, remove it.
            if (_rooms.Any(r => r.Id == room.Id))
            {
                _rooms.Remove(room);

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