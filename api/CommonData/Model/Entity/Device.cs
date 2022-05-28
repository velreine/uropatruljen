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
        public HardwareLayout Layout { get; set; }
        
        // The home this device should belong to.
        // Read additional comment on Room property.
        public Home Home { get; set; }
        
        // (Optionally the Room this Device should be in, this should be a Room of the attached home.)
        // Even though a Room is "inside" of a Home,
        // the "Home" property is necessary in case the device has not had a room attached yet.
        public Room Room { get; set; }
        
    }

}