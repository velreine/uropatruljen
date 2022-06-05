using System;
using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity {

/**
 * Represents the lowest demeanor for persisted component state.
 * This is because ALL components should be able to be Turned On and Off.
 * Therefore all other types of component state should derive from this base class.
 *
 * TODO: Abstract keyword is commented out because of Entity Framework.
 */
    public class ComponentState : AbstractEntity
    {
        
        public bool IsOn { get; set; }
        
        /** The device this saved state belongs to. **/
        public Device? Device { get; set; }
        
        /** The component this saved state belongs to. **/
        public Component? Component { get; set; }

        // EF needs those in case it does not join device and component.
        public int DeviceId { get; set; }
        public int ComponentId { get; set; }
        
        [Obsolete("This constructor should only be used by Entity Framework and not in User-Land as using this constructor cannot guarantee a \"valid\" entity state.")]
        public ComponentState() { }
        
        public ComponentState(int? id,bool isOn, Device? device, Component? component) : base(id)
        {
            IsOn = isOn;
            Device = device;
            Component = component;
        }
    }
}