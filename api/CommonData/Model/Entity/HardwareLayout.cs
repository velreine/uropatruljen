using System.Collections.Generic;
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

    private ICollection<Component> _attachedComponents = new List<Component>();

    public ICollection<Component> AttachedComponents
    {
        get => _attachedComponents;
        set => _attachedComponents = value;
    }

    public HardwareLayout AddComponent(Component component)
    {
        // If the list already contains this component return and do nothing.
        if (AttachedComponents.Any(c => c.Id == component.Id)) return this;

        // Otherwise add the component.
        AttachedComponents.Add(component);
        // And also populate the inverse side.
        component.Layout = this;

        return this;
    }

    public HardwareLayout RemoveComponent(Component component)
    {
        // If the list contains the component, remove it.
        if (AttachedComponents.Any(c => c.Id == component.Id))
        {
            AttachedComponents.Remove(component);

            // And also update the inverse side (unless already changed.)
            if (component.Layout == this)
            {
                component.Layout = null;
            }
                
        }

        return this;
    }
    
}

}