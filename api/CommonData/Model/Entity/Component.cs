using System.Collections.Generic;
using System.Linq;
using CommonData.Model.Entity.Contracts;
using CommonData.Model.Static;

namespace CommonData.Model.Entity {

/**
 * A component represents a physical hardware component that belongs to a hardware model/configuration.
 */
public class Component : AbstractEntity
{
        public string Name { get; set; }

        public ComponentType Type { get; set; }

        /**
         * ManyToOne HardwareLayout, inverse=HardwareLayout.AttachedComponents
         */
        public HardwareLayout Layout { get; set; }
        
        private ICollection<Pin> _pins = new List<Pin>();
        
        public ICollection<Pin> Pins { 
            get => _pins;
            set => _pins = value;
        }

        public Component AddPin(Pin pin)
        {
            // If the list already contains this pin return and do nothing.
            if (Pins.Any(p => p.Id == pin.Id)) return this;

            // Otherwise add the person.
            Pins.Add(pin);
            // And also populate the inverse side.
            pin.Component = this;

            return this;
        }
        
        public Component RemovePin(Pin pin)
        {
            // If the list contains the pin, remove it.
            if (Pins.Any(p => p.Id == pin.Id))
            {
                Pins.Remove(pin);

                // And also update the inverse side (unless already changed.)
                if (pin.Component == this)
                {
                    pin.Component = null;
                }
                
            }

            return this;
        }
        
        
}
}