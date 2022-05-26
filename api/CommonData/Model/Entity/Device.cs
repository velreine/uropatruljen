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
        
        // The room this Device resides in.
        public Room Room { get; set; }
        
    }

}