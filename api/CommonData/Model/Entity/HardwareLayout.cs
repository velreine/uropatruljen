using System.Collections.Generic;
using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity {

/**
 * A hardware layout consists of components, and the components pin-mapping etc.
 * This is necessary for software to automatically build a User Interface around the hardware layout,
 * removing the need for writing UI boilerplate code for each released piece of hardware.
 */
public class HardwareLayout : AbstractEntity
{
    // E.g. Smart Uro v0.1 (Green Edition) ... or similar product text.
    public string ProductName { get; set; }
    
    // A string that uniquely identifies this hardware configuration.
    public string ModelNumber { get; set; }

    public ICollection<Component> AttachedComponents { get; set; } = new List<Component>();
}

}