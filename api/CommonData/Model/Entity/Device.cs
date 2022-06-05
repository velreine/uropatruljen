using System;
using System.Collections.Generic;
using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity {
    
    /**
     * This is a device like the "smart uro", the name is chosen by the user.
     */
    public class Device : AbstractEntity
    {
        // A name the user has given to this device.
        public string Name { get; set; }
    
        // A string that uniquely identifies this device.
        public string SerialNumber { get; set; }
        
        // The physical layout that matches this device.
        public HardwareLayout HardwareLayout { get; set; }
        
        
        // When these are defined.
        // Entity Framework will populate these without needing to join data.
        // These fields can also be found on "Home.Id", "Room.Id", "Layout.Id" when these are populated.
        public int HomeId { get; set; }
        public int? RoomId { get; set; }
        
        public int HardwareLayoutId { get; set; }
        
        // The home this device should belong to.
        // Read additional comment on Room property.
        public Home Home { get; set; }
        
        // (Optionally the Room this Device should be in, this should be a Room of the attached home.)
        // Even though a Room is "inside" of a Home,
        // the "Home" property is necessary in case the device has not had a room attached yet.
        public Room? Room { get; set; }

        [Obsolete("This constructor should only be used by Entity Framework and not in User-Land as using this constructor cannot guarantee a \"valid\" entity state.")]
        public Device() { }

  
        
        public Device(int? id,string name, string serialNumber, HardwareLayout hardwareLayout, /*int homeId, int roomId,*/ Home home, Room? room) : base(id)
        {
            Name = name;
            SerialNumber = serialNumber;
            HardwareLayout = hardwareLayout;
            /*HomeId = homeId;
            RoomId = roomId;*/
            Home = home;
            Room = room;
        }
    }

}