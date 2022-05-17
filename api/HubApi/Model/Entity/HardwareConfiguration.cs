using HubApi.Model.Entity.Contracts;

namespace HubApi.Model.Entity;

/**
 * A hardware configuration consists of components, and the components pin-mapping etc.
 * This is necessary for software to automatically build a UserInterface around the hardware configuration,
 * removing the need for writing UI boilerplate code for each released piece of hardware.
 */
public class HardwareConfiguration : AbstractEntity
{
    // E.g. Smart Uro v0.1 (Green Edition) ... or similar product text.
    public string Name { get; set; }
    
    // A string that uniquely identifies this hardware configuration.
    public string Serialnumber { get; set; }

    public ICollection<Component> AttachedComponents { get; set; }



}