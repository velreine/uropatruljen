using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public HardwareLayout HardwareLayout { get; set; }
        
        private readonly ICollection<Pin> _pins = new List<Pin>();
        
        public IReadOnlyCollection<Pin> Pins => (IReadOnlyCollection<Pin>)_pins;


        [Obsolete("This constructor should only be used by Entity Framework and not in User-Land as using this constructor cannot guarantee a \"valid\" entity state.")]
        public Component() { }
        
        public Component(int? id, string name, ComponentType type, HardwareLayout hardwareLayout, ICollection<Pin> pins) : base(id)
        {
            Name = name;
            Type = type;
            HardwareLayout = hardwareLayout;
            _pins = pins.ToList();

            _pins = new ObservableCollection<Pin>().ToList();

        }


        public Component AddPin(Pin pin)
        {
            // If the list already contains this pin return and do nothing.
            if (_pins.Any(p => p.Id == pin.Id)) return this;

            // Otherwise add the person.
            _pins.Add(pin);
            // And also populate the inverse side.
            pin.Component = this;

            return this;
        }
        
        public Component RemovePin(Pin pin)
        {
            // If the list contains the pin, remove it.
            if (_pins.Any(p => p.Id == pin.Id))
            {
                _pins.Remove(pin);

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