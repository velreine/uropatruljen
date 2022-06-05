using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
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

    private readonly ICollection<Component> _components = new List<Component>();
    private readonly ICollection<Device> _devices = new List<Device>();

    public IReadOnlyCollection<Component> Components { get => (IReadOnlyCollection<Component>)_components; }
    public IReadOnlyCollection<Device> Devices { get => (IReadOnlyCollection<Device>)_devices; }
    
    [Obsolete("This constructor should only be used by Entity Framework and not in User-Land as using this constructor cannot guarantee a \"valid\" entity state.")]
    public HardwareLayout()
    {
        
    }

    public HardwareLayout(int? id,string productName, string modelNumber) : base(id)
    {
        ProductName = productName;
        ModelNumber = modelNumber;
    }
    
    public HardwareLayout(int? id,string productName, string modelNumber, ICollection<Component> components, ICollection<Device> devices) : base(id)
    {
        ProductName = productName;
        ModelNumber = modelNumber;
        _components = components;
        _devices = devices;
    }
    
    public HardwareLayout AddDevice(Device device)
    {
        // If the list already contains this device return and do nothing.
        if (_devices.Any(c => c.Id == device.Id)) return this;

        // Otherwise add the component.
        _devices.Add(device);
        // And also populate the inverse side.
        device.HardwareLayout = this;

        return this;
    }
    
    public HardwareLayout RemoveDevice(Device device)
    {
        // If the list contains the component, remove it.
        if (_devices.Any(c => c.Id == device.Id))
        {
            _devices.Remove(device);

            // And also update the inverse side (unless already changed.)
            if (device.HardwareLayout == this)
            {
                device.HardwareLayout = null;
            }
                
        }

        return this;
    }
    
    
    public HardwareLayout AddComponent(Component component)
    {
        // If the list already contains this component return and do nothing.
        if (_components.Any(c => c.Id == component.Id)) return this;

        // Otherwise add the component.
        _components.Add(component);
        // And also populate the inverse side.
        component.HardwareLayout = this;

        return this;
    }

    public HardwareLayout RemoveComponent(Component component)
    {
        // If the list contains the component, remove it.
        if (_components.Any(c => c.Id == component.Id))
        {
            _components.Remove(component);

            // And also update the inverse side (unless already changed.)
            if (component.HardwareLayout == this)
            {
                component.HardwareLayout = null;
            }
                
        }

        return this;
    }
    
}

}